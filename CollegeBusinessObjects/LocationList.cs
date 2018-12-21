using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class LocationList: DataList
    {
        public LocationList(): base("Location", "LocationID")
        {

        }

        protected override void GenerateList()
        {
            Location location = new Location();

            SetDataTableColumns(location);

            List.Clear();

            while (Reader.Read())
            {
                location = new Location(Reader.GetValue(0).ToString());
                base.SetValues(location);
                List.Add(location);

                AddDataTableRow(location);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
