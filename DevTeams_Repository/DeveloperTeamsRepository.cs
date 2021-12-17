using DevTeams_POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTeams_Repository
{
    /*
     * Our managers need to be able to add and remove members to/from a team by their unique identifier.
     * They should be able to see a list of existing developers to choose from and add to existing teams.
     * Odds are, the manager will create a team, and then add Developers individually from the Developer
     * Directory to that team. 
     */
    public class DeveloperTeamsRepository
    {
        private readonly List<DevTeam> _devTeamList = new List<DevTeam>();
        private DeveloperRepository _devRepo;
        private int _count;


        public DeveloperTeamsRepository(DeveloperRepository devRepo)
        {
            _devRepo = devRepo;
        }

        // Create
        public bool AddDevTeamToDirectory(DevTeam newDevTeam)
        {
            if(newDevTeam == null)
            {
                return false;
            }
            newDevTeam.TeamID = ++_count;
            _devTeamList.Add(newDevTeam);

            return true;
        }
        // Read
        public List<DevTeam> GetAllDevTeams()
        {
            return _devTeamList;
        }
        public DevTeam GetDevTeamByID(int searchID)
        {
            foreach (DevTeam devTeam in _devTeamList)
            {
                if (devTeam.TeamID == searchID)
                {
                    return devTeam;
                }
            }
            return null;
            //return (DevTeam)_devTeamList.Where(team => team.TeamID == searchID);
        }
        public List<Developer> GetTeamMembersByTeamID(int searchID)
        {
            return GetDevTeamByID(searchID).DeveloperList;
        }
        // Update
        public bool UpdateTeamInList(int iD, DevTeam updatedTeam)
        {
            DevTeam outdatedTeam = GetDevTeamByID(iD);
            
            if(outdatedTeam != null)
            {
                outdatedTeam.TeamID = updatedTeam.TeamID;
                outdatedTeam.TeamName = updatedTeam.TeamName;
                outdatedTeam.DeveloperList = updatedTeam.DeveloperList;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddDevToTeam(int teamID, int devID)
        {
            DevTeam thisTeam = GetDevTeamByID(teamID);
            Developer dev = _devRepo.GetDeveloperByID(devID);
            int startingCount = thisTeam.DeveloperList.Count;
            if (dev == null)
            {
                return false;
            }
            foreach (Developer thisDev in thisTeam.DeveloperList)
            {
                if (thisDev.ID == devID)
                {
                    Console.WriteLine($"Developer {devID} is already on this team.");
                    return false;
                }
                    
            }
            thisTeam.DeveloperList.Add(dev);
            return startingCount < thisTeam.DeveloperList.Count;
        }
        public List<Developer> MakeListOfDevelopers(string devIDs)
        {
            List<int> devIDsList = new List<int>();
            List<Developer> devList = new List<Developer>();
            devIDsList = devIDs.Split(',').Select(s => { int i; return Int32.TryParse(s, out i) ? i : -1; }).ToList();
            foreach(int iDNumber in devIDsList)
            {
                devList.Add(_devRepo.GetDeveloperByID(iDNumber));
            }
            return devList;
        }
        public bool AddMultipleDevs(int teamID, List<Developer> devList)
        {
            DevTeam thisTeam = GetDevTeamByID(teamID);
            int startingCount = thisTeam.DeveloperList.Count;
            if(devList == null)
            {
                return false;
            }
            thisTeam.DeveloperList.AddRange(devList);
            return startingCount < thisTeam.DeveloperList.Count;
        }
        public bool RemoveDevFromTeam(int teamID, int devID)
        {
            return GetDevTeamByID(teamID).DeveloperList.Remove(_devRepo.GetDeveloperByID(devID));
        }
        // Delete
        public bool RemoveDevTeamFromDirectory(DevTeam oldDevTeam)
        {
            return _devTeamList.Remove(oldDevTeam);
        }
    }
}
