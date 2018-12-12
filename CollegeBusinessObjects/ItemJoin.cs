using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class ItemJoin: Item
    {
        private string idJoin;

        public ItemJoin()
        {

        }

        public ItemJoin(string id, string idJoin)
            :base(id)
        {
            this.idJoin = idJoin;
        }

        public string getIdJoin()
        {
            return idJoin;
        }

        public void setIdJoin(string idJoin)
        {

        }
    }
}
