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
    }
}
