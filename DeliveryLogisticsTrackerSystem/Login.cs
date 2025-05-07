using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DeliveryLogisticsTrackerSystem
{
    public partial class Login : Form
    {
        Database db = new Database();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userType;
            bool isLoggedIn = db.CheckLogin(email, password, out userType);

            if (isLoggedIn)
            {
                MessageBox.Show($"Login successful! You are logged in as {userType}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();

                if (userType == "Admin")
                {
                    adminDashboard AdminDashboard = new adminDashboard();
                    AdminDashboard.ShowDialog();
                }

                else if (userType == "Personnel")
                {
                    PersonnelDashboard personnelDashboard = new PersonnelDashboard();
                    personnelDashboard.ShowDialog();
                }

                else
                {
                    userDashboard UserDashboard = new userDashboard();
                    UserDashboard.ShowDialog();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Register register = new Register();
            register.ShowDialog();
            this.Show();
        }
    }
}
