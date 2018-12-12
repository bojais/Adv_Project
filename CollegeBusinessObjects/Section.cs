using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class Section: Item
    {
        private string toughtCourseID;
        private string instructorID;
        private string capacity;

        public Section()
        {

        }

        public Section(string id): base(id)
        {

        }

        public string SectionID
        {
            get { return base.getID(); }
            set { base.setID(value); }
        }

        public string ToughtCourseID
        {
            get { return toughtCourseID; }
            set { toughtCourseID = value; }
        }

        public string InstructorID
        {
            get { return instructorID; }
            set { instructorID = value; }
        }

        public string Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
    }
}
