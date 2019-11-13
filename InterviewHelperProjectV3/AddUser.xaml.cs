using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InterviewHelperProjectV3
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private OleDbConnection con;

        public AddUser()
        {
            InitializeComponent();

            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\InterviewHelperDB.mdb";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (txtPasswordFirst.Password == txtPasswordSecond.Password && txtUsername.Text != "" && txtFullName.Text != "")
            {

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;

                cmd.CommandText = "SELECT UserName FROM tbLogin WHERE UserName=@Username";
                cmd.Parameters.AddWithValue("Username", txtUsername.Text);

                var userReader = cmd.ExecuteScalar();

                if (userReader == null)
                {
                    cmd.CommandText = "insert into tbLogin(UserName, UserPassword, UserFullName) Values(@Username, @Password, @FullName)";
                    cmd.Parameters.AddWithValue("Username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("Password", txtPasswordFirst.Password);
                    cmd.Parameters.AddWithValue("FullName", txtFullName.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Successfully Added");

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username already in use");
                }

                
            }
            else
            {
                MessageBox.Show("Please fill boxes or passwords dont match");
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
