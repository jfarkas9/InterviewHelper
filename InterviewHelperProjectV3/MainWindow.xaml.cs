using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Navigation;

namespace InterviewHelperProjectV3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OleDbConnection con;
        private DataTable dt;
        private int UserId;
        private User currentUser;

        public MainWindow(User currentUser)
        {
            this.currentUser = currentUser;
            this.UserId = currentUser.UserId;
            InitializeComponent();

            //Connect your access database
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\InterviewHelperDB.mdb";
            BindGrid();
        }


        //Display records in grid
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from tbInterviewHelper WHERE UserId =" + UserId;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
            dateDateContact.SelectedDate = DateTime.Today;
            lbluserId.Content = "Currently logged in as: " + currentUser.UserFullName;
        }

        //Add records in grid
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtEntryId.Text == "")
            {
                cmd.CommandText = "insert into tbInterviewHelper(DateOfContact,CompanyName,RecruiterName,Status,Location,Salary,Notes,UserId) Values(@DateofContact, @CompanyName, @RecruiterName, @Status, @Location, @Salary, @Notes, @UserId)";
                cmd.Parameters.AddWithValue("DateofContact", dateDateContact.SelectedDate);
                cmd.Parameters.AddWithValue("CompanyName", txtCompanyName.Text);
                cmd.Parameters.AddWithValue("RecruiterName", txtRecruiterName.Text);
                cmd.Parameters.AddWithValue("Status", comboStatus.Text);
                cmd.Parameters.AddWithValue("Location", txtLocation.Text);
                cmd.Parameters.AddWithValue("Salary", txtSalary.Text);
                cmd.Parameters.AddWithValue("Notes", txtNotes.Text);
                cmd.Parameters.AddWithValue("UserId", UserId);
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Entry Added Successfully...");
                ClearAll();
            }
            else
            {
                cmd.CommandText = "update tbInterviewHelper set DateOfContact=@DateofContact, CompanyName=@CompanyName, RecruiterName=@RecruiterName, Status=@Status, Location=@Location, Salary=@Salary, Notes=@Notes where Id=@EntryId";
                cmd.Parameters.AddWithValue("DateofContact", dateDateContact.SelectedDate);
                cmd.Parameters.AddWithValue("CompanyName", txtCompanyName.Text);
                cmd.Parameters.AddWithValue("RecruiterName", txtRecruiterName.Text);
                cmd.Parameters.AddWithValue("Status", comboStatus.Text);
                cmd.Parameters.AddWithValue("Location", txtLocation.Text);
                cmd.Parameters.AddWithValue("Salary", txtSalary.Text);
                cmd.Parameters.AddWithValue("Notes", txtNotes.Text);
                cmd.Parameters.AddWithValue("EntryId", txtEntryId.Text);

                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Entry Details Updated Successfully...");
                ClearAll();
            }
        }

        //Clear all records from controls
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtEntryId.Text = "";
            dateDateContact.SelectedDate = DateTime.Today;
            //dateDateContact.DisplayDate = DateTime.Today;
            txtCompanyName.Text = "";
            txtRecruiterName.Text = "";
            comboStatus.SelectedIndex = 0;
            txtLocation.Text = "";
            Map.IsEnabled = false;
            txtSalary.Text = "";
            txtNotes.Text = "";
            btnAdd.Content = "Add";
        }

        //Edit records
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtEntryId.Text = row["Id"].ToString();
                dateDateContact.SelectedDate = (DateTime)row["DateOfContact"];
                txtCompanyName.Text = row["CompanyName"].ToString();
                txtRecruiterName.Text = row["RecruiterName"].ToString();
                comboStatus.Text = row["Status"].ToString();
                txtLocation.Text = row["Location"].ToString();
                Map.IsEnabled = true;
                txtSalary.Text = row["Salary"].ToString();
                txtNotes.Text = row["Notes"].ToString();
                txtEntryId.IsEnabled = false;
                btnAdd.Content = "Update";
            }
            else
            {
                MessageBox.Show("Please Select Any Interview From List...");
            }
        }

        //Delete records from grid
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from tbInterviewHelper where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Entry Deleted Successfully...");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Please Select Any Entry From List...");
            }
        }

        //Exit
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Open map in Map Window
        private void Hyperlink_RequestNavigate1(object sender, RequestNavigateEventArgs e)
        {
            //String output = e.Uri.ToString();
            String url = GoogleMapUrl(txtLocation.Text, "Map", 0);

            //Open in MapWindow
            MapWindow mapWindow = new MapWindow(url);
            mapWindow.Show();
        }

        private void Hyperlink_RequestNavigate2(object sender, RequestNavigateEventArgs e)
        {
            //String output = e.Uri.ToString();
            String url = GoogleMapUrl(txtLocation.Text, "Map", 0);

            ////Open in Browser
            Process.Start(new ProcessStartInfo(url));
            //e.Handled = true;
        }

        // Return a Google map URL.
        //http://csharphelper.com/blog/2016/05/display-google-maps-for-an-address-in-c/
        private string GoogleMapUrl(string query, string map_type, int zoom)
        {
            // Start with the base map URL.
            string url = "http://maps.google.com/maps?";

            // Add the query.
            url += "q=" + HttpUtility.UrlEncode(query, Encoding.UTF8);

            // Add the type.
            map_type = GoogleMapTypeCode(map_type);
            if (map_type != null) url += "&t=" + map_type;

            // Add the zoom level.
            if (zoom > 0) url += "&z=" + zoom.ToString();

            return url;
        }

        // Return a Google map type code.
        private string GoogleMapTypeCode(string map_type)
        {
            // Insert the proper type.
            switch (map_type)
            {
                case "Map":
                    return "m";

                case "Satellite":
                    return "k";

                case "Hybrid":
                    return "h";

                case "Terrain":
                    return "p";

                case "Google Earth":
                    return "e";

                default:
                    return null;
            }
        }

        private void BtnLogOff_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void BtnChangePass_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword chngPass = new ChangePassword(currentUser);
            chngPass.Show();
        }
    }
}