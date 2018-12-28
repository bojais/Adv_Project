using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
            this.idFieldJoin = idFieldJoin;
        }

        public void Update(ItemJoin item)
        {
            // Opening the connection
            Connection.Open();

            // Get the type of the received item
            Type type = item.GetType();

            // Get the properties of the item
            PropertyInfo[] properties = type.GetProperties();

            // Loop through the properties of each item
            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetValue(item) != null && prop.Name != IdField)
                {
                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@value", prop.GetValue(item));
                    Command.Parameters.AddWithValue("@id", item.getID());
                    Command.Parameters.AddWithValue("@idJoin", item.getIdJoin());
                    Command.CommandText = $"UPDATE {this.Table} SET {prop.Name} = @value WHERE {IdField} = @id AND {idFieldJoin} = @idJoin";

                    // Exception Handling for executing the command
                    try
                    {
                        Command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        item.setValid(false);
                        item.setErrorMessage(ex.Message);
                    }
                }
            }
            Connection.Close();
        }
    }
}
