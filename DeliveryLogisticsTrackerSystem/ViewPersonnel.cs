using MySql.Data.MySqlClient;
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

namespace DeliveryLogisticsTrackerSystem
{
    public partial class ViewPersonnel : Form
    {

        private int personnelID;
        private Database db = new Database();

        public ViewPersonnel(int id)
        {
            InitializeComponent();
            personnelID = id;
            ViewPersonnelData();
        }

        private void ViewPersonnelData()
        {
            DataTable personnelData = db.PersonnelID(personnelID);

            if (personnelData.Rows.Count > 0)
            {
                DataRow row = personnelData.Rows[0];

                txtEmail.Text = row["email"].ToString();
                txtFullName.Text = row["full_name"].ToString();
                txtAddress.Text = row["address"].ToString();
                txtPhoneNumber.Text = row["phone_number"].ToString();
                txtRole.Text = row["role"].ToString();
                txtStatus.Text = row["status"].ToString();

                if (row["profile_image"] != DBNull.Value)
                {
                    byte[] profileData = (byte[])row["profile_image"];
                    using (MemoryStream ms = new MemoryStream(profileData))
                    {
                        CirclepbProfile.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    CirclepbProfile.Image = null;
                }
            }
            else
            {
                MessageBox.Show("No personnel data found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
