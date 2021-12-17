using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTeams_POCOs
{
    //Teams need to contain their Team members (Developers) and their Team Name, and Team ID.

    public class DevTeam
    {
        public DevTeam()
        {

        }
        public DevTeam(string teamName)
        {
            TeamName = teamName;
            DeveloperList = new List<Developer>();
        }
        public DevTeam(string teamName, List<Developer> devList)
        {
            TeamName = teamName;
            DeveloperList = devList;
        }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public List<Developer> DeveloperList{ get; set; }


    }
}
