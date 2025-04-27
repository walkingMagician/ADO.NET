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

using CacheLibrary;

namespace AcademyDataSet
{
    public partial class MainForm : Form
    {
        Cache cache;
        //Connector connector;
        
        public MainForm()
        {
            InitializeComponent();
            AllocConsole();

            //1) создаём dataSet
            cache = new Cache(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
            //connector = new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
            
            //set = new DataSet("GroupsRelatedData");
            cache.AddTable("Directions", "direction_id,direction_name");
            cache.AddTable("Groups", "group_id,group_name,direction");
            cache.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
            //printGroups();
            Console.WriteLine(cache.HasParents("Groups"));
            cache.print("Groups");
            //LoadGroupsRelatedData();


            cbDirection.DataSource = cache.Set.Tables["Directions"];
            cbDirection.ValueMember = "direction_id";
            cbDirection.DisplayMember = "direction_name";

            cbGroup.DataSource = cache.Set.Tables["Groups"];
            cbGroup.ValueMember = "group_id";
            cbGroup.DisplayMember = "group_name";
        }

        

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        private void cbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selectedValue = (sender as ComboBox).SelectedValue;
            string filter = $"direction = {selectedValue.ToString()}";
            Console.WriteLine(filter);
            cache.Set.Tables["Groups"].DefaultView.RowFilter = filter;
        }
    }
}
