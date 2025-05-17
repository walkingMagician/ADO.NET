using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace WPF
{
    public partial class MainWindow : Window
    {

        Cache cache;
        SqlConnection connector = new SqlConnection(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);

        public MainWindow()
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


            //cbDirection.DataSource = cache.Set.Tables["Directions"];
            //cbDirection.ValueMember = "direction_id";
            //cbDirection.DisplayMember = "direction_name";


            //cbGroup.DataSource = cache.Set.Tables["Groups"];
            //cbGroup.ValueMember = "group_id";
            //cbGroup.DisplayMember = "group_name";

            // Загружаем направления
            SqlDataAdapter directionsAdapter = new SqlDataAdapter(
                "SELECT direction_id, direction_name FROM Directions", connector);
            directionsAdapter.Fill(cache.Set, "Directions");

            // Загружаем все группы
            SqlDataAdapter groupsAdapter = new SqlDataAdapter(
                "SELECT group_id, group_name, direction_id FROM Groups, Directions", connector);
            groupsAdapter.Fill(cache.Set, "Groups");

            // Привязываем направления
            cbDirection.ItemsSource = cache.Set.Tables["Directions"].DefaultView;
            cbDirection.SelectedIndex = 0;

        }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        //private void cbDirection_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    object selectedValue = (sender as ComboBox).SelectedValue;
        //    string filter = $"direction = {selectedValue.ToString()}";
        //    Console.WriteLine(filter);
        //    cache.Set.Tables["Groups"].DefaultView.RowFilter = filter;
        //}

        private void cbDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedValue = (sender as ComboBox).SelectedValue;
            string filter = $"direction = {selectedValue.ToString()}";
            Console.WriteLine(filter);
            //cache.Set.Tables["Groups"].DefaultView.RowFilter = filter;
        }
    }
}
