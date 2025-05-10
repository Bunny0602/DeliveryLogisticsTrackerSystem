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
    public partial class adminDashboard : Form
    {

        Database db = new Database();

        public adminDashboard()
        {
            InitializeComponent();
            totalUsers();
            personnelDataTable();
            Refresh();
        }

        private void totalUsers()
        {
            int totalUser = db.TotalUsers();
            lblTotalUsers.Text = totalUser.ToString();
        }

        private void personnelDataTable()
        {
            try
            {
                DataTable personneldata = db.PersonnelData();
                DataPersonnel.DataSource = personneldata;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading personnel data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnViewPersonnel_Click(object sender, EventArgs e)
        {
            if (DataPersonnel.CurrentRow != null && DataPersonnel.CurrentRow.Cells["ID"].Value != null)
            {
                try
                {
                    int selectedID = Convert.ToInt32(DataPersonnel.CurrentRow.Cells["ID"].Value);

                    ViewPersonnel viewPersonnel = new ViewPersonnel(selectedID);
                    viewPersonnel.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening personnel view: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a personnel to view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddpersonnel_Click(object sender, EventArgs e)
        {
            AddPersonnel addForm = new AddPersonnel();
            addForm.ShowDialog();

        }


        private void Refresh()
        {
            try
            {
                DataTable personneldata = db.PersonnelData();

                DataPersonnel.DataSource = personneldata;
                totalUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void btnDelPersonnel_Click(object sender, EventArgs e)
        {
            if(DataPersonnel.SelectedRows.Count > 0)
            {
                int selectedID = Convert.ToInt32(DataPersonnel.SelectedRows[0].Cells["ID"].Value);
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this personnel?", "Delete Personnel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    bool isDeleted = db.DelPersonnel(selectedID);

                    if (isDeleted)
                    {
                        MessageBox.Show("Personnel deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting personnel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a personnel to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdatePersonnel_Click(object sender, EventArgs e)
        {
            if(DataPersonnel.SelectedRows.Count > 0)
            {
                int selectedID = Convert.ToInt32(DataPersonnel.SelectedRows[0].Cells["ID"].Value);
                UpdatePersonnel updateForm = new UpdatePersonnel(selectedID);
                updateForm.ShowDialog();
                Refresh();
            }
            else
            {
                MessageBox.Show("Please select a personnel to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
