using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WPF
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
            _timer = new System.Timers.Timer(300000); // 5мин
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
                    if (HasParents(table.TableName))
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
