using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class Location: Item
    {
        private string name;
        private string capacity;

        public Location()
        {

        }

        public Location(string id): base(id)
        {

        }

        public string LocationID
        {
            get { return base.getID(); }
            set { base.setID(value); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
    }
}
