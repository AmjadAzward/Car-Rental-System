using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;


namespace Coursework
{
    public partial class Login : Form
    {
        private string connectionString =
          "Data Source=AmjadAzward\\SQLEXPRESS;Initial Catalog=rentals;Integrated Security=True";

        public Login()
        {
            InitializeComponent();

        }


        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }



        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLPassword.Text))
                {
                    MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLPassword.Focus();
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Password FROM Users WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", txtLUsername.Text.Trim());

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string storedPassword = result.ToString(); 

                            if (txtLPassword.Text == storedPassword)
                            {
                                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Form1 dashboard = new Form1(txtLUsername.Text.Trim()); 
                                dashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid username.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
        }

        private void guna2HtmlLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {
            Register my = new Register();
            my.Show();
            this.Hide();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtLUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_MouseEnter(object sender, EventArgs e)
        {
            txtLPassword.PasswordChar = '\0';

        }

        private void guna2ImageButton1_MouseDown(object sender, MouseEventArgs e)
        {
            txtLPassword.PasswordChar = '\0';

        }

        private void guna2ImageButton1_MouseLeave(object sender, EventArgs e)
        {
            txtLPassword.PasswordChar = '*';

        }

        private void guna2ImageButton1_MouseUp(object sender, MouseEventArgs e)
        {
            txtLPassword.PasswordChar = '*';

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            guna2ImageButton1.HoverState.ImageSize = guna2ImageButton1.ImageSize;
        }
    }
    
}
