using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class CourseList: DataList
    {
        public CourseList(): base("Course", "CourseID")
        {

        }

        protected override void GenerateList()
        {
            Course course = new Course();

            SetDataTableColumns(course);

            List.Clear();

            while(Reader.Read())
            {
                course = new Course(Reader.GetValue(0).ToString());
                base.SetValues(course);
                List.Add(course);

                AddDataTableRow(course);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
