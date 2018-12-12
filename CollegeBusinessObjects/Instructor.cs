using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class Instructor: Item
    {
        private string lastName;
        private string firstName;
        private string hireDate;
        private string password;

        public Instructor()
        {

        }

        public Instructor(string id): base(id)
        {

        }

        public string InstructorID
        {
            get { return base.getID(); }
            set { base.setID(value); }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string HireDate
        {
            get { return hireDate; }
            set { hireDate = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
