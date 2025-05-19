# B1-Create Bug (Fully implemented)
# B2-View Bug List (Fully implemented)
# B3-Assign to Developer (Started but incomplete)
    Reason: I believe communication and schedule alignments prevented full implementation. The Assign to developer assignment works properly and has valid tests, the requirement at this point is to create a page that redirects from the MenuUI and accepts bugID and developer to be assigned and then displaying a confirmation or failed to assign message of some sort.
# B5-Delete Bug (Started but incomplete)
    Reason: I believe communication and schedule alignments prevented full implementation. The Delete bug function works properly and has valid tests, the requirements at this point is to create a page that redirects from the MenuUI and accepts a bugID to be deleted and then displays a confirmation or failure message of some sort.
# B6-MenuUI (Started but incomplete)
    Reason: MenuUI was not innitially on the sprint plan and was added later to allow an entry point into each of our sprint stories, for this reason each persons sprint story needed the MenuUI to be completed before they could implemnet fully. Each person fully created the methods and tests they needed to complete the tasks of their sprint. Assign to developer and Delete bug still need pages that redirect from MenuUI to be fully implemented.


Bug status tracker

# Sprint 2 – Bug Tracker Implementation

## Team Members:
- (Ian Engle, Chris Cane, Jean Paul Sidibe, Todd Holloman)

## Sprint Goal:
(Summarize your Sprint Goal from your Sprint Plan)

## Features Implemented:
[Create Bug] -- COMPLETE
User Story
As a User, I want to be able to create a bug so that the developer knows it needs to be fixed and what status, priority, and severity it is.

Acceptance Criteria
1 Have an input initialize a bug object. <-- User presses (C) to open the initialization.
2 Ensure the bug has a title, description, severity, assignto, and status field. <-- The initialize process is a queue of questions asking for all required data.
3 Check that status is only resulting enum/list statuses. [Open, Pending, InProgress, Closed] <-- Checks and fills the Status field with Open, the defaulting Status.
4 Ensure that once the data has been added, it builds and displays correctly. <-- Once the Questions are answered, it uses said data and passes it into the create bug method to build the bug and pass it into the list. After that, a printout displays all the correct data.

(5) Points – The feature requires input to instantiate the creation of a bug, opens fields to input valid data for each property, and an enum/list to store all the necessary status types, ensuring that status is tied to the enum/list. 

I managed to build a process that initializes the creation of a bug by  requiring the user to press the letter c while interacting with the project's main menu. Once pressed, I run a method that asks a queue of questions to fulfill all the required data {Title, Description, Priority, Severity}. Once the questions are answered, it takes the data, passes into creating the bug with said data, and adds it to the saved list. To verify the bug was created, a ticket will be displayed that shows what the bug model looks like. This way, A User will be able to create a bug that holds all the information a developer will need to know it needs to be addressed, how severe, and what its priority will be.

### ✅ B2: View Bug List
- Explain your solution and testing approach

### ✅ B4: Priority Tagging
- Describe how priority was stored and used

## Tests Completed:
- List which tests exist and what they validate

## Known Issues:
- Note any bugs, confusion, or to-dos

## What We Learned:
- Share insights from building and testing
- What is one thing your team would do differently in the next sprint?

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


### B2: View Bug List
# User Story
As a User, I want to be able to view a list of all bugs so that I can easily track the status of all bugs from one place.

# Acceptance Criteria
All bugs are added to a list at the time of creation.
The list can be viewed by the user.
The list can be sorted by Title or by Status.

# Tests
1. ViewBugs_SortedByTitle_OrderShouldChange
This test validates that the order of the seed data bugs changes to be alphabetically sorted by title.

2. ViewBugs_SortedByStatus_OrderShouldChange
This test validates that the order of the seed data bugs changes to be in order of enum status value (0-3).
