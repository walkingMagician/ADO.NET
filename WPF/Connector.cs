using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace WPF
{
    public class Connector
    {
        private readonly string _CONNECTION_STRING;
        private SqlConnection _connection;

        public Connector(string connectionString)
        {
            _CONNECTION_STRING = connectionString;
            _connection = new SqlConnection(_CONNECTION_STRING);
            Console.WriteLine(_CONNECTION_STRING);
        }

        public void AddTableConnector(DataSet set, string table, string columns)
        {
            //2.1) добавляем таблицу в dataset
            set.Tables.Add(table);

            //2.2) добавляем поля(столбики) в таблицу  
            string[] a_columns = columns.Split(',');
            for (int i = 0; i < a_columns.Length; i++)
            {
                set.Tables[table].Columns.Add(a_columns[i]);
            }
            //2.3) определяем какое поле является первичным ключем:
            set.Tables[table].PrimaryKey =
               new DataColumn[] { set.Tables[table].Columns[0] };

            string cmd = $"SELECT {columns} FROM {table}";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd, _connection);
            adapter.Fill(set.Tables[table]);
        }

        public void AddRelationConnector(DataSet set, string relation_name, string child, string parent)
        {
            set.Relations.Add
                (
                    relation_name,
                    set.Tables[parent.Split(',')[0]].Columns[parent.Split(',')[1]],
                    set.Tables[child.Split(',')[0]].Columns[child.Split(',')[1]]
                );
        }

        public void RefreshTableConnector(DataSet set, string tableName)
        {
            if (!set.Tables.Contains(tableName)) return;
            DataTable dt = set.Tables[tableName];
            string columns = string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            dt.Rows.Clear();
            string cmd = $"SELECT {columns} FROM {tableName}";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd, _connection);
            adapter.Fill(dt);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
