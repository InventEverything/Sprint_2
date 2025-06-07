
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Core
{
    public class BugService
    {
        private readonly List<Bug> _bugs = new List<Bug>(); //Saved bugs in a list
        private readonly string TZ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Get the current timezone for display purposes;

        public List<Bug> getBugs
        {
            get { return _bugs; }
        }
      
        public BugService() // Constructor that initializes the bug list with some sample bugs
        {
            _bugs.Add(new Bug(1, "Zebra", "Test bug 1", 1, 1)); // Status is Open
            _bugs.Add(new Bug(2, "Apple", "Test bug 2", 2, 2)); // Status is Pending
            _bugs.Add(new Bug(3, "Monkey", "Test bug 3", 2, 2)); // Status is InProgress
            _bugs[2].UpdateStatus(BugStatus.InProgress);
            _bugs[1].UpdateStatus(BugStatus.InProgress);
            _bugs[1].UpdateStatus(BugStatus.Pending);
        }


        // Method to add a bug to the list of bugs *
        public Bug CreateBug(string title, string description, int priority, int severity)
        {
            var bug = new Bug(_bugs.Count + 1, title, description, priority, severity);
            _bugs.Add(bug);
            return bug;
        }
        
        public bool DeleteBug(int bugId)
        {
            var bug = _bugs.FirstOrDefault(b => b.BugId == bugId);
            if (bug != null)
            {
                _bugs.Remove(bug);
                return true;
            }
            return false;
        }
      
        public List<Bug> SortBugsByTitle()
        {
            List<Bug> UnsortedBugs = _bugs;
            UnsortedBugs.Sort((x, y) => string.Compare(x.Title.ToLower(), y.Title.ToLower(), StringComparison.Ordinal));
            List<Bug> SortedByTitle = UnsortedBugs;
            return SortedByTitle;
        }

        public List<Bug> SortBugsByStatus()
        {
            List<Bug> UnsortedBugs = _bugs;
            UnsortedBugs.Sort((x, y) => x.Status.CompareTo(y.Status));
            List<Bug> SortedById = UnsortedBugs;
            return SortedById;
        }

        // ToDo: Add to developer service should assign to developer and then check if status is open or not, if open set to in progress

    }
}
