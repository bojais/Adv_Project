using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class Schedule: Item
    {
        private string sectionID;
        private string locationID;
        private string day;
        private string time;
        private string duration;

        public Schedule()
        {

        }

        public Schedule(string id): base (id)
        {

        }

        public string ScheduleID
        {
            get { return base.getID(); }
            set { base.setID(value); }
        }

        public string SectionID
        {
            get { return sectionID; }
            set { sectionID = value; }
        }

        public string LocationID
        {
            get { return locationID; }
            set { locationID = value; }
        }

        public string Day
        {
            get { return day; }
            set { day = value; }
        }

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }
    }
}
