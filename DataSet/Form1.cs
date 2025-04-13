using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Runtime.InteropServices;
using System.Configuration;
using System.Data.SqlClient;

namespace AcademyDataSet
{
    public partial class MainForm : Form
    {
        readonly string CONNECTION_STRING = "";
        SqlConnection connection = null;
        DataSet GroupsRelatedData = null;
        public MainForm()
        {
            InitializeComponent();
            CONNECTION_STRING = ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString;
            connection = new SqlConnection(CONNECTION_STRING);
            AllocConsole();
            Console.WriteLine(CONNECTION_STRING);
            LoadGroupsRelatedData();

        }

        void LoadGroupsRelatedData()
        { 
            //1) создаём dataSet
            GroupsRelatedData = new DataSet();

            //2.1) добавляем таблицы в dataset
            const string dsTable_Directions = "Directions";
            const string dst_col_direction_id = "direction_id";
            const string dst_col_direction_name = "direction_name";
            GroupsRelatedData.Tables.Add(dsTable_Directions);
            //2.2) добавляем поля(столбики) в таблицу  
            GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id, typeof(byte));
            GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name, typeof(string));
            //2.3) определяем какое поле является первичным ключем:
            GroupsRelatedData.Tables[dsTable_Directions].PrimaryKey =
               new DataColumn[] { GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id] };

            const string dsTable_groups = "Groups";
            const string dst_Groups_col_group_id = "group_id";
            const string dst_Groups_col_group_name = "group_name";
            const string dst_Groups_col_group_direction = "direction";

            GroupsRelatedData.Tables.Add(dsTable_groups);
            GroupsRelatedData.Tables[dsTable_groups].Columns.Add(dst_Groups_col_group_id, typeof(int));
            GroupsRelatedData.Tables[dsTable_groups].Columns.Add(dst_Groups_col_group_name, typeof(string));
            GroupsRelatedData.Tables[dsTable_groups].Columns.Add(dst_Groups_col_group_direction, typeof(byte));
            GroupsRelatedData.Tables[dsTable_groups].PrimaryKey =
                new DataColumn[] { GroupsRelatedData.Tables[dsTable_groups].Columns[0] };

            //3) строим связи между таблицами 
            GroupsRelatedData.Relations.Add
                (
                    "GroupsDirections",
                    GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id],//parent field первичный ключ в другой таблице
                    GroupsRelatedData.Tables[dsTable_groups].Columns[dst_Groups_col_group_direction]//child field внешний ключ
                );

            //4) загрузка данных в DataSet
            string directionsCmd = "SELECT * FROM Directions";
            string groupsCmd = "SELECT * FROM Groups";
            SqlDataAdapter directionsAdapter = new SqlDataAdapter(directionsCmd, connection);
            SqlDataAdapter groupsAdapter = new SqlDataAdapter(groupsCmd, connection);

            directionsAdapter.Fill(GroupsRelatedData.Tables[dsTable_Directions]);
            groupsAdapter.Fill(GroupsRelatedData.Tables[dsTable_groups]);

            for (int i = 0; i < GroupsRelatedData.Tables["directions"].Rows.Count; i++)
            {
                Console.Write(GroupsRelatedData.Tables["Directions"].Rows[i] + ":\t");
                for (int j = 0; j < GroupsRelatedData.Tables["Directions"].Columns.Count; j++)
                {
                    Console.Write(GroupsRelatedData.Tables["Directions"].Rows[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }
        
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}
