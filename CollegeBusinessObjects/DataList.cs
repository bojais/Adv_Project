using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;

namespace CollegeBusinessObjects
{
    public class DataList
    {
        private string table;
        private string idField;
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        public List<Item> list;
        private DataTable dataTable;

        public DataList(string table, string idField)
        {
            this.table = table;
            this.idField = idField;
            connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=College;Integrated Security=True");
            command = connection.CreateCommand();
            list = new List<Item>();
            dataTable = new DataTable();
        }

        protected string Table
        {
            get { return table; }
            set { table = value; }
        }

        protected string IdField
        {
            get { return idField; }
            set { idField = value; }
        }

        protected SqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        protected SqlCommand Command
        {
            get { return command; }
            set { command = value; }
        }

        protected SqlDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        public List<Item> List
        {
            get { return list; }
            set { list = value; }
        }

        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        protected virtual void GenerateList()
        {

        }

        /// <summary>
        /// Get the max id in the database
        /// </summary>
        /// <returns>The max id</returns>
        public int GetMaxID()
        {
            // Init the command
            command.CommandText = $"SELECT max({idField}) FROM {table}";

            // Execute the command
            reader = command.ExecuteReader();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

        public void SetDataTableColumns(Item item)
        {
            DataTable.Clear();
            DataTable.Columns.Clear();

            Type type = item.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach(PropertyInfo property in properties)
            {
                DataTable.Columns.Add(property.Name);
            }
        }

        public void AddDataTableRow(Item item)
        {
            Type type = item.GetType();
            PropertyInfo[] properties = type.GetProperties();

            string[] values = new string[properties.Length];
            int fieldCount = 0;

            foreach(PropertyInfo property in properties)
            {
                values[fieldCount] = property.GetValue(item).ToString();
                fieldCount++;
            }

            DataTable.Rows.Add(values);
        }

        public void SetValues(Item item)
        {
            Type type = item.GetType();
            PropertyInfo[] properties = type.GetProperties();

            int fieldCount = 0;

            foreach(PropertyInfo property in properties)
            {
                property.SetValue(item, reader.GetValue(fieldCount).ToString());
                fieldCount++;
            }
        }

        /// <summary>
        /// Calculate the total of a cloumn
        /// </summary>
        /// <param name="column">The column to sum</param>
        /// <returns>the sum</returns>
        public int TotalValue(string column)
        {
            // Init the command
            command.CommandText = $"SELECT sum({column}) FROM {table}";

            // Execute the command
            reader = command.ExecuteReader();

            // Read the next row
            reader.Read();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

        /// <summary>
        /// Calculate the total of a cloumn where another column is equal to a value
        /// </summary>
        /// <param name="sumColumn">Column to sum</param>
        /// <param name="column">Column to compare</param>
        /// <param name="value">The value to compare with</param>
        /// <returns>the sum</returns>
        public int TotalValue(string sumColumn, string column, string value)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT sum({sumColumn}) FROM {table} WHERE {column} = @value";

            // Execute the command
            reader = command.ExecuteReader();

            // Read the next row
            reader.Read();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

        /// <summary>
        /// Calculate the total of a cloumn where another column is equal to a value from another table
        /// </summary>
        /// <param name="sumColumn">Column to sum</param>
        /// <param name="column">Column to compare</param>
        /// <param name="value">The value to compare with</param>
        /// <param name="tableTwo">The second table to compare column with</param>
        /// <param name="key">The common key between the two tables</param>
        /// <returns>the sum</returns>
        public int TotalValue(string sumColumn, string column, string value, string tableTwo, string key)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT sum({sumColumn}) FROM {table} t, {tableTwo} tt  WHERE se.{column} = @value = t.{key} = tt.{key}";

            // Execute the command
            reader = command.ExecuteReader();

            // Read the next row
            reader.Read();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

        /// <summary>
        /// Calculate the total of a cloumn where another column is equal to a value
        /// </summary>
        /// <param name="sumColumn">Column to sum</param>
        /// <param name="column">Column to compare</param>
        /// <param name="value">The value to compare with</param>
        /// <param name="tableTwo">The second table to compare column with</param>
        /// <param name="tableThree">The third table to compare column with</param>
        /// <param name="key">The common key between the three tables</param>
        /// <returns>the sum</returns>
        public int TotalValue(string sumColumn, string column, string value, string tableTwo, string tableThree, string keyOne, string keyTwo)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT sum({sumColumn}) FROM {table} t, {tableTwo} tt, {tableThree} ttt  WHERE t.{column} = @value AND t.{keyOne} = tt.{keyOne} AND tt.{keyTwo} = ttt.{keyTwo}";

            // Execute the command
            reader = command.ExecuteReader();

            // Read the next row
            reader.Read();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

        /// <summary>
        /// Calculate the average of a cloumn
        /// </summary>
        /// <param name="column">The column to calculate</param>
        /// <returns>The average</returns>
        public double AverageValue(string column)
        {
            // Init the command
            command.CommandText = $"SELECT avg({column}) FROM {table}";

            // Execute the command
            reader = command.ExecuteReader();

            // Read the next row
            reader.Read();

            // Get the value from the reader and cast it to an int
            return (double)reader.GetValue(0);
        }

        /// <summary>
        /// Calculate the average of a cloumn where another column is equal to a value
        /// </summary>
        /// <param name="avgColumn">Column to calculate</param>
        /// <param name="column">Column to compare</param>
        /// <param name="value">The value to compare with</param>
        /// <returns>the sum</returns>
        public double AverageValue(string avgColumn, string column, string value)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT avg({avgColumn}) FROM {table} WHERE {column} = @value";

            // Execute the command
            reader = command.ExecuteReader();

            // Read the next row
            reader.Read();

            // Get the value from the reader and cast it to an int
            return (double)reader.GetValue(0);
        }

        /// <summary>
        /// Calculate the average of a cloumn where another column is equal to a value
        /// </summary>
        /// <param name="avgColumn">Column to calculate</param>
        /// <param name="column">Column to compare</param>
        /// <param name="value">The value to compare with</param>
        /// <param name="tableTwo">The second table to compare column with</param>
        /// <param name="key">The common key between the two tables</param>
        /// <returns>the sum</returns>
        public double AverageValue(string avgColumn, string column, string value, string tableTwo, string tableThree, string keyOne, string keyTwo)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT avg({avgColumn}) FROM {table} t, {tableTwo} tt, {tableThree} ttt  WHERE t.{column} = @value AND t.{keyOne} = tt.{keyOne} AND tt.{keyTwo} = ttt.{keyTwo}";

            // Execute the command
            reader = command.ExecuteReader();

            // Read the next row
            reader.Read();

            // Get the value from the reader and cast it to an int
            return (double)reader.GetValue(0);
        }

