using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class SectionStudentList: DataListJoin
    {
        public SectionStudentList(): base("SectionStudent", "SectionID", "StudentID")
        {

        }

        protected override void GenerateList()
        {
            SectionStudent sectionStudent = new SectionStudent();

            SetDataTableColumns(sectionStudent);

            List.Clear();

            while (Reader.Read())
            {
                sectionStudent = new SectionStudent(Reader.GetValue(0).ToString(), Reader.GetValue(1).ToString());
                base.SetValues(sectionStudent);
                List.Add(sectionStudent);

                AddDataTableRow(sectionStudent);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
