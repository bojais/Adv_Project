using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class Item
    {
        private string id;
        private bool valid = true;
        private string errorMessage;

        public Item()
        {

        }

        public Item(string id)
        {
            this.id = id;
        }

        public string getID()
        {
            return id;
        }

        public void setID(string id)
        {
            this.id = id;
        }

        public void setValid(bool valid)
        {
            this.valid = valid;
        }

        public bool getValid()
        {
            return valid;
        }

        public void setErrorMessage(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public string getErrorMessage()
        {
            return errorMessage;
        }
    }
}
