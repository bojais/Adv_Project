using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class InstructorList: DataList
    {
        public InstructorList(): base ("Instructor", "InstructorID")
        {

        }

        protected override void GenerateList()
        {
            Instructor instructor = new Instructor();

            SetDataTableColumns(instructor);

            List.Clear();

            while (Reader.Read())
            {
                instructor = new Instructor(Reader.GetValue(0).ToString());
                base.SetValues(instructor);
                List.Add(instructor);

                AddDataTableRow(instructor);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
