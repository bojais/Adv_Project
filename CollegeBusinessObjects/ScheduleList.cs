using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class ScheduleList: DataList
    {
        public ScheduleList(): base("Schedule", "ScheduleID")
        {

        }

        protected override void GenerateList()
        {
            Schedule schedule = new Schedule();

            SetDataTableColumns(schedule);

            List.Clear();

            while (Reader.Read())
            {
                schedule = new Schedule(Reader.GetValue(0).ToString());
                base.SetValues(schedule);
                List.Add(schedule);

                AddDataTableRow(schedule);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
