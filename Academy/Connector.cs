﻿/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Academy
{
    internal class Connector
    {
        readonly string CONNECTION_STRING;
        SqlConnection connection;
        public Connector(string connection_string)
        {
            CONNECTION_STRING = connection_string;
            connection = new SqlConnection(CONNECTION_STRING); 
            AllocConsole();
            Console.WriteLine(CONNECTION_STRING);
        }

        public DataTable Select(string colums, string tables, string condition = "", string group_by = "")
        {
            DataTable table = null;
            string cmd = $"SELECT {colums} FROM {tables}";
            if (condition != "") cmd += $" WHERE {condition}";
            if (group_by != "") cmd += $" GROUP BY {group_by}";

            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            { 
                // создаём таблицу
                table = new DataTable();

                // определяем набор палей таблицы (добавляем столбцы в таблицу)
                for (int i = 0; i < reader.FieldCount; i++)
                { 
                    table.Columns.Add(reader.GetName(i));
                }

                // добавляем строки таблицы
                while (reader.Read())
                { 
                    // создаём строку
                    DataRow row = table.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[i] = reader[i];
                    table.Rows.Add(row);
  
                }
            }
            reader.Close();
            connection.Close();
            return table;
        }

        public Dictionary<string, int> GetDictionary(string table)
        {
            Dictionary<string, int> dictionary = null;
            string id_column= table.ToLower().Remove(table.Length - 1, 1) + "_id";
            string name_column = table.ToLower().Remove(table.Length - 1, 1) + "_name";
            string cmd = $"SELECT {name_column}, {id_column} FROM {table}";
            SqlCommand command = new SqlCommand (cmd, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            { 
                dictionary = new Dictionary<string, int>();
                while(reader.Read())
                {
                    dictionary[reader[0].ToString()] = Convert.ToInt32(reader[1]);

                }
            }
            reader.Close();
            connection.Close();
            return dictionary;
        }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}
*/