        // TODO: Add comments
        public bool Exists(string columnOne, string valueOne, string columnTwo, string valueTwo, string columnThree, string valueThree)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@valueOne", valueOne);
            command.Parameters.AddWithValue("@valueTwo", valueTwo);
            command.Parameters.AddWithValue("@valueThree", valueThree);

            // Init the command
            command.CommandText = $"SELECT {columnOne}, {columnTwo} FROM {table} WHERE {columnOne} = @valueOne AND {columnTwo} = @valueTwo AND  {columnThree} = @valueThree";

            // Execute the command
            reader = command.ExecuteReader();

            // Get the value from the reader and cast it to an int
            return reader.Read();
        }

        // TODO: Add comments
        public bool Exists(string tableTwo, string columnOne, string valueOne, string columnTwo, string valueTwo, string columnThree, string valueThree, string keyOne)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@valueOne", valueOne);
            command.Parameters.AddWithValue("@valueTwo", valueTwo);
            command.Parameters.AddWithValue("@valueThree", valueThree);

            // Init the command
            command.CommandText = $"SELECT t.{columnOne}, t.{columnTwo} FROM {table} t, {tableTwo} tt WHERE t.{columnOne} = @valueOne AND t.{columnTwo} = @valueTwo AND tt.{columnThree} = @valueThree AND t.{keyOne} = tt.{keyOne}";

