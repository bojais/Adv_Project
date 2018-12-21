using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class StudentList: DataList
    {
        public StudentList(): base("Student", "StudentID")
        {

        }

        protected override void GenerateList()
        {
            Student student = new Student();

            SetDataTableColumns(student);

            List.Clear();

            while (Reader.Read())
            {
                student = new Student(Reader.GetValue(0).ToString());
                base.SetValues(student);
                List.Add(student);

                AddDataTableRow(student);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
