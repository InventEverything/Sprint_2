Bug status tracker

============================================================

Bugs have ID's, Titles, Descriptions, and Status'.
Newly created bugs will always default to being Open.

Status' can be any of the following Open, InProgress, Resolved, Closed.
Status' can be changed from any status to any other status but not to the same status that it already is.

============================================================
### B3: Assign a Bug to a Developer

I implemented the AssignToDeveloper method, a feature that allows users to assign bugs to developers efficiently

# How It Maps to the User Story:
· User Story: As a user, I want to assign a bug to a selected developer.

# Implementation:
- The method checks input validity to ensure a developer is provided.
- It assigns the bug to the selected developer, updating its property.
- Prevents reassignment if a bug is already assigned

# Tests
1. AssignToDeveloper_WhenBugAlreadyAssigned_ReturnsAlreadyAssignedMessage
Purpose: Ensures that if a bug is already assigned to a developer, reassigning it returns the correct message.
Validation: The method should return "This bug is already assigned to a developer." when an attempt is made to assign it again.

2. AssignToDeveloper_WhenDeveloperNameIsNullOrEmpty_ReturnsInvalidInputMessage
Purpose: Verifies that an invalid developer name (null, empty, or whitespace) results in an appropriate error message.
The methodd should return "invalid input" when given a blank developer name

3. AssignToDeveloper_WithValidDeveloperName_UpdatesAssignToDeveloperProperty
Purpose: Ensures that a valid developer name correctly updates the AssignedToDeveloper property of the Bug object.
Validation: The assigned developer should match the input name after execution.

4. AssignToDeveloper_WithValidDeveloperName_ReturnsSuccessfulAssignmentMessage
Purpose: Confirms that assigning a bug with a valid developer name returns a success message
