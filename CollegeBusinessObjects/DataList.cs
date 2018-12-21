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


        public int TotalValue(string column)
        {
            // Init the command
            command.CommandText = $"SELECT sum({column}) FROM {table}";

            // Execute the command
            reader = command.ExecuteReader();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

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

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

        public int TotalValue(string sumColumn, string column, string value, string tableTwo, string key)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);
            //command.Parameters.AddWithValue("@key", key);

            // Init the command
            command.CommandText = $"SELECT sum({sumColumn}) FROM {table} t, {tableTwo} tt  WHERE se.{column} = @value = t.{key} = tt.{key}";
            // SELECT sum(duration) from Schedule sc, Section se WHERE sc.SectionID = se.SectionID AND se.InstructorID = VALUE

            // Execute the command
            reader = command.ExecuteReader();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }

        public int TotalValue(string sumColumn, string column, string value, string tableTwo, string key, string key2)
        {
            // Clear all the prevously set parameters
            command.Parameters.Clear();
            // Set the new parameters
            command.Parameters.AddWithValue("@value", value);
            //command.Parameters.AddWithValue("@key", key);

            // Init the command
            command.CommandText = $"SELECT sum({sumColumn}) FROM {table} t, {tableTwo} tt  WHERE t.{key} = tt.{key} AND {column} = @value";
            // SELECT sum(duration) from Schedule sc, Section se, SectionStudent ss WHERE ss.StudentID = VALUE AND ss.SectionID = se.SectionID AND ss.SectionID = s.SectionID
            // Execute the command
            reader = command.ExecuteReader();

            // Get the value from the reader and cast it to an int
            return (int)reader.GetValue(0);
        }
    }
}
