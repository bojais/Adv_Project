using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class ToughtCourse: Item
    {
        private string courseID;
        private string semester;
        private string year;

        public ToughtCourse()
        {

        }

        public ToughtCourse(string id): base(id)
        {

        }

        public string ToughtCourseID
        {
            get { return base.getID(); }
            set { base.setID(value); }
        }

        public string CourseID
        {
            get { return courseID; }
            set { courseID = value; }
        }

        public string Semester
        {
            get { return semester; }
            set { semester = value; }
        }

        public string Year
        {
            get { return year; }
            set { year = value; }
        }
    }
}
