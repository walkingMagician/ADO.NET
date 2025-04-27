using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using ConnectorLibrary;

namespace CacheLibrary
{
    public class Cache
    {
        DataSet set = null;
        public DataSet Set { get => this.set; }
        Connector _connector;

        private readonly System.Timers.Timer _timer;
        private bool _timer_check = false;

        public Cache(string connectionString)
        {
            _connector = new Connector(connectionString);
            set = new DataSet();

            // время таймер //
            _timer = new System.Timers.Timer(3000); // 5мин
            _timer.Elapsed += TimerUpdate;
            _timer.AutoReset = true;
            _timer.Enabled = true;

        }

        private void TimerUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_timer_check) return;

            try
            {
                _timer_check = true;
                Console.WriteLine("Обновление БД " + DateTime.Now);
                
                foreach (DataTable table in set.Tables)
                {
                    if(HasParents(table.TableName))
                        _connector.RefreshTableConnector(set, table.TableName);
                }

                Console.WriteLine("БД обновлена " + DateTime.Now);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка обновления БД " + ex.Message);
            }
            finally 
            { 
                _timer_check = false; 
            }
        }

        public void AddTable(string tableName, string columns)
        {
            _connector.AddTableConnector(set, tableName, columns);
            print(tableName);
        }

        public void AddRelation(string relation_name, string child, string parents)
        {
            _connector.AddRelationConnector(set, relation_name, child, parents);
        }
        public void Dispose()
        { 
            _timer?.Dispose();
            _connector?.Dispose();
        }

        /*private void RefreshTable(string tableName)
        {
            if (!set.Tables.Contains(tableName)) return;
            DataTable dt = set.Tables[tableName];
            string columns = string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            dt.Rows.Clear();
            string cmd = $"SELECT {columns} FROM {tableName}";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
            adapter.Fill(dt);
        }

        public void AddTable(string table, string columns)
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
            SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
            adapter.Fill(set.Tables[table]);
            print(table);

        }

        public void AddRelation(string relation_name, string child, string parent)
        {
            set.Relations.Add
                (
                    relation_name,
                    set.Tables[parent.Split(',')[0]].Columns[parent.Split(',')[1]],
                    set.Tables[child.Split(',')[0]].Columns[child.Split(',')[1]]
                );
        }*/

        //void LoadGroupsRelatedData()
        //{

        //    //1) создаём dataSet
        //    set = new DataSet();

        //    //2.1) добавляем таблицы в dataset
        //    const string dsTable_Directions = "Directions";
        //    const string dst_col_direction_id = "direction_id";
        //    const string dst_col_direction_name = "direction_name";
        //    set.Tables.Add(dsTable_Directions);
        //    //2.2) добавляем поля(столбики) в таблицу  
        //    set.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id, typeof(byte));
        //    set.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name, typeof(string));
        //    //2.3) определяем какое поле является первичным ключем:
        //    set.Tables[dsTable_Directions].PrimaryKey =
        //       new DataColumn[] { set.Tables[dsTable_Directions].Columns[dst_col_direction_id] };

        //    const string dsTable_groups = "Groups";
        //    const string dst_Groups_col_group_id = "group_id";
        //    const string dst_Groups_col_group_name = "group_name";
        //    const string dst_Groups_col_group_direction = "direction";

        //    set.Tables.Add(dsTable_groups);
        //    set.Tables[dsTable_groups].Columns.Add(dst_Groups_col_group_id, typeof(int));
        //    set.Tables[dsTable_groups].Columns.Add(dst_Groups_col_group_name, typeof(string));
        //    set.Tables[dsTable_groups].Columns.Add(dst_Groups_col_group_direction, typeof(byte));
        //    set.Tables[dsTable_groups].PrimaryKey =
        //        new DataColumn[] { set.Tables[dsTable_groups].Columns[0] };

        //    //3) строим связи между таблицами 
        //    set.Relations.Add
        //        (
        //            "GroupsDirections",
        //            set.Tables[dsTable_Directions].Columns[dst_col_direction_id],//parent field первичный ключ в другой таблице
        //            set.Tables[dsTable_groups].Columns[dst_Groups_col_group_direction]//child field внешний ключ
        //        );

        //    //4) загрузка данных в DataSet
        //    string directionsCmd = "SELECT * FROM Directions";
        //    string groupsCmd = "SELECT * FROM Groups";
        //    SqlDataAdapter directionsAdapter = new SqlDataAdapter(directionsCmd, connection);
        //    SqlDataAdapter groupsAdapter = new SqlDataAdapter(groupsCmd, connection);

        //    directionsAdapter.Fill(set.Tables[dsTable_Directions]);
        //    groupsAdapter.Fill(set.Tables[dsTable_groups]);

        //    print(dsTable_Directions);
        //    print(dsTable_groups);
        //}

        public void print(string table)
        {
            Console.WriteLine("\n==================================================");
            for (int i = 0; i < set.Tables[table].Columns.Count; i++)
                Console.Write(set.Tables[table].Columns[i].Caption + "\t\t");
            Console.WriteLine("\n--------------------------------------------------\n");

            int number_of_parens = set.Tables[table].ParentRelations.Count;
            for (int i = 0; i < number_of_parens; i++)
                Console.WriteLine(set.Tables[table].ParentRelations[i].ToString());

            //Console.Write(set.Tables[table].ParentRelations.Contains("direction"));
            for (int i = 0; i < set.Tables[table].Rows.Count; i++)
            {
                //Console.Write(set.Tables[table].Rows[i] + ":\t");
                for (int j = 0; j < set.Tables[table].Columns.Count; j++)
                {
                    if (HasParents(table) && set.Tables[table].ParentRelations[0].ChildColumns.Contains(set.Tables[table].Columns[j]))
                    {
                        string parent_relation_name = !HasParents(table) ? "" : $"{set.Tables[table].TableName}{set.Tables[table].Columns[j].ColumnName}s";
                        string parent_column_name = $"{set.Tables[table].Columns[j].ColumnName}_name";
                        Console.WriteLine(
                            //set.Tables[table].ParentRelations[0].ParentColumns[0]
                            set.Tables[table].Rows[i].GetParentRow(parent_relation_name)[parent_column_name]
                            );
                    }
                    else
                        Console.Write(set.Tables[table].Rows[i][j] + "\t");
                    /*if (
                        set.Tables[table].ParentRelations.Contains
                        (parent_relation_name) && 
                       )
                    {
                        Console.WriteLine(
                            set.Tables[table].Rows[i].
                            GetParentRow(parent_relation_name)[$"{set.Tables[table].Columns[i].ColumnName}_name"]
                            );
                    }*/
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n==================================================");
        }

        public bool HasParents(string table)
        {
            return set.Tables[table].ParentRelations.Count > 0;
        }

        public void printGroups()
        {
            Console.WriteLine("\n==================================================");
            string table = "Groups";
            for (int i = 0; i < set.Tables[table].Rows.Count; i++)
            {
                for (int j = 0; j < set.Tables[table].Columns.Count; j++)
                {
                    Console.Write(set.Tables[table].Rows[i][j] + "\t");
                }
                Console.WriteLine(set.Tables[table].Rows[i].GetParentRow("GroupsDirections")["direction_name"]);
                Console.WriteLine();
            }
            Console.WriteLine("\n==================================================");
        }

    }
}
