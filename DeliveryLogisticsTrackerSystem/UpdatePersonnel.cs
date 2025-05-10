using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DeliveryLogisticsTrackerSystem
{
    public partial class UpdatePersonnel : Form
    {
        private int personnelID;
        private Database db = new Database();
        private byte[] ProfileImage;

        public UpdatePersonnel(int id)
        {
            InitializeComponent();
            personnelID = id;
            PersonnelInfo();
        }

        public void PersonnelInfo()
        {
            DataTable personnelData = db.PersonnelID(personnelID);

            if (personnelData != null && personnelData.Rows.Count > 0)
            {
                DataRow row = personnelData.Rows[0];

                txtEmail.Text = row["email"].ToString();
                txtFullName.Text = row["full_name"].ToString();
                txtAddress.Text = row["address"].ToString();
                txtPhone.Text = row["phone_number"].ToString();

                if (row.Table.Columns.Contains("profile_image") && row["profile_image"] != DBNull.Value)
                {
                    byte[] profileImage = (byte[])row["profile_image"];
                    using (MemoryStream ms = new MemoryStream(profileImage))
                    {
                        ProfileUpdate.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    ProfileUpdate.Image = null;
                }

                if (row.Table.Columns.Contains("password") && row["password"] != DBNull.Value)
                {
                    txtPass.Text = row["password"].ToString();
                }
                else
                {
                    txtPass.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("No personnel data found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private void btnUpDateImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string imagePath = ofd.FileName;
                    ProfileImage = File.ReadAllBytes(imagePath);
                    ProfileUpdate.Image = Image.FromFile(imagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string fullname = txtFullName.Text;
            string phone = txtPhone.Text;
            string password = txtPass.Text;
            string address = txtAddress.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isUpdated = db.UpdatePersonnel(personnelID, email, fullname, phone, address, password, ProfileImage);

            if (isUpdated)
            {
                MessageBox.Show("Personnel updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error updating personnel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
