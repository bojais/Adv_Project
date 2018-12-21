using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeBusinessObjects
{
    public class SectionList: DataList
    {
        public SectionList(): base("Section", "SectionID")
        {

        }

        protected override void GenerateList()
        {
            Section section = new Section();

            SetDataTableColumns(section);

            List.Clear();

            while (Reader.Read())
            {
                section = new Section(Reader.GetValue(0).ToString());
                base.SetValues(section);
                List.Add(section);

                AddDataTableRow(section);
            }
            Reader.Close();
            Connection.Close();
        }
    }
}
