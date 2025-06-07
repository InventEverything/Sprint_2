namespace BugTracker.Tests
{
    using BugTracker.Core;
    public class BugTests
    {
        #region ** Constructor Tests **
        [Fact] // checks to see if the constructor is valid
        public void ValidConstructorTest()
        {
            Bug bug = new Bug(0, "Constructor", "Valid constructor test", 0, 1);
            Assert.IsType<Bug>(bug);
        }

        [Fact] // checks to see if status is changed to closed status
        public void UpdateStatusTestClosed()
        {
            Bug bug = new Bug(4, "Closed", "Tests Closed status", 0, 0);
            bug.UpdateStatus((BugStatus)1);
            bug.UpdateStatus((BugStatus)3);
            Assert.Equal(BugStatus.Closed, bug.Status);
        }

        [Fact] // checks to see if status is changed to inprogress status
        public void UpdateStatusTestInProgress()
        {
            Bug bug = new Bug(2, "InProgress", "Tests InProgress status", 1, 1);
            bug.UpdateStatus((BugStatus)1);
            Assert.Equal(BugStatus.InProgress, bug.Status);
        }

        [Fact] // checks to see if status is changed to pending status
        public void UpdateStatusTestPending()
        {
            Bug bug = new Bug(3, "Pending", "Tests Resolved status", 0, 0);
            bug.UpdateStatus((BugStatus)1);
            bug.UpdateStatus((BugStatus)2);
            Assert.Equal(BugStatus.Pending, bug.Status);
        }

        [Fact] // checks to see if the constructor initializes correctly
        public void Constructor_ValidInput_InitializesCorrectly()
        {
            // Arrange & Act
            var bug = new Bug(5, "Login fails", "Fails with correct credentials", 2, 3);

            // Assert
            Assert.Equal(5, bug.BugId);
            Assert.Equal("Login fails", bug.Title);
            Assert.Equal("Fails with correct credentials", bug.Description);
            Assert.Equal(BugPriority.High, bug.Priority);
            Assert.Equal(BugSeverity.Critical, bug.Severity);
            Assert.Equal(BugStatus.Open, bug.Status);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")] // checks to see if title is null or empty, and the exception is thrown
        public void Constructor_InvalidTitle_ThrowsArgumentException(string invalidTitle)
        {
            // Arrange, Act & Assert
            // TODO: Uncomment this after implementing the guard clause
            var ex = Assert.Throws<ArgumentException>(() => new Bug(1, invalidTitle, "desc", 0, 1));
            Assert.Equal("Title cannot be null, empty, or whitespace.", ex.Message);
        }
        #endregion
        #region ** UpdateStatus Tests **

        [Fact]
        public void StatusChange_OpenToInProgress_ShouldChange()
        {
            // Arrange
            var bug = new Bug(10, "OpenToInProgress", "Should succeed", 0, 1);

            // Act
            bug.UpdateStatus(BugStatus.InProgress);

            // Assert
            Assert.Equal(BugStatus.InProgress, bug.Status);
        }

        [Theory]
        [InlineData(BugStatus.Pending)]
        [InlineData(BugStatus.Closed)]
        public void StatusChange_InProgressToPendingOrClosed_ShouldChange(BugStatus bugStatus)
        {
            // Arrange
            var bug = new Bug(11, "InProgressToPendingOrClosed", "Should succeed", 0, 1);

            // Act
            bug.UpdateStatus(BugStatus.InProgress);
            bug.UpdateStatus(bugStatus);

            // Assert
            Assert.Equal(bugStatus, bug.Status);
        }

        [Theory]
        [InlineData(BugStatus.InProgress)]
        [InlineData(BugStatus.Closed)]
        public void StatusChange_PendingToInProgressOrClosed_ShouldChange(BugStatus bugStatus)
        {
            // Arrange
            var bug = new Bug(12, "PendingToInProgressOrClosed", "Should succeed", 0, 1);

            // Act
            bug.UpdateStatus(BugStatus.InProgress);
            bug.UpdateStatus(BugStatus.Pending);
            bug.UpdateStatus(bugStatus);

            // Assert
            Assert.Equal(bugStatus, bug.Status);
        }

        [Theory]
        [InlineData(BugStatus.Open)]
        [InlineData(BugStatus.Pending)]
        public void StatusChange_ClosedToOpenOrPending_ShouldNotChange(BugStatus bugStatus)
        {
            // Arrange
            var bug = new Bug(13, "ClosedToOpenOrPending", "Should not succeed", 0, 1);

            // Act
            bug.UpdateStatus(BugStatus.InProgress);
            bug.UpdateStatus(BugStatus.Pending);
            bug.UpdateStatus(BugStatus.Closed);
            bug.UpdateStatus(bugStatus);

            // Assert
            Assert.Equal(BugStatus.Closed, bug.Status);
        }
        [Fact] // checks to see if status is changed to new status
        public void UpdateStatus_ChangesStatus_ToNewStatus()
        {
            // Arrange
            var bug = new Bug(6, "Icon missing", "Settings icon not visible", 0, 1);

            // Act
            bug.UpdateStatus(BugStatus.InProgress);

            // Assert
            Assert.Equal((BugStatus)1, bug.Status);
        }

        [Fact] // checks to see if status is changed from old status
        public void UpdateStatus_ChangesStatus_FromOldStatus()
        {
            // Arrange
            var bug = new Bug(8, "Icon missing", "Settings icon not visible", 0, 1);
            var initialStatus = bug.Status;
            // Act
            bug.UpdateStatus(BugStatus.InProgress);

            // Assert
            Assert.NotEqual(initialStatus, bug.Status);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)] // checks to see if same status exception is thrown
        public void UpdateStatus_ToSameStatus_ThrowsArgumentException(int status)
        {
            // Arrange
            var bug = new Bug(7, "StatusTest", "Status must change to a new status when updated", 0, 1);
            switch (status)
            {
                case 0:
                    break;
                case 1:
                    // Assignes the bug status to the same value as the one being tested
                    bug.UpdateStatus((BugStatus)status);
                    break;
                case 2:
                    // Assignes the bug status to the same value as the one being tested
                    bug.UpdateStatus((BugStatus)1);
                    bug.UpdateStatus((BugStatus)status);
                    break;
                case 3:
                    // Assignes the bug status to the same value as the one being tested
                    bug.UpdateStatus((BugStatus)1);
                    bug.UpdateStatus((BugStatus)2);
                    bug.UpdateStatus((BugStatus)status);
                    break;
            }
            // Act
            try
            {
                bug.UpdateStatus((BugStatus)status);
            }
            catch (ArgumentException ex)
            {
                Assert.Equal("Status is already set to the same value.", ex.Message);
            }
        }
        #endregion
        #region ** UpdateAssignToDeveloper Tests **
        [Fact] // checks to AssignedToDeveloper is null
        public void Constructor_AssignedToDeveloper_DefaultsToNull()
        {
            // Arrange & Act
            var bug = new Bug(9, "Login fails", "Fails with correct credentials", 0, 1);
            // Assert
            Assert.Null(bug.AssignedToDeveloper);
        }

        [Fact] // checks to see if AssignedToDeveloper is assigns a developer correctly
        public void UpdateAssignedToDeveloper_ChangesAssignedToDeveloper()
        {
            // Arrange
            var bug = new Bug(10, "Login fails", "Fails with correct credentials", 0, 1);
            string developerName = "John Doe";
            // Act
            bug.UpdateAssignedToDeveloper(developerName);
            // Assert
            Assert.Equal(developerName, bug.AssignedToDeveloper);
        }
        #endregion
        #region ** SetPriority Tests **
        [Fact] // checks to see if priority sets correctly
        public void SetPriority_ValidPriority_SetsPriority()
        {
            // Arrange
            var bug = new Bug(11, "Test", "Test", 0 /*Act*/, 0);
            // Assert
            Assert.Equal(BugPriority.Low, bug.Priority);
        }

        [Fact] // Checks to see if Invalid entry exeption is thrown
        public void SetPriority_InvalidPriority_ThrowsArgumentException()
        {
            // Arrange
            var bug = new Bug(14, "Test", "Test", 0, 0);
            // Act
            var ex = Assert.Throws<ArgumentException>(() => bug.SetPriority(4));
            // Assert
            Assert.Equal("Priority is not a valid value.", ex.Message);
        }

        [Fact] // Checks if the priority is set to low
        public void SetPriority_SetsPriorityToLow()
        {
            // Arrange
            var bug = new Bug(15, "Test", "Test", 0 /*Act*/, 0);
            // Assert
            Assert.Equal(BugPriority.Low, bug.Priority);
        }

        [Fact] // Checks if the priority is set to medium
        public void SetPriority_SetsPriorityToMedium()
        {
            // Arrange
            var bug = new Bug(16, "Test", "Test", 1 /*Act*/, 0);
            // Assert
            Assert.Equal(BugPriority.Medium, bug.Priority);
        }

        [Fact] // Checks if the priority is set to high
        public void SetPriority_SetsPriorityToHigh()
        {
            // Arrange
            var bug = new Bug(17, "Test", "Test", 2 /*Act*/, 0);
            // Assert
            Assert.Equal(BugPriority.High, bug.Priority);
        }
        #endregion
        #region ** SetSeverity Tests **
        [Fact] // Checks to see if severity sets correctly
        public void SetSeverity_ValidSeverity_SetsSeverity()
        {
            // Arrange
            var bug = new Bug(18, "Test", "Test", 2, 2 /*Act*/);
            // Assert
            Assert.Equal(BugSeverity.Major, bug.Severity);
        }

        [Fact] // Checks to see if Invalid entry exception is thrown
        public void SetSeverity_InvalidSeverity_ThrowsArgumentException()
        {
            // Arrange
            var bug = new Bug(21, "Test", "Test", 0, 0);
            // Act
            var ex = Assert.Throws<ArgumentException>(() => bug.SetSeverity(4));
            // Assert
            Assert.Equal("Severity is not a valid value.", ex.Message);
        }

        [Fact] // Checks if the severity is set to trivial
        public void SetSeverity_SetsSeverityToTrivial()
        {
            // Arrange
            var bug = new Bug(22, "Test", "Test", 0, 0 /*Act*/);
            // Assert
            Assert.Equal(BugSeverity.Trivial, bug.Severity);
        }

        [Fact] // Checks if the severity is set to minor
        public void SetSeverity_SetsSeverityToMinor()
        {
            // Arrange
            var bug = new Bug(23, "Test", "Test", 1, 1 /*Act*/);
            // Assert
            Assert.Equal(BugSeverity.Minor, bug.Severity);
        }

        [Fact] // Checks if the severity is set to major
        public void SetSeverity_SetsSeverityToMajor()
        {
            // Arrange
            var bug = new Bug(24, "Test", "Test", 2, 2 /*Act*/);
            // Assert
            Assert.Equal(BugSeverity.Major, bug.Severity);
        }

        [Fact] // Checks if the severity is set to critical
        public void SetSeverity_SetsSeverityToCritical()
        {
            // Arrange
            var bug = new Bug(25, "Test", "Test", 2, 3 /*Act*/);
            // Assert
            Assert.Equal(BugSeverity.Critical, bug.Severity);
        }
        #endregion
        #region ** CreateBug Tests **
        [Fact] // checks to see if bug is created correctly
        public void CreateBug_ValidInput_CreatesBug()
        {
            // Arrange
            var bugService = new BugService();
            // Act
            var bug = bugService.CreateBug("Icon Error", "Missing Icon", 0, 0);
            // Assert
            Assert.IsType<Bug>(bug);
        }
        #endregion
        #region ** Method AssignToDeveloper Tests **

        [Fact]
        public void AssignToDeveloper_WhenBuggAlreadyAssigned_ReturnsAlreadyAssignedMessage()
        {
            // Arrange
            var bug = new Bug(5, "title", "description", (int)BugPriority.High, (int)BugSeverity.Critical);
            bug.AssignedToDeveloper = "Paul";

            // Act
            var result = bug.AssignToDeveloper(bug, "Jean");

            // Assert
            Assert.Equal("This bug is already assigned to a developer.", result);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AssignToDeveloper_WhenDeveloperNameIsNullOrEmpty_ReturnsInvalidInputMessage(string developerName)
        {
            var bug = new Bug(5, "title", "description", (int)BugPriority.High, (int)BugSeverity.Critical);

            var result = bug.AssignToDeveloper(bug, developerName);

            Assert.Equal("invalid input", result, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void AssignToDeveloper_WithValidDeveloperName_UpdatesAssignToDeveloperProperty()
        {
            var bug = new Bug(5, "title", "description", (int)BugPriority.High, (int)BugSeverity.Critical);
            var result = bug.AssignToDeveloper(bug, "Paul");
            Assert.Equal("paul", bug.AssignedToDeveloper, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void AssignToDeveloper_WithhValidDevelperName_ReturnsSuccessfulAssignmentMessage() 
        {
            var bug = new Bug(5, "title", "description", (int)BugPriority.High, (int)BugSeverity.Critical);
            var result = bug.AssignToDeveloper(bug, "Paul");
            Assert.Equal("Bug: 5 has been successfully assigned to developer: paul", result, StringComparer.OrdinalIgnoreCase);
        }
        #endregion
    }
    public class BugServiceTests
    {
        [Fact]
        public void DeleteBug_ShouldRemoveBug_WhenBugExists()
        {
            // Arrange
            var service = new BugService();
            var bug = service.CreateBug("Test Bug", "Sample description", 1, 2);
            int bugId = bug.BugId;

            // Act
            bool result = service.DeleteBug(bugId);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(service.getBugs, b => b.BugId == bugId);
        }

        [Fact]
        public void DeleteBug_ShouldReturnFalse_WhenBugDoesNotExist()
        {
            // Arrange
            var service = new BugService();
            int nonExistentBugId = 999;

            // Act
            bool result = service.DeleteBug(nonExistentBugId);

            // Assert
            Assert.False(result);
        }
    }
}
