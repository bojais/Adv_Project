using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class Course: Item
    {
        private string title;
        private string credits;

        public Course()
        {

        }

        public Course(string id): base(id)
        {

        }

        public string CourseID
        {
            get { return base.getID(); }
            set { base.setID(value); }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Credits
        {
            get { return credits; }
            set { credits = value; }
        }
    }
}
