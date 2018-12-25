using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class ToughtCourseList: DataList
    {
        public ToughtCourseList(): base("TaughtCourse", "TaughtCourseID")
        {

        }

        protected override void GenerateList()
        {
            ToughtCourse toughtCourse = new ToughtCourse();

            SetDataTableColumns(toughtCourse);

            List.Clear();

            while (Reader.Read())
            {
                toughtCourse = new ToughtCourse(Reader.GetValue(0).ToString());
                base.SetValues(toughtCourse);
                List.Add(toughtCourse);

                AddDataTableRow(toughtCourse);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
