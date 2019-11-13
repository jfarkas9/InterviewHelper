using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace InterviewHelperProjectV3
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private OleDbConnection con;

        public LoginWindow()
        {
            InitializeComponent();

            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\InterviewHelperDB.mdb";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            cmd.CommandText = "SELECT * FROM tbLogin WHERE UserName=@Username AND UserPassword=@Password";
            cmd.Parameters.AddWithValue("Username", txtUsername.Text);
            cmd.Parameters.AddWithValue("Password", txtPassword.Password);
            
            OleDbDataReader userReader = cmd.ExecuteReader();

            //https://docs.microsoft.com/en-us/dotnet/api/system.data.oledb.oledbcommand?view=netframework-4.8
            userReader.Read();
           
            if(!userReader.HasRows)
            {
                MessageBox.Show("Username and/or Password not found");
            }
            else
            {
                int uId = userReader.GetInt32(0);
                string uName = userReader.GetString(1);
                string uFName = userReader.GetString(3);

                User currentUser = new User(uId, uName, uFName);
                //System.Diagnostics.Debug.WriteLine(currentUser);

                MainWindow main = new MainWindow(currentUser);

                main.Show();
                this.Close();
            }

            
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser();

            addUser.Show();
            //this.Hide();
        }
    }
}