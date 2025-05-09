using System;
using System.Data;
using System.Linq.Expressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DeliveryLogisticsTrackerSystem
{
    public class Database
    {
        private string connectionString = "Server=localhost;Database=delivery_logistics;Uid=root;Pwd=;";

        public MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return conn;
        }

        public bool CheckLogin(string email, string password, out string userType)
        {
            userType = "";
            MySqlConnection conn = GetConnection();

            if (conn == null) return false;

            try
            {
                string query = "SELECT role FROM users WHERE email = @Email AND password = @Password";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userType = reader["role"].ToString();
                    reader.Close();
                    return true;
                }
                reader.Close();

                query = "SELECT email FROM personnel WHERE email = @Email AND password = @Password";
                cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userType = "Personnel";
                    reader.Close();
                    return true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public bool CheckEmail(string email)
        {
            MySqlConnection conn = GetConnection();
            if (conn == null) return false;

            try
            {
                string query = "SELECT email FROM users WHERE email = @Email";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                MySqlDataReader reader = cmd.ExecuteReader();
                bool exist = reader.HasRows;
                reader.Close();
                return exist;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public int TotalUsers()
        {
            int totalUser = 0;
            MySqlConnection conn = GetConnection();
            if (conn == null) return totalUser;

            try
            {
                string query = "SELECT COUNT(*) AS total from users WHERE role = 'User'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    totalUser = Convert.ToInt32(reader["total"]);
                }

                reader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error getting total user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return totalUser;
        }

        public DataTable PersonnelData()
        {
            DataTable personnelTable = new DataTable();
            MySqlConnection conn = GetConnection();
            if (conn == null) return personnelTable;

            try
            {
                string query = @"SELECT personnel_id AS 'ID', email AS 'Email', full_name AS 'Full Name', phone_number AS 'Phone Number', role AS 'Role', status AS 'Status', address AS 'Address' FROM personnel";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(personnelTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting personnel data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return personnelTable;
        }

        public DataTable PersonnelID(int id)
        {
            DataTable personnelid = new DataTable();

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    string query = @"SELECT email, full_name, phone_number, role, status, address, profile_image FROM personnel WHERE personnel_id = @ID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(personnelid);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error getting personnel ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return personnelid;
        }

        public bool Addpersonnel(string email, string fullname, string phone_number, string address, string role, string status, string password, byte[] profileimage)
        {
            using (MySqlConnection conn = GetConnection())
            {
                string query = @"INSERT INTO personnel (email, full_name, phone_number, address, role, status, password, profile_image) VALUE (@Email, @FullName, @PhoneNumber, @Address, @Role, @Status, @Password, @Profile)";


                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@FullName", fullname);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phone_number);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Password", password);

                        if (profileimage == null)
                        {
                            cmd.Parameters.AddWithValue("@Profile", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Profile", profileimage);
                        }

                            int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error adding personnel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
    }
}
