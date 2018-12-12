using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class SectionStudent: ItemJoin
    {
        private string grade;

        public SectionStudent()
        {

        }

        public SectionStudent(string id, string idJoin): base (id, idJoin)
        {

        }

        public string SectionID
        {
            get { return base.getID(); }
            set { base.setID(value); }
        }

        public string StudentID
        {
            get { return base.getIdJoin(); }
            set { base.setIdJoin(value); }
        }

        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }
    }
}
