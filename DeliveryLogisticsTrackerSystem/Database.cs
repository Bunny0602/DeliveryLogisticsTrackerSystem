using System;
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
    }
}
