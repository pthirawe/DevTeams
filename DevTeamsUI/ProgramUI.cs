using DevTeams_POCOs;
using DevTeams_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTeamsUI
{
    public class ProgramUI
    {
        private readonly DeveloperRepository _devRepo;
        private readonly DeveloperTeamsRepository _devTeamRepo;

        public ProgramUI()
        {
            _devRepo = new DeveloperRepository();
            _devTeamRepo = new DeveloperTeamsRepository(_devRepo);
        }

        public void Run()
        {
            Seed();
            RunApplication();
        }
        // Displays Main Menu
        private void RunApplication()
        {
            bool isRunning = true;
            do
            {
                Console.WriteLine("Welcome to DevTeams\n" +
                    "1. Add A New Developer\n" +
                    "2. View All Existing Developers\n" +
                    "3. View Developer By ID\n" +
                    "4. View Developers Without Pluralsite\n" +
                    "5. Update an Existing Developer\n" +
                    "6. Delete an Existing Developer\n" +
                    "=================================\n" +
                    "7. Create A New Team\n" +
                    "8. View All Teams\n" +
                    "9. View Team Details By ID\n" +
                    "10. Update an Existing Team\n" +
                    "11. Delete a Team\n" +
                    "0. Exit");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        return;
                    case "1":
                        AddDeveloper();
                        break;
                    case "2":
                        ViewAllDevelopers();
                        break;
                    case "3":
                        ViewDeveloperByID();
                        break;
                    case "4":
                        ViewDevelopersWithoutPluralsite();
                        break;
                    case "5":
                        UpdateExistingDeveloper();
                        break;
                    case "6":
                        DeleteExistingDeveloper();
                        break;
                    case "7":
                        AddNewDevTeam();
                        break;
                    case "8":
                        ListAllTeams();
                        break;
                    case "9":
                        ViewTeamByID();
                        break;
                    case "10":
                        UpdateTeamMenu();
                        break;
                    case "11":
                        DeleteTeam();
                        break;
                    default:
                        Console.WriteLine("Invalid Selection.");
                        WaitForKey();
                        break;
                }
                Console.Clear();
            }while (isRunning);
        }

        private void UpdateTeamMenu()
        {
            Console.Clear();
            Console.WriteLine("==Update Existing Team==\n" +
                "1. Update Team Details\n" +
                "2. Add Developer To Team\n" +
                "3. Remove Developer From Team"
                );

            string userInput = Console.ReadLine();
            
            switch(userInput)
            {
                case "1":
                    UpdateExistingTeam();
                    break;
                case "2":
                    AddDeveloperToTeam();
                    break;
                case "3":
                    RemoveDeveloperFromTeam();
                    break;
                default:
                    Console.WriteLine("Invalid Selection.  Returning To Main Menu.");
                    WaitForKey();
                    break;
            }
        }

        // Developer Data Methods
        private void AddDeveloper()
        {
            string devFirstName;
            string devLastName;
            bool hasPluralSite = false;

            Console.Clear();
            Console.WriteLine("Please Enter the Developer First Name.");
            devFirstName = Console.ReadLine();
            Console.WriteLine("Please Enter the Developer Last Name.");
            devLastName = Console.ReadLine();

            hasPluralSite = CheckPluralsite();

            _devRepo.AddDeveloperToDirectory(new DevTeams_POCOs.Developer(devFirstName, devLastName, hasPluralSite));
        }

        private void ViewAllDevelopers()
        {
            Console.Clear();
            Console.WriteLine("               ============Developers============");
            Console.WriteLine("==ID==|======First Name======|======Last Name======|=Pluralsite=");
            foreach(Developer dev in _devRepo.GetAllDevelopers())
            {
                DisplayDeveloperDataList(dev);
            }
            WaitForKey();
        }

        private void ViewDeveloperByID()
        {
            int devID;

            Console.Clear();
            Console.WriteLine("Please enter a developer ID: ");
            devID = Int32.Parse(Console.ReadLine());

            ViewDeveloperByID(devID);
        }

        private void ViewDeveloperByID(int devID)
        {
            Developer dev;
            Console.Clear();
            dev = _devRepo.GetDeveloperByID(devID);
            if (dev == null)
            {
                Console.WriteLine("Developer ID is not valid. Returning to main menu.");
                WaitForKey();
                return;
            }
            Console.WriteLine("=====Developer=====");
            DisplayDeveloperData(dev);
            WaitForKey();
        }

        private void ViewDevelopersWithoutPluralsite()
        {
            Console.Clear();
            Console.WriteLine("              ==Developers Without Pluralsite==");
            Console.WriteLine("==ID==|======First Name======|======Last Name======|=Pluralsite=");
            foreach (Developer dev in _devRepo.GetDevsWithoutPluralsight())
            {
                DisplayDeveloperDataList(dev);
            }
            WaitForKey();
        }

        private void UpdateExistingDeveloper()
        {
            int searchID;
            string firstName;
            string lastName;
            bool hasPluralsite;
            bool confirmation = false;
            Developer dev;

            Console.Clear();
            Console.WriteLine("Please enter the ID of the developer to be updated.");
            searchID = Int32.Parse(Console.ReadLine());
            dev = _devRepo.GetDeveloperByID(searchID);
            if (dev == null)
            {
                Console.WriteLine("Invalid developer ID.  Returning to main menu.");
                WaitForKey();
                return;
            }
            Console.WriteLine("----------------------------------");
            DisplayDeveloperData(dev);
            Console.WriteLine("Update this developer? (y/n)");
            confirmation = ConfirmSelection();

            if (!confirmation)
            {
                Console.WriteLine("Cancelled.  Returning to main menu.");
                WaitForKey();
                return;
            }

            Console.WriteLine("Please enter the new First Name of the Developer.");
            firstName = Console.ReadLine();
            Console.WriteLine("Please enter the new Last Name of the Developer.");
            lastName = Console.ReadLine();
            hasPluralsite = CheckPluralsite();

            Developer newDev = new Developer(firstName, lastName, hasPluralsite);

            if (!_devRepo.UpdateDeveloperInList(searchID, newDev))
            {
                Console.WriteLine("Problem updating developer.  Please check ID number.");
                return;
            }

            Console.WriteLine("Successfully updated Developer.");
            WaitForKey();
        }

        private void DeleteExistingDeveloper()
        {
            Developer dev;
            bool confirmation = false;

            Console.Clear();
            Console.WriteLine("Please enter the ID of the developer to be deleted.");
            dev = _devRepo.GetDeveloperByID(Int32.Parse(Console.ReadLine()));
            if(dev == null)
            {
                Console.WriteLine("Invalid developer ID.  Returning to main menu.");
                WaitForKey();
                return;
            }
            Console.WriteLine("----------------------------------");
            DisplayDeveloperData(dev);
            Console.WriteLine("Delete this developer? (y/n)");
            confirmation=ConfirmSelection();

            if (!confirmation)
            {
                Console.WriteLine("Cancelled.  Returning to main menu.");
                WaitForKey();
                return;
            }

            if(!_devRepo.RemoveDeveloperFromDirectory(dev))
            {
                Console.WriteLine("Problem deleting Developer.  Please check ID number.");
                WaitForKey();
                return;
            }
            Console.WriteLine("Successfully Deleted Developer.");
            WaitForKey();
        }

        // Dev Team Methods

        private void AddNewDevTeam()
        {
            string devTeamName;
            Console.Clear();
            Console.WriteLine("Please Enter the Dev Team Name.");
            devTeamName = Console.ReadLine();

            DevTeam newDevTeam = new DevTeam(devTeamName);

            if(!_devTeamRepo.AddDevTeamToDirectory(newDevTeam))
            {
                Console.WriteLine("Problem adding new Dev Team.  Returning to main menu.");
                WaitForKey();
                return;
            }
            Console.WriteLine("Successfully Added New Dev Team.");
            WaitForKey();
        }

        private void ListAllTeams()
        {
            Console.Clear();
            Console.WriteLine("       ============Dev Teams============");
            Console.WriteLine("==ID==|======Team Name======|==Number Of Members==");
            foreach (DevTeam devTeam in _devTeamRepo.GetAllDevTeams())
            {
                DisplayTeamDataList(devTeam);
            }
            WaitForKey();
        }

        private void ViewTeamByID()
        {
            DevTeam devTeam;

            Console.Clear();
            Console.WriteLine("Please enter a Dev Team ID: ");
            devTeam = FetchAndCheckTeam();
            if (devTeam == null)
            {
                return;
            }
            Console.WriteLine("=====Dev Team=====");
            DisplayTeamData(devTeam);

            WaitForKey();
        }

        private void UpdateExistingTeam()
        {
            int searchID;
            string teamName;
            bool confirmation = false;
            DevTeam toUpdate;

            Console.Clear();
            Console.WriteLine("Please enter the ID of the Team to be updated.");
            while(!Int32.TryParse(Console.ReadLine(), out searchID))
            {
                Console.WriteLine("Invalid input.  Please enter a number.");
            }
            //searchID = Int32.Parse(Console.ReadLine());
            if ((toUpdate = FetchAndCheckTeam(searchID)) == null)
            {
                return;
            }
            Console.WriteLine("".PadRight(64,'-'));
            DisplayTeamData(toUpdate);
            Console.WriteLine("Update this Team? (y/n)");
            confirmation = ConfirmSelection();

            if (!confirmation)
            {
                Console.WriteLine("Cancelled.  Returning to main menu.");
                WaitForKey();
                return;
            }

            Console.WriteLine("Please enter the new Name of the Team.");
            teamName = Console.ReadLine();

            toUpdate.TeamName = teamName;

            if (!_devTeamRepo.UpdateTeamInList(searchID, toUpdate))
            {
                Console.WriteLine("Problem updating Team.  Please check ID number.");
                WaitForKey();
                return;
            }

            Console.WriteLine("Successfully updated Team.");
            WaitForKey();
        }

        private void AddDeveloperToTeam()
        {
            int searchID;
            int devID;
            string idToUpdate;
            bool confirmation = false;
            bool isValid;
            DevTeam toUpdate;
            List<Developer> devsToAdd;

            Console.Clear();
            Console.WriteLine("Please enter the ID of the Team to be updated.");
            if(!Int32.TryParse(Console.ReadLine(), out searchID))
            {
                Console.WriteLine("Invalid Number.  Returning to main menu.");
                WaitForKey();
                return;
            }
            if ((toUpdate = FetchAndCheckTeam(searchID)) == null)
            {
                return;
            }
            Console.WriteLine("----------------------------------");
            DisplayTeamData(toUpdate);
            Console.WriteLine("Update this Team? (y/n)");
            confirmation = ConfirmSelection();

            if (!confirmation)
            {
                Console.WriteLine("Cancelled.  Returning to main menu.");
                WaitForKey();
                return;
            }

            Console.WriteLine("Please enter a Developer ID or list of ID's delinated by commas [,] to be added.");
            idToUpdate = Console.ReadLine();
            isValid = Int32.TryParse(idToUpdate, out devID);
            //Check if integer parsed correctly.
            if (!isValid)
            {
                //if not, Try to parse list
                devsToAdd = _devTeamRepo.MakeListOfDevelopers(idToUpdate);
                if(devsToAdd.Contains(null))
                {
                    Console.WriteLine("Invalid entry. Check Entered IDs.  Returning to main menu.");
                    WaitForKey();
                    return;
                }
                if(!_devTeamRepo.AddMultipleDevs(searchID, devsToAdd))
                {
                    Console.WriteLine("Problem adding developers to team.  Returning to main menu.");
                    WaitForKey();
                    return;
                }
            }
            // If initial parse to int worked, try to add
            else if (isValid)
            {
                if(!_devTeamRepo.AddDevToTeam(searchID, devID))
                {
                    Console.WriteLine("Problem adding developer to team.  Please check ID numbers.");
                    WaitForKey();
                    return;
                }
            }

            Console.WriteLine("Successfully updated Team.");
            WaitForKey();
        }

        private void RemoveDeveloperFromTeam()
        {
            int searchID;
            int idToRemove;
            DevTeam toUpdate;

            bool confirmation = false;

            Console.Clear();
            Console.WriteLine("Please enter the ID of the Team to be updated.");
            while (!Int32.TryParse(Console.ReadLine(), out searchID))
            {
                Console.WriteLine("Invalid input.  Please enter a number.");
            }
            if ((toUpdate = FetchAndCheckTeam(searchID)) == null)
            {
                return;
            }
            Console.WriteLine("----------------------------------");
            DisplayTeamData(toUpdate);
            Console.WriteLine("Update this Team? (y/n)");
            confirmation = ConfirmSelection();

            if (!confirmation)
            {
                Console.WriteLine("Cancelled.  Returning to main menu.");
                WaitForKey();
                return;
            }

            do
            {
                Console.WriteLine("Please enter a Developer ID to remove.");
                while(!Int32.TryParse(Console.ReadLine(), out idToRemove))
                {
                    Console.WriteLine("Invalid Entry.  Please enter a number.");
                }

                if (!_devTeamRepo.RemoveDevFromTeam(searchID, idToRemove))
                {
                    Console.WriteLine("Problem removing Developer.  Please check ID numbers.");
                    return;
                }
                Console.WriteLine("Successfully Removed Developer.");
                Console.WriteLine("Remove another developer from this team?");
                confirmation = ConfirmSelection();
            }while(confirmation);
        }

        public void DeleteTeam()
        {
            int searchID;
            DevTeam toRemove;

            bool confirmation = false;

            do
            {
                Console.Clear();
                Console.WriteLine("Please enter the ID of the Team to be deleted.");
                while (!Int32.TryParse(Console.ReadLine(), out searchID))
                {
                    Console.WriteLine("Invalid input.  Please enter a number.");
                }
                if ((toRemove = FetchAndCheckTeam(searchID)) == null)
                {
                    return;
                }
                Console.WriteLine("----------------------------------");
                DisplayTeamData(toRemove);
                Console.WriteLine("Delete this Team? (y/n)");
                confirmation = ConfirmSelection();

                if (!confirmation)
                {
                    Console.WriteLine("Cancelled.  Returning to main menu.");
                    WaitForKey();
                    return;
                }

                if (!_devTeamRepo.RemoveDevTeamFromDirectory(toRemove))
                {
                    Console.WriteLine("Problem removing Developer.  Please check ID numbers.");
                    return;
                }
                Console.WriteLine("Successfully Removed Team.");
                Console.WriteLine("Remove another team?");
                confirmation = ConfirmSelection();
            } while (confirmation);
        }


        // Helper Methods

        private void WaitForKey()
        {
            Console.WriteLine("Press Any Key to Continue.");
            Console.ReadKey();
        }

        private bool CheckPluralsite()
        {
            Console.WriteLine("Does the developer have PluralSite access? (y/n)");
            return ConfirmSelection();
        }

        private bool ConfirmSelection()
        {
            bool isValid;
            do
            {
                string confirm = Console.ReadLine();
                isValid = (confirm.ToLower() == "y") || (confirm.ToLower() == "n");

                switch (confirm.ToLower())
                {
                    case "y":
                        return true;
                    case "n":
                        return false;
                    default:
                        Console.WriteLine("Please use y or n to respond.");
                        break;
                }

            } while (!isValid);
            return false;
        }
        
        private void DisplayDeveloperData(Developer dev)
        {
            Console.WriteLine(
                $"ID: {string.Format("{0:00000}", dev.ID)}\n" +
                $"First Name: {dev.FirstName}\n" +
                $"Last Name: {dev.LastName}\n" +
                $"Plurasite License: " + (dev.HasPluralSight ? "Yes" : "No")
                );
            Console.WriteLine("----------------------------------");
        }
        
        private void DisplayDeveloperDataList(Developer dev)
        {
            if(dev != null)
            {
                Console.WriteLine(
                    $"{string.Format("{0:00000}", dev.ID)}".PadRight(6) + "|" +
                    $"{dev.FirstName}".PadRight(22) + "|" +
                    $"{dev.LastName}".PadRight(21) + "|" +
                    (dev.HasPluralSight ? "Yes".PadLeft(8).PadRight(12) : "No".PadLeft(7).PadRight(12))
                    );
                Console.WriteLine("".PadRight(64,'-'));
            }
            else
            {
                Console.WriteLine("Null Item Detected.");
            }
        }

        private void DisplayTeamData(DevTeam devTeam)
        {
            Console.WriteLine(
                $"Team ID: {string.Format("{0:00000}", devTeam.TeamID)}\n" +
                $"Team Name: {devTeam.TeamName}\n" +
                $"Number of Team Members: {devTeam.DeveloperList.Count}"
                );
            Console.WriteLine("              ============Team Members============");
            Console.WriteLine("==ID==|======First Name======|======Last Name======|=Pluralsite=");
            foreach (Developer dev in devTeam.DeveloperList)
            {
                DisplayDeveloperDataList(dev);
            }
            //Console.WriteLine("".PadRight(64,'-'));
        }
        private void DisplayTeamDataList(DevTeam devTeam)
        {
            Console.WriteLine(
                $"{string.Format("{0:00000}", devTeam.TeamID)}".PadRight(6) + "|" +
                $"{devTeam.TeamName}".PadRight(21) + "|" +
                $"{devTeam.DeveloperList.Count}".PadLeft(11).PadRight(21)
                );
            Console.WriteLine("".PadRight(50, '-'));
        }
        private DevTeam FetchAndCheckTeam()
        {
            bool isValid;
            int findThisTeam;
            //searchID = Int32.Parse(Console.ReadLine());
            do
            {
                isValid = Int32.TryParse(Console.ReadLine(), out findThisTeam);
                if (!isValid)
                {
                    Console.WriteLine("Invalid input.  Please enter a number.");
                    WaitForKey();
                }
            } while (!isValid);
            DevTeam fetched = _devTeamRepo.GetDevTeamByID(findThisTeam);
            if (fetched == null)
            {
                Console.WriteLine("Invalid Team ID.  Returning to main menu.");
                WaitForKey();
            }
            return fetched;
        }

        private DevTeam FetchAndCheckTeam(int findThisTeam)
        {
            DevTeam fetched = _devTeamRepo.GetDevTeamByID(findThisTeam);
            if (fetched == null)
            {
                Console.WriteLine("Invalid Team ID.  Returning to main menu.");
                WaitForKey();
            }
            return fetched;
        }

        // Debug Methods
        private void Seed()
        {
            Developer dev1 = new Developer("John", "Smith", false);
            Developer dev2 = new Developer("Jack", "Hall", true);
            Developer dev3 = new Developer("Robotnik", "Eggman", false);
            Developer dev4 = new Developer("Reno", "Jackson", true);
            Developer dev5 = new Developer("Jaina", "Proudmoore", true);
            Developer dev6 = new Developer("Sylvanas", "Windrunner", false);


            _devRepo.AddDeveloperToDirectory(dev1);
            _devRepo.AddDeveloperToDirectory(dev2);
            _devRepo.AddDeveloperToDirectory(dev3);
            _devRepo.AddDeveloperToDirectory(dev4);
            _devRepo.AddDeveloperToDirectory(dev5);
            _devRepo.AddDeveloperToDirectory(dev6);

            DevTeam team1 = new DevTeam("Bungling Boar", new List<Developer>());
            DevTeam team2 = new DevTeam("Mangy Meerkat", new List<Developer>());

            _devTeamRepo.AddDevTeamToDirectory(team1);
            _devTeamRepo.AddDevTeamToDirectory(team2);
        }
    }
}
