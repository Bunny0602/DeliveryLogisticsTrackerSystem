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
using System.IO;

namespace DeliveryLogisticsTrackerSystem
{
    public partial class AddPersonnel : Form
    {

        Database db = new Database();
        private byte[] ProfileImage = null;

        public AddPersonnel()
        {
            InitializeComponent();
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string imagePath = openFileDialog.FileName;

                        ProfileImage = File.ReadAllBytes(imagePath);

                        profileImage.Image = Image.FromFile(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAddpersonnel_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPass.Text;
            string fullname = txtFullName.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            string role = "Driver";
            string status = "Available";


            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                byte[] profileImageData = null;
                if (profileImage != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        profileImage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        profileImageData = ms.ToArray();
                    }
                }

                Database db = new Database();

                bool isSaved = db.Addpersonnel(email, fullname, phone, address, role, status, password, ProfileImage);

                if (isSaved)
                {
                    MessageBox.Show("Personnel added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error adding personnel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }   
    }
}
