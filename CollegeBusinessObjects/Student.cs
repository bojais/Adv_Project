using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class Student: Item
    {
        private string lastName;
        private string firstName;
        private string enrollmentDate;
        private string password;

        public Student()
        {

        }

        public Student(string id): base(id)
        {

        }

        public string StudentID
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

        public string EnrollmentDate
        {
            get { return enrollmentDate; }
            set { enrollmentDate = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
