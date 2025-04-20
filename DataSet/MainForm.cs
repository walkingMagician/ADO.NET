﻿using System;
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
        Cache cache;

        public MainForm()
        {
            InitializeComponent();
            AllocConsole();

            //1) создаём dataSet
            cache = new Cache(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
            //set = new DataSet("GroupsRelatedData");
            cache.AddTable("Directions", "direction_id,direction_name");
            cache.AddTable("Groups", "group_id,group_name,direction");
            cache.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
            //printGroups();
            Console.WriteLine(cache.HasParents("Groups"));
            cache.print("Groups");
            //LoadGroupsRelatedData();

        }

        

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}
