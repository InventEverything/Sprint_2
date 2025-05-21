namespace BugTracker.Core
{ 
    public class Bug
    {
        public int BugId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BugPriority Priority { get; private set; }
        public BugSeverity Severity { get; private set; }
        public BugStatus Status { get; private set; }
        public string? AssignedToDeveloper { get; set; }

        // Constructor for Bug class, initializes properties and sets default values *
        public Bug(int bugId, string title, string description, int priority, int severity)
        {
            BugPriority newPriority;
            BugSeverity newSeverity;
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null, empty, or whitespace.");

            newPriority = SetPriority(priority); // Convert the priority to their respective enums
            newSeverity = SetSeverity(severity); // Convert the severity to their respective enums

            BugId = bugId;
            Title = title;
            Description = description;
            Priority = newPriority;
            Severity = newSeverity;
            Status = BugStatus.Open;
            AssignedToDeveloper = null;
        }
      
    #region ** Changing Data Methods **
        // Update the bug's status or checks if Status already exisits *
        public void UpdateStatus(BugStatus newStatus)
        {
            if (newStatus == Status)
                throw new ArgumentException("Status is already set to the same value.");
            else
            {
                switch (newStatus)
                {
                    case BugStatus.Closed:
                        if (Status == BugStatus.InProgress || Status == BugStatus.Pending)
                        {
                            Status = newStatus;
                        }
                        break;
                    case BugStatus.InProgress:
                        if (Status == BugStatus.Open || Status == BugStatus.Pending)
                        {
                            Status = newStatus;
                        }
                        break;
                    case BugStatus.Pending:
                        if (Status == BugStatus.InProgress)
                        {
                            Status = newStatus;
                        }
                        break;
                }
            }
        }

        // Update the bug's priority or checks if Priority already exisits *
        public BugPriority SetPriority(int priority)
        {
            BugPriority newPriority = (BugPriority)priority; // Convert the priority to their respective enums
            if (newPriority != BugPriority.High &&
                newPriority != BugPriority.Medium &&
                newPriority != BugPriority.Low)
                throw new ArgumentException("Priority is not a valid value.");
            else
                return newPriority;
        }

        // Update the bug's severity or checks if Severity already exisits *
        public BugSeverity SetSeverity(int severity)
        {
            BugSeverity newSeverity = ((BugSeverity)severity); // Convert the priority to their respective enums

            if (newSeverity != BugSeverity.Trivial &&
                newSeverity != BugSeverity.Minor &&
                newSeverity != BugSeverity.Major &&
                newSeverity != BugSeverity.Critical)
                throw new ArgumentException("Severity is not a valid value.");
            else
                return newSeverity;
        }

        // Assigns the bug to developer to AssignToDeveloper via string *
        public void UpdateAssignedToDeveloper(string developer)
        {
            AssignedToDeveloper = developer;
        }

        // Assign a bug to a developer
        public string AssignToDeveloper(Bug bug, string developerName)
        {
            if (string.IsNullOrWhiteSpace(developerName))
            {
                return ("Invalid input");
            }

            if (!string.IsNullOrWhiteSpace(bug.AssignedToDeveloper))
            {
                return ("This bug is already assigned to a developer.");
            }

            bug.AssignedToDeveloper = developerName;
            return ($"Bug: {bug.BugId} has been successfully assigned to developer: {bug.AssignedToDeveloper}");
        }
        #endregion

    }

    #region ** Enums **
    public enum BugStatus //* Enum for Bug Status, 4 levels: Open, InProgress, Resolved, Closed **
    {
        Open,
        InProgress,
        Pending,
        Closed
    }

    public enum BugPriority //* Enum for Bug Priority, 3 levels: Low, Medium, High **
    {
        Low,
        Medium,
        High
    }

    public enum BugSeverity //* Enum for Bug Severity, 4 levels: Trivial, Minor, Major, Critical **
    {
        Trivial,
        Minor,
        Major,
        Critical
    }
    #endregion
}
