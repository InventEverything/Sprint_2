using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BugTracker.Core
{
    public class BugMenuUI
    {
        #region ** UI Display **
        private readonly TextWriter _output; // <-- This is a private field that is used to write to the console
        private BugService _bugService = new BugService(); // Create an instance of BugService

        public BugMenuUI(TextWriter output = null) // <-- This is a constructor that takes a TextWriter as an argument
        {                                           // if no TextWriter is provided, it will use the default console output
            _output = output ?? Console.Out;
        }
        public BugMenuUI(BugService bugService)
        {
            _bugService = bugService;
        }


        // These are injectable callbacks, so we can mock them in tests
        public Action OnCreateBug = () => { /*Console.WriteLine("You chose to Create a Bug.");*/ };
        public Action OnViewBugList = () => { };
        public Action OnAssignBug = () => { };
        public Action OnDeleteBug = () => { };


        // This method is used to clear the console screen safely for testing purposes



        #region ** Menu **
        public void DisplayMenu(Func<char> inputProvider, bool skipClear = false) // <-- Func<char> is a delegate type that
        {                                                                             // takes a function that returns a char
            do
            {
                SafeClear(skipClear); 
                _output.WriteLine("==========================================");
                _output.WriteLine("|             BUG TRACKER MENU           |");
                _output.WriteLine("==========================================");
                _output.WriteLine("| (C) Create Bug                         |");
                _output.WriteLine("| (V) View Bug List                      |");
                _output.WriteLine("| (A) Assign to Developer                |");
                _output.WriteLine("| (D) Delete Bug                         |");
                _output.WriteLine("|                                        |");
                _output.WriteLine("| (E) Exit                               |");
                _output.WriteLine("==========================================");

                _output.WriteLine("Please select an option: ");
                char userChoice = char.ToUpper(inputProvider());
                _output.WriteLine("\n");
                switch (userChoice)
                {
                    case 'C':
                        if (!skipClear) // <-- This is used to skip the clear screen for testing purposes
                        { Func<string> stringProvider = () => Console.ReadLine(); CreateBug(stringProvider); }
                        else { OnCreateBug(); }
                        break;
                    case 'V':
                        if (!skipClear)
                        { ViewBugList(); }
                        else { OnViewBugList(); }
                        break;
                    case 'A':
                        if (!skipClear)
                        { /*Assign Developer method goes here*/ }
                        else { OnAssignBug(); }
                        break;
                    case 'D':
                        if (!skipClear)
                        { /*Delete method goes here*/ }
                        else { OnDeleteBug(); }
                        break;
                    case 'E':
                        if (!skipClear)
                        { Environment.Exit(0); }
                        break;
                    default:
                        _output.WriteLine("Invalid option. Please try again.");
                        if (!skipClear)
                        { DisplayMenu(() => Console.ReadKey().KeyChar); }
                        break;
                }
            } while (!skipClear);
        }
        #endregion

        #region ** Bug Display **
        public void DisplayBugDetails(Bug bug, bool skipClear = false)
        { // in "{bug.BugId,-18}|" the "18" means the padding(or character alignment from the center) on th which ever side
          // + numbers are right aligned, - numbers are left aligned
            SafeClear(skipClear); // Clear the console screen if skipClear is false
            _output.WriteLine("==========================================");
            _output.WriteLine("|               BUG  TICKET              |");
            _output.WriteLine("==========================================");
            _output.WriteLine($"| Bug ID:                {bug.BugId,-16}|");
            _output.WriteLine($"| Title:                 {bug.Title,-16}|");
            _output.WriteLine($"| Description:           {Truncate(bug.Description, 18),-16}|");
            _output.WriteLine($"| Priority:              {bug.Priority,-16}|");
            _output.WriteLine($"| Severity:              {bug.Severity,-16}|");
            _output.WriteLine($"| Status:                {bug.Status,-16}|");
            _output.WriteLine($"| Assigned To Developer: {bug.AssignedToDeveloper,-16}|");
            _output.WriteLine("==========================================");
            if (!skipClear)
            {
                _output.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
                DisplayMenu(() => Console.ReadKey().KeyChar); // < -- Is how to call the menu again with reference to the  
            }                                                      // input provider
        }

        #region ** Helper Methods **

        // This method truncates the text to a maximum length and adds "..." if it exceeds that length
        private string Truncate(string text, int maxLength)
        {
            if (text.Length > maxLength)
            {
                return text.Substring(0, maxLength - 3) + "...";
            }
            return text;
        }
        #endregion
        #endregion



        #endregion
        #region ** Create Bug **

        #region ** Original Code (Week 6 Project) **
        /*
        public void CreateBug()
        {
            Bug bug;
            bool skipClear = false; // This is used to skip the clear screen for testing purposes

            if (!skipClear)
            { Console.Clear(); }
            _output.WriteLine("What is the bug's title?");
            string title = Console.ReadLine();

            if (!skipClear)
            { Console.Clear(); }
            _output.WriteLine("What is the bugs description?");
            string description = Console.ReadLine();

            if (!skipClear)
            { Console.Clear(); }
            _output.WriteLine("What is the bugs priority? (0 = Low, 1 = Medium, 2 = High)");
            string priorityInput = Console.ReadLine();

            if (!int.TryParse(priorityInput, out int parsedPriority) || parsedPriority < 0 || parsedPriority > 2)
            {// checks if the input is a number and if it is between 0 and 2
                // if not, it will return an error message
                _output.WriteLine("Invalid input. Please enter a number between 0 and 2.");
                _output.WriteLine("Press any key to coninue.");
                Console.ReadKey();

                return;
            }
            int priority = parsedPriority;

            if (!skipClear)
            { Console.Clear(); }
            _output.WriteLine("What is the bugs severity? (0 = Trivial, 1 = Minor, 2 = Major, 3 = Critical)");
            string severityInput = Console.ReadLine();

            if (!int.TryParse(severityInput, out int parsedSeverity) || parsedSeverity < 0 || parsedSeverity > 3)
            {// checks if the input is a number and if it is between 0 and 3
                // if not, it will return an error message
                _output.WriteLine("Invalid input. Please enter a number between 0 and 3.");
                _output.WriteLine("Press any key to coninue.");
                Console.ReadKey();
                return;
            }
            int severity = parsedSeverity;

            bug = _bugService.CreateBug(title, description, priority, severity); // Call the CreateBug method from BugService
            DisplayBugDetails(bug); // Display the bug details
        } */
        #endregion

        #region **Refactored Code (Week 7 Project)**
        public Action OnDisplayMenu = () => { };
        public Action OnBugDetails = () => { };

        public BugMenuUI(TextWriter output = null, BugService bugService = null)
        {
            _output = output ?? Console.Out; // If no output is provided, use the default console output
            _bugService = bugService ?? new BugService(); // Set the BugService instance
        }

        // Create a bug using the BugService and display the details
        public void CreateBug(Func<string> inputProvider, bool skipClear = false)
        {
            string title = PromptInput("What is the bug's title?", inputProvider, skipClear);
            string description = PromptInput("What is the bug's description?", inputProvider, skipClear);
            int priority = PriorityCatch(inputProvider, skipClear); // Call the PriorityCatch method to get the priority
            int severity = SeverityCatch(inputProvider, skipClear); // Call the SeverityCatch method to get the severity

            Bug bug = _bugService.CreateBug(title, description, priority, severity); // Call the CreateBug method from BugService
            SafeDisplayBug(bug, skipClear); // Display the bug details
        }

        #region **Helper Methods**
        //A method to catch the and return the priority value from user input
        public int PriorityCatch(Func<string> inputProvider, bool skipClear = false)
        {
            string input;
            bool isValid = false;
            int parsedPriority = -1; // Initialize parsedPriority to an invalid value

            do 
            { 
                string priorityInput = 
                    PromptInput("What is the bug's priority? (0 = Low, 1 = Medium, 2 = High)", inputProvider, skipClear);
                    isValid = int.TryParse(priorityInput, out parsedPriority) && parsedPriority >= 0 && parsedPriority <= 2; 
            } while (!isValid) ; // Loop until a valid input is provided
            
            return parsedPriority; // Return the valid priority value
        }

        // A method to catch the and return the severity value from user input
        public int SeverityCatch(Func<string> inputProvider, bool skipClear = false)
        {
            string input;
            bool isValid = false;
            int parsedSeverity = -1; // Initialize parsedSeverity to an invalid value
            do
            {
                string severityInput = PromptInput(
                    "What is the bug's severity? (0 = Trivial, 1 = Minor, 2 = Major, 3 = Critical)", 
                    inputProvider, 
                    skipClear);
                isValid = int.TryParse(severityInput, out parsedSeverity) && parsedSeverity >= 0 && parsedSeverity <= 3;
            } while (!isValid); // Loop until a valid input is provided
            return parsedSeverity; // Return the valid severity value
        }

        #region **Private Methods**
        // This method is used to clear the console screen safely for testing purposes
        private void SafeClear(bool skipClear)
        {
            if (!skipClear)
            { Console.Clear(); }
        }

        // Display a prompt message and capture user input
        private string PromptInput(string message, Func<string> inputProvider, bool skipClear)
        {
            SafeClear(skipClear);
            _output.WriteLine(message);
            return inputProvider();
        }

        // Validates pathing of DisplayBugDetails and passes a Mockup for debugging purposes
        private void SafeDisplayBug(Bug bug, bool skipClear)
        {
            if (!skipClear)
                DisplayBugDetails(bug);
            else
                OnBugDetails(); // This is used to call the OnBugDetails action if skipClear is true
        }
        #endregion
        #endregion
        #endregion
        #endregion

        #region ** View Bug List **
        public void ViewBugList()
        {
            bool skipClear = false; // This is used to skip the clear screen for testing purposes
            List<Bug> bugList = _bugService.getBugs; // Get the list of bugs from the BugService
            do 
            {
                if (!skipClear)
                { Console.Clear(); }
                _output.WriteLine("Bug Id\tBug Title\tBug Status\tDescription\t\t\tAssigned To\t\tSeverity\tPriority"); // Headers
                Separator();
                Content(bugList);
                Separator();
                _output.WriteLine("Press (T) to sort by Title, (S) to sort by Status, or (M) to return to Menu"); // Footer
                ConsoleKey input = Console.ReadKey().Key;
                bugList = SortedContent(input, bugList);
            } while (true); // This is used to keep the list open until the user chooses to exit
        }
        private void Separator()
        {
            int lengthOfSeparator = 120; // This is used to set the length of the separator
            for (int i = 0; i < lengthOfSeparator; i++)
            { _output.Write("="); }
            _output.WriteLine("\n");
        }
        private void Content(List<Bug> List)
        {
            foreach (var bug in List)
            {
                _output.WriteLine($"{bug.BugId}\t" +
                    $"{Truncate(bug.Title + "            ", 14)}\t" +
                    $"{Truncate(bug.Status + "        ", 14)}\t" +
                    $"{Truncate(bug.Description + "                             ", 30)}\t" +
                    $"{Truncate(bug.AssignedToDeveloper + "              ", 14)}\t\t" +
                    $"{Truncate(bug.Severity + "      ", 14)}\t" +
                    $"{bug.Priority}\n");
            }
        }
        public List<Bug> SortedContent(ConsoleKey input, List<Bug> list = null, bool skipClear = false)
        {
            switch (input)
            {
                case ConsoleKey.T:
                    list = _bugService.SortBugsByTitle();
                    break;
                case ConsoleKey.S:
                    list = _bugService.SortBugsByStatus();
                    break;
                case ConsoleKey.M:
                    DisplayMenu(() => Console.ReadKey().KeyChar);
                    break;
                default:
                    _output.WriteLine("Invalid option. Please try again.");
                    if (!skipClear)
                    { ViewBugList(); }
                    break;
            }
            return list;
        }
        #endregion
    }
}

