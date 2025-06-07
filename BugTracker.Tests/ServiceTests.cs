namespace BugTracker.Tests
{
    using BugTracker.Core;

    public class ServiceTests
    {
        [Fact] // Checks if after creation of bug that it properly adds to internal list. 
        public void CreateBug_AddsBugToList_WithCorrectDetails()
        {
            // Arrange  
            var bugManager = new BugService();

            // Act  
            var bug = bugManager.CreateBug("Test Title", "Test Description", 2, 1);

            // Assert  
            Assert.NotNull(bug);
            Assert.Equal(4, bug.BugId); // Should be the 4th bug because 3 bugs are created as seeds by default
            Assert.Equal("Test Title", bug.Title);
            Assert.Equal("Test Description", bug.Description);
            Assert.Equal(BugPriority.High, bug.Priority);
            Assert.Equal(BugSeverity.Minor, bug.Severity);

            // Verify it was added to the internal list  
            var allBugs = bugManager.getBugs;
            Assert.Same(bug, allBugs[3]);
        }

        [Fact] // Checks that bugs are assigned the right index after being created.
        public void CreateBug_AssignsIncrementingBugId()
        {
            // Arrange  
            var bugManager = new BugService();

            // Act  
            var bug1 = bugManager.CreateBug("Bug 1", "Desc 1", (int)BugPriority.Low, 1);
            var bug2 = bugManager.CreateBug("Bug 2", "Desc 2", (int)BugPriority.High, 2);

            // Assert  
            Assert.Equal(4, bug1.BugId);
            Assert.Equal(5, bug2.BugId);
        }
    }
}
