using DevTeams_POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTeams_Repository
{
    public class DeveloperRepository
    {
        private readonly List<Developer> _developerContext = new List<Developer>();
        private int _count;
        //Create
        public bool AddDeveloperToDirectory(Developer newDev)
        {
            if (newDev==null)
            {
                return false;
            }
            newDev.ID=++_count;
            _developerContext.Add(newDev);
            return true;
        }
        //Read
        public List<Developer> GetAllDevelopers()
        {
            return _developerContext;
        }
        public Developer GetDeveloperByID(int devID)
        {
            foreach(Developer dev in _developerContext)
            {
                if (dev.ID == devID)
                {
                    return dev;
                }
            }
            return null;
        }
        public List<Developer> GetDevsWithoutPluralsight()
        {
            return _developerContext.Where(dev=>!dev.HasPluralSight).ToList();
        }
        //Update
        public bool UpdateDeveloperInList(int iD, Developer updatedDeveloper)
        {
            Developer outdatedDeveloper = GetDeveloperByID(iD);
            if (outdatedDeveloper != null)
            {
                outdatedDeveloper.FirstName = updatedDeveloper.FirstName;
                outdatedDeveloper.LastName = updatedDeveloper.LastName;
                outdatedDeveloper.HasPluralSight = updatedDeveloper.HasPluralSight;
                return true;
            }
            else
            {
                return false;
            }
        }
        //Delete
        public bool RemoveDeveloperFromDirectory(Developer oldDev)
        {
            return _developerContext.Remove(oldDev);
        }
    }
}
