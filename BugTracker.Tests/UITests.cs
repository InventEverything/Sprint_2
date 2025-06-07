using System;
using System.IO;

namespace BugTracker.Tests
{

    using BugTracker.Core;
    using System.Text;

    public class UITests
    {
        // Encodes and captures the output of the console
        private class TestOutput : TextWriter
        {
            public override Encoding Encoding => Encoding.UTF8; // <-- This is the encoding used for the output
            public StringWriter StringWriter = new StringWriter(); // <- This is the StringWriter used to capture the output
            public override void WriteLine(string? value) => StringWriter.WriteLine(value);
            public override void Write(string? value) => StringWriter.Write(value);
        }

        #region ** Display Menu Tests **
        [Fact] // Checks to see if the menu displays correctly
        public void DisplayMenu_DisplaysMenuText()
        {
            // Arrange  
            var sw = new StringWriter(); // Create the writer FIRST
            var menu = new BugMenuUI(sw); // Inject the menu here

            // Act  
            menu.DisplayMenu(() => 'X', skipClear: true); // Simulates invalid input  

            // Capture output
            var output = sw.ToString();

            // Optionally, print to console to debug
            Console.WriteLine("Captured Output:");
            Console.WriteLine(output);

            // Assert  
            Assert.Contains("BUG TRACKER MENU", output);
            Assert.Contains("(C) Create Bug", output);
            Assert.Contains("Invalid option", output);
        }

        [Fact] // Checks to see if input routs to the correct method and pushes the correct action
        public void DisplayMenu_WhenInputIsC_CallsOnCreateBug()
        {
            // Arrange
            var output = new TestOutput();
            var menu = new BugMenuUI(output);
            bool createBugCalled = false;
            menu.OnCreateBug = () => createBugCalled = true;

            // Act
            menu.DisplayMenu(() => 'C', skipClear: true);

            // Assert
            Assert.True(createBugCalled);
        }


        #endregion

        #region ** Bug Details Test **

        [Fact] //Checks to see that all the proper data from creation of bug shows up
        public void DisplayBugDetails_DisplaysCorrectDetails()
        {
            // Arrange
            var bug = new Bug(1, "Test Bug", "This is a test bug", 1, 1);
            var output = new StringWriter();
            var ticket = new BugMenuUI(output); // Pass the output to the class

            // Act
            ticket.DisplayBugDetails(bug, skipClear: true); // Skip Console.Clear and ReadKey
            string printedOutput = output.ToString();

            // Assert
            Assert.Contains("BUG  TICKET", printedOutput);
            Assert.Contains("Test Bug", printedOutput);
            Assert.Contains("This is a test bug".Substring(0, 18), printedOutput); // Truncated output
            Assert.Contains("1", printedOutput);
            Assert.Contains("1", printedOutput);
        }

        #endregion

        #region ** View Bugs Tests **
        [Fact]
        public void ViewBugs_SortedByTitle_OrderShouldChange()
        {
            // Arrange
            var bugs = new List<Bug>();
            var output = new TestOutput();
            var userView = new BugMenuUI(output); // Pass the output to the class

            // Act
            bugs = userView.SortedContent(ConsoleKey.T, null, true);

            // Assert
            Assert.Equal(bugs[0].BugId, 2);
            Assert.Equal(bugs[1].BugId, 3);
            Assert.Equal(bugs[2].BugId, 1);
        }
        [Fact]
        public void ViewBugs_SortedByStatus_OrderShouldChange()
        {
            // Arrange
            var bugs = new List<Bug>();
            var output = new TestOutput();
            var userView = new BugMenuUI(output); // Pass the output to the class

            // Act
            bugs = userView.SortedContent(ConsoleKey.S, null, true);

            // Assert
            Assert.Equal(bugs[0].BugId, 1);
            Assert.Equal(bugs[1].BugId, 3);
            Assert.Equal(bugs[2].BugId, 2);
        }
        #endregion

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
