using BugTracker.Core;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Tests
{
    public class ImprovedTest
    {

        #region **Create Bug UI Tests**
        [Fact]
        public void CreateBug_WithValidInput_DisplaysAllQuestions()
        {
            //Arrange
            var sw = new StringWriter();
            var testBugService = new BugService(); // Assuming you have a BugService that handles bug creation
            var menu = new BugMenuUI(sw, testBugService); // Assumes constructor _output and bugService are set up correctly

            //Mock the CreateBug method to simulate user input
            var inputs = new Queue<string>(new[]
            {
                "Test Bug",             // title 
                "This is a test bug",   // description
                "1",                    // priority
                "1",                    // severity
            });

            Func<string> inputProvider = () => inputs.Dequeue(); // Mock input provider

            //Act
            menu.CreateBug(inputProvider, skipClear: true);
            var output = sw.ToString(); // Capture output

            //Assert
            Assert.Contains("What is the bug's title?", output);
            Assert.Contains("What is the bug's description?", output);
            Assert.Contains("What is the bug's priority?", output);
            Assert.Contains("What is the bug's severity?", output);
        }

        [Theory]
        [InlineData(new[] { "abc", "5", "0" }, 0)] // Invalid, Invalid, Valid
        [InlineData(new[] { "-1", "3", "1" }, 1)]  // Invalid, Invalid, Valid
        [InlineData(new[] { "1" }, 1)]            // First try valid
        [InlineData(new[] { "2", "0", "1" }, 2)]  // First valid is 2
        public void PriorityCatch_WithVariousInputs_ReturnsValidPriority(string[] simulatedInputs, int expected)
        {
            // Arrange
            var sw = new StringWriter();
            var menu = new BugMenuUI(sw);
            var inputs = new Queue<string>(simulatedInputs);
            Func<string> inputProvider = () => inputs.Dequeue();

            // Act
            int result = menu.PriorityCatch(inputProvider, skipClear: true);
            string output = sw.ToString();

            // Assert
            Assert.Equal(expected, result);
            Assert.Contains("What is the bug's priority?", output);
        }

        [Theory]
        [InlineData(new[] { "abc", "5", "0" }, 0)] // Invalid, Invalid, Valid
        [InlineData(new[] { "-1", "9", "3" }, 3)]  // Invalid, Invalid, Valid
        [InlineData(new[] { "1" }, 1)]            // First try valid
        [InlineData(new[] { "2", "0", "1" }, 2)]  // First valid is 2
        public void SeverityCatch_WithVariousInputs_ReturnsValidPriority(string[] simulatedInputs, int expected)
        {
            // Arrange
            var sw = new StringWriter();
            var menu = new BugMenuUI(sw);
            var inputs = new Queue<string>(simulatedInputs);
            Func<string> inputProvider = () => inputs.Dequeue();

            // Act
            int result = menu.SeverityCatch(inputProvider, skipClear: true);
            string output = sw.ToString();

            // Assert
            Assert.Equal(expected, result);
            Assert.Contains("What is the bug's severity?", output);
        }

        #endregion

        #region **DateTime Tests**

        #region **Constructor Tests**
        [Fact] // Checks that the bug constructor initializes the DateCreated property to now.
        public void Constructor_DateCreated_IsSetToNow()
        {
            // Arrange
            var before = DateTime.UtcNow;

            // Act
            var bug = new Bug(6, "Test", "Test", 0, 1);

            var after = DateTime.UtcNow;

            // Assert
            Assert.InRange(bug.DateCreated, before, after);
        }
        #endregion

        #region **Set Time Tests**
        [Fact] // Checks that the DateClosed property is null when bug is created.
        public void Constructor_DateClosed_IsNull()
        {
            // Arrange & Act
            var bug = new Bug(6, "Test", "Test", 0, 1);
            // Assert
            Assert.Null(bug.DateClosed);
        }
        
        [Theory] // Validates that DateClosed only sets when status is Closed
        [InlineData(BugStatus.Pending, false)]
        [InlineData(BugStatus.InProgress, false)]
        [InlineData(BugStatus.Closed, true)]
        public void UpdateStatus_SetsDateClosedOnlyWhenStatusIsClosed(BugStatus status, bool shouldSetDateClosed)
        {
            // Arrange
            var bug = new Bug(6, "Test", "Test", 0, 1);

            // Prepare valid status path before calling the target status
            if (status == BugStatus.Pending)
            {
                bug.UpdateStatus(BugStatus.InProgress);
            }
            else if (status == BugStatus.Closed)
            {
                bug.UpdateStatus(BugStatus.InProgress);
                bug.UpdateStatus(BugStatus.Pending);
            }

            var beforeUpdate = DateTime.UtcNow;

            // Act — apply the actual test status
            bug.UpdateStatus(status);

            var afterUpdate = DateTime.UtcNow;

            // Assert
            if (shouldSetDateClosed)
            {
                Assert.Equal(BugStatus.Closed, bug.Status);
                Assert.NotNull(bug.DateClosed);
                Assert.InRange(bug.DateClosed.Value, beforeUpdate, afterUpdate);
            }
            else
            {
                Assert.NotEqual(BugStatus.Closed, bug.Status); // Sanity check
                Assert.Null(bug.DateClosed);
            }
        }

        #endregion

        #region **Close Time Tests**
        [Fact] // Checks that the DateClosed property is set when bug is closed.
        public void UpdateStatus_Closed_SetsDateClosed()
        {
            // Arrange
            var bug = new Bug(6, "Test", "Test", 0, 1);

            bug.UpdateStatus(BugStatus.InProgress); // Valid transition from Open
            bug.UpdateStatus(BugStatus.Pending);    // Valid transition from InProgress

            var before = DateTime.UtcNow;

            // Act
            bug.UpdateStatus(BugStatus.Closed);     // Valid transition from Pending

            var after = DateTime.UtcNow;

            // Assert
            Assert.Equal(BugStatus.Closed, bug.Status);
            Assert.NotNull(bug.DateClosed);
            Assert.InRange(bug.DateClosed.Value, before, after);
        }
        #endregion

        #endregion
    }
}
