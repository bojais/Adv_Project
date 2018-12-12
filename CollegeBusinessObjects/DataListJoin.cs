using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class DataListJoin: DataList
    {
        string idFieldJoin;

        public DataListJoin(string table, string idField, string idFieldJoin)
            :base(table, idField)
        {

        }
    }
}
