using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class CourseList: DataList
    {
        public CourseList(): base("Courses", "CourseID")
        {

        }

        protected override void GenerateList()
        {
            Course course = new Course();


        }
    }
}