            // Execute the command
            reader = command.ExecuteReader();

            // Get the value from the reader and cast it to an int
            return reader.Read();
        }





        // A simple delete method that deletes a record in a table using the ID
        public void Delete(Item item)
        {
            // Opening the connection
            connection.Open();

            // Clear all the previously set parameters
            command.Parameters.Clear();

            // Set the new Parameters
            command.Parameters.AddWithValue("@id", item.getID());

            // Init the command
            command.CommandText = $"DELETE FROM {table} WHERE {idField} = @id";

            // Execute the command
            command.ExecuteNonQuery();
        }






        public void Delete(string column, string value)
        {
            // Opening the connection`
            connection.Open();

            // Clear all the previously set parameters
            command.Parameters.Clear();

            // Set the new Parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"DELETE FROM {table} WHERE {column} = @value";

            // Execute the command
            command.ExecuteNonQuery();
        }






        // With extra (table3 and table4) parameters and no keys parameter
        // This can delete a record in Section table with all it's related records in the tables Schedule and SectionStudent
        //public void Delete(string table2, string table3, string table4, string column, string value)
        //{
        //    // Opening the connection
        //    connection.Open();

        //    // Clear all the previously set parameters
        //    command.Parameters.Clear();

        //    // Set the new Parameters
        //    command.Parameters.AddWithValue("@value", value);

        //    // Delete related records from the first grand child table
        //    command.CommandText = $"DELETE t4 FROM {table4} AS t4 INNER JOIN {table2} AS t2 on t4.{column} = t2.{column} AND t2.{column} = @value";

        //    // Execute the command
        //    command.ExecuteNonQuery();

        //    // Delete related records from the second grand child table
        //    command.CommandText = $"DELETE t3 FROM {table3} AS t3 INNER JOIN {table2} AS t2 on t3.{column} = t2.{column} AND t2.{column} = @value";

        //    // Execute the command
        //    command.ExecuteNonQuery();

        //    // Delete related records from the child table
        //    command.CommandText = $"DELETE t2 FROM {table2} AS t2 INNER JOIN {table} AS t1 on t2.{column} = t1.{column} AND t1.{column} = @value";

        //    // Delete some record from the parent table
        //    command.CommandText = $"DELETE FROM {table} WHERE {column} = @value";

        //    // Execute the command
        //    command.ExecuteNonQuery();
        //}


        
        //
        public void Delete(string table2, string table3, string table4, string key1, string key2, string value, string value2)
        {
            // Opening the connection
            connection.Open();

            // Clear all the previously set parameters
            command.Parameters.Clear();

            // Set the new Parameters
            command.Parameters.AddWithValue("@value", value);
            command.Parameters.AddWithValue("@value2", value2);


            // Delete related records from the first grand child table
            command.CommandText = $"DELETE t4 FROM {table4} AS t4 INNER JOIN {table2} AS t2 on t4.{key2} = t2.{key2} AND t2.{key2} = @value2";
            // Execute the command
            command.ExecuteNonQuery();


            // Delete related records from the second grand child table
            command.CommandText = $"DELETE t3 FROM {table3} AS t3 INNER JOIN {table2} AS t2 on t3.{key2} = t2.{key2} AND t2.{key2} = @value2";
            // Execute the command
            command.ExecuteNonQuery();


            // Delete related records from the child table
            command.CommandText = $"DELETE t2 FROM {table2} AS t2 INNER JOIN {table} AS t1 on t2.{key1} = t1.{key1} AND t1.{key1} = @value";
            // Execute the command
            command.ExecuteNonQuery();


            // Delete some record from the parent table
            command.CommandText = $"DELETE FROM {table} WHERE {key1} = @value";
            // Execute the command
            command.ExecuteNonQuery();
        }
    }
}
