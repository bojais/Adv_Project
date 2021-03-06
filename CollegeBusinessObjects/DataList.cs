﻿using System;
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

        public virtual void Populate()
        {
            Connection.Open();
            Command.CommandText = $"SELECT * FROM {this.Table}";
            Reader = Command.ExecuteReader();
            GenerateList();
        }

        public void Populate(Item item)
        {
            Connection.Open();
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", item.getID());
            Command.CommandText = $"SELECT * FROM {this.Table} WHERE {this.IdField} = @id";
            Reader = Command.ExecuteReader();
            Reader.Read();
            SetValues(item);
            Reader.Close();
            Connection.Close();
        }

        public virtual void Filter(string field, string value)
        {
            Connection.Open();
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@field", field);
            command.Parameters.AddWithValue("@value", value);
            Command.CommandText = $"SELECT * FROM {this.Table} WHERE {@field} = @value";
            Reader = Command.ExecuteReader();
            GenerateList();
        }

        public virtual void Filter(string table2, string field, string value, string key)
        {
            Connection.Open();
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@field", field);
            command.Parameters.AddWithValue("@value", value);
            command.Parameters.AddWithValue("@key", key);
            Command.CommandText = $"SELECT * FROM {this.Table} t, {table2} tt WHERE tt.{@field} = @value AND t.{@key} = tt.{@key}";
            Reader = Command.ExecuteReader();
            GenerateList();
        }

        public virtual void FilterJoin(string table2, string key)
        {
            Connection.Open();
            command.Parameters.Clear();
            Command.CommandText = $"SELECT DISTINCT t.* FROM {this.Table} t, {table2} tt  WHERE t.{key} = tt.{key}";
            Reader = Command.ExecuteReader();
            GenerateList();
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
            // Open the connection
            connection.Open();

            // Init the command
            command.CommandText = $"SELECT max({idField}) FROM {table}";

            // Execute the command
            reader = command.ExecuteReader();

            // Save the value before closing the connection
            reader.Read();
            int maxId = (int)reader.GetValue(0);

            // Close the connection
            reader.Close();
            connection.Close();

            // Get the value from the reader and cast it to an int
            return maxId;
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
            // Open the connection
            connection.Open();

            // Init the command
            command.CommandText = $"SELECT coalesce(sum({column}), 0) FROM {table}";

            // Execute the command
            reader = command.ExecuteReader();

            // Init the total value to 0
            int totalValue = 0;

            // Read the next row and check if if it has any values
            if (reader.Read())
            {
                // if it does, set it to the totalValue
                totalValue = reader.GetInt32(0);
            }

            // Close the connection
            reader.Close();
            connection.Close();

            return totalValue;
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
            // Open the connection
            connection.Open();

            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            // Note: We use coalesce() here to convert NULL results to 0
            command.CommandText = $"SELECT coalesce(sum({sumColumn}), 0) FROM {table} WHERE {column} = @value";

            // Execute the command
            reader = command.ExecuteReader();

            // Init the total value to 0
            int totalValue = 0;

            // Read the next row and check if if it has any values
            if (reader.Read())
            {
                // if it does, set it to the totalValue
                totalValue = reader.GetInt32(0);
            }

            // Close the connection
            reader.Close();
            connection.Close();

            return totalValue;
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
            // Open the connection
            connection.Open();

            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT coalesce(sum({sumColumn}), 0) FROM {table} t, {tableTwo} tt  WHERE tt.{column} = @value AND t.{key} = tt.{key}";

            // Execute the command
            reader = command.ExecuteReader();

            // Init the total value to 0
            int totalValue = 0;

            // Read the next row and check if if it has any values
            if (reader.Read())
            {
                // if it does, set it to the totalValue
                totalValue = reader.GetInt32(0);
            }

            // Close the connection
            reader.Close();
            connection.Close();
            
            return totalValue;
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
            // Open the connection
            connection.Open();

            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT coalesce(sum({sumColumn}), 0) FROM {table} t, {tableTwo} tt, {tableThree} ttt  WHERE ttt.{column} = @value AND t.{keyOne} = tt.{keyOne} AND tt.{keyTwo} = ttt.{keyTwo}";

            // Execute the command
            reader = command.ExecuteReader();

            // Init the total value to 0
            int totalValue = 0;

            // Read the next row and check if if it has any values
            if (reader.Read())
            {
                // if it does, set it to the totalValue
                totalValue = reader.GetInt32(0);
            }

            // Close the connection
            reader.Close();
            connection.Close();

            return totalValue;
        }

        /// <summary>
        /// Calculate the average of a cloumn
        /// </summary>
        /// <param name="column">The column to calculate</param>
        /// <returns>The average</returns>
        public double AverageValue(string column)
        {
            // Open the connection
            connection.Open();

            // Init the command
            command.CommandText = $"SELECT coalesce(avg(cast({column} as float)), 0) FROM {table}";

            // Execute the command
            reader = command.ExecuteReader();

            // Init the total value to 0
            double averageValue = 0;

            // Read the next row and check if if it has any values
            if (reader.Read())
            {
                // if it does, set it to the totalValue
                averageValue = reader.GetDouble(0);
            }

            // Close the connection
            reader.Close();
            connection.Close();

            return averageValue;
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
            // Open the connection
            connection.Open();

            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT coalesce(avg(cast({avgColumn} as float)), 0) FROM {table} WHERE {column} = @value";

            // Execute the command
            reader = command.ExecuteReader();

            // Init the total value to 0
            double averageValue = 0;

            // Read the next row and check if if it has any values
            if (reader.Read())
            {
                // if it does, set it to the totalValue
                averageValue = reader.GetDouble(0);
            }

            // Close the connection
            reader.Close();
            connection.Close();

            return averageValue;
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
            // Open the connection
            connection.Open();

            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"SELECT coalesce(avg(cast({avgColumn} as float)), 0) FROM {table} t, {tableTwo} tt, {tableThree} ttt  WHERE ttt.{column} = @value AND t.{keyOne} = tt.{keyOne} AND tt.{keyTwo} = ttt.{keyTwo}";

            // Execute the command
            reader = command.ExecuteReader();

            // Init the total value to 0
            double averageValue = 0;

            // Read the next row and check if if it has any values
            if (reader.Read())
            {
                // if it does, set it to the totalValue
                averageValue = reader.GetDouble(0);
            }

            // Close the connection
            reader.Close();
            connection.Close();

            return averageValue;
        }

        // TODO: Add comments
        public bool Exists(string columnOne, string valueOne, string columnTwo, string valueTwo, string columnThree, string valueThree)
        {
            // Open the connection
            connection.Open();

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

            // Save the value before closing the connection
            bool exist = reader.Read();

            // Close the connection
            reader.Close();
            connection.Close();

            return exist;
        }

        // TODO: Add comments
        public bool Exists(string tableTwo, string columnOne, string valueOne, string columnTwo, string valueTwo, string columnThree, string valueThree, string keyOne)
        {
            // Open the connection
            connection.Open();

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

            // Save the value before closing the connection
            bool exist = reader.Read();

            // Close the connection
            reader.Close();
            connection.Close();

            return exist;
        }

        public void Add(Item item)
        {
            // Open the connection
            connection.Open();

            /** 
             * this commandtext is just to store key information into the schematable
             * This code block is for getting the columns information such as IsAutoIncrement
             * and store those details into schemaTable 
             **/
            command.CommandText = $"SELECT * FROM {table}";
            reader = command.ExecuteReader(CommandBehavior.KeyInfo);
            DataTable schemaTable = reader.GetSchemaTable();
            reader.Close();

            // Get the type of the received item
            Type type = item.GetType();

            // Clear the command from any othe parameters that were used previously
            command.Parameters.Clear();

            // Get the properties of the item
            PropertyInfo[] properties = type.GetProperties();

            // A counter for the properties in each loop cycle
            int count = 0;

            // A string that contains the command text
            string addString = $"INSERT INTO {table} (";

            // Loop through the properties of each item
            foreach(PropertyInfo property in properties)
            {
                // If the column is not auto increment, add the column names in the command text
                if(!schemaTable.Rows[count]["IsAutoIncrement"].ToString().Equals("True"))
                {
                    // Add the column names to command text veriable
                    addString += property.Name;
                    
                    // Increment the counter to move to the next column name
                    count++;

                    // While the column name is not the last property in the table, add a comma in the command text variable
                    if(count < properties.Count())
                    {
                        addString += ", ";
                    }
                }
                // Otherwise ignore the property and move to the next one
                else
                {
                    count++;
                }
            }

            // More building up on the command text variable
            addString += ") VALUES (";

            // Reset the counter, to start adding the values to the command text variable
            count = 0;

            // A counter for replacing the the variable name with a number
            int paramCounter = 1;

            // Loop through the properties of each item
            foreach (PropertyInfo property in properties)
            {
                // If the column is not auto increment, add values in the command text
                if (!schemaTable.Rows[count]["IsAutoIncrement"].ToString().Equals("True"))
                {
                    // If the properties are not empty, add their values to the command text variable
                    if(property.GetValue(item) != null)
                    {
                        // Replace each value with a command parameter using the counter declared previuosly
                        command.Parameters.AddWithValue("@" + paramCounter, property.GetValue(item));
                        addString += "@" + paramCounter;
                        paramCounter++;
                    }
                    // If no values exist, add 'NULL' to the command text variable
                    else
                    {
                        addString += "NULL";
                    }
                    // Increment and move to the next property
                    count++;

                    // While the value is not the last property in the table, add a comma in the command text variable
                    if (count < properties.Count())
                    {
                        addString += ", ";
                    }
                }
                // Otherwise ignore the property and move to the next one
                else
                {
                    count++;
                }
            }
            
            // Closing for the command text
            addString += ")";

            // Add the string veriable that was builded during the method to the command.CommandText
            command.CommandText = addString;

            try
            {
                command.ExecuteNonQuery();
            }
            // Exception Handling for executing the command
            catch(SqlException ex)
            {
                item.setValid(false);
                item.setErrorMessage(ex.Message);
            }

            // Close the connection
            connection.Close();
        }

        public void Update(Item item)
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
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@value", prop.GetValue(item));
                    command.Parameters.AddWithValue("@id", item.getID());
                    command.CommandText = $"UPDATE {table} SET {prop.Name} = @value WHERE {IdField} = @id";

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    // Exception Handling for executing the command
                    catch (SqlException ex)
                    {
                        item.setValid(false);
                        item.setErrorMessage(ex.Message);
                    }
                }
            }
            connection.Close();
        }

        // A simple delete method that deletes a record in a table using the ID
        // $"DELETE FROM {Student} WHERE {StudentID} = 1"
        // $"DELETE FROM {Instructor} WHERE {InstructorID} = 1"
        // $"DELETE FROM {TaughtCourse} WHERE {TaughtCourseID} = 1"
        // $"DELETE FROM {Schedule} WHERE {ScheduleID} = 1"
        // $"DELETE FROM {SectionStudent} WHERE {SectionStudentID} = 1"
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

            try
            {
                // Execute the command
                command.ExecuteNonQuery();

            }
            // Exception Handling for executing the command
            catch (SqlException ex)
            {
                item.setValid(false);
                item.setErrorMessage(ex.Message.ToString());
            }

            // Close the connection
            connection.Close();
        }

        // To delete Section after deleting SectionStudent and Schedule
        // To delete SectionStudent before deleting student
        public void Delete(string column, string value)
        {
            // Opening the connection
            connection.Open();

            // Clear all the previously set parameters
            command.Parameters.Clear();

            // Set the new Parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"DELETE FROM {table} WHERE {column} = @value";

            // Execute the command
            command.ExecuteNonQuery();

            // Close the connection
            connection.Close();
        }

        // The correct Delete method to delete from Schedule and SectionStudent
        // Then will be called twice: for Schedule, and for SectionStudent
        public void Delete(string tableTwo, string column, string key, string value)
        {
            // Open the connection
            connection.Open();

            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);

            // Init the command
            command.CommandText = $"Delete {table} FROM {table} INNER JOIN {tableTwo} ON {table}.{key} = {tableTwo}.{key} AND {tableTwo}.{column} = @value";

            // Execute the command
            command.ExecuteNonQuery();

            // Close the connection
            connection.Close();
        }
        
        public bool Login(string idColumn, string passwordColumn,string id, string password)
        {
            // Open the connection
            connection.Open();

            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@password", password);

            // Init the command
            command.CommandText = $"SELECT * FROM {table} WHERE {idColumn} = @id AND {passwordColumn} = @password";

            // Execute the command
            reader = command.ExecuteReader();

            // Set the return value of reader to the found var
            // to assert wheather the user has been found or not
            bool found = reader.Read();

            // Close the connection
            reader.Close();
            connection.Close();

            return found;
        }
    }
}
