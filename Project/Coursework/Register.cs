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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {

        }

        private string connectionString =
        "Data Source=AmjadAzward\\SQLEXPRESS;Initial Catalog=rentals;Integrated Security=True";

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRFullName.Text))
                {
                    MessageBox.Show("Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRFullName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtREmail.Text) || !txtREmail.Text.Contains("@") || !txtREmail.Text.Contains("."))
                {
                    MessageBox.Show("A valid email address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtREmail.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRPhone.Text) || txtRPhone.Text.Length < 10 || !txtRPhone.Text.All(char.IsDigit))
                {
                    MessageBox.Show("A valid phone number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRPhone.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRPassword.Text) || txtRPassword.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRPassword.Focus();
                    return;
                }

                if (txtRPassword.Text != txtCpass.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCpass.Focus();
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string Users1 = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(Users1, connection))
                    {
                        command.Parameters.AddWithValue("@Email", txtREmail.Text.Trim());
                        int emailCount = (int)command.ExecuteScalar();
                        if (emailCount > 0)
                        {
                            MessageBox.Show("This email address is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtREmail.Focus();
                            return;
                        }
                    }

                    string Employees1 = "SELECT COUNT(*) FROM Employees WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(Employees1, connection))
                    {
                        command.Parameters.AddWithValue("@Email", txtREmail.Text.Trim());
                        int emailCount = (int)command.ExecuteScalar();
                        if (emailCount > 0)
                        {
                            MessageBox.Show("This email address is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtREmail.Focus();
                            return;
                        }
                    }


                    string Customers1 = "SELECT COUNT(*) FROM Customers WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(Customers1, connection))
                    {
                        command.Parameters.AddWithValue("@Email", txtREmail.Text.Trim());
                        int emailCount = (int)command.ExecuteScalar();
                        if (emailCount > 0)
                        {
                            MessageBox.Show("This email address is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtREmail.Focus();
                            return;
                        }
                    }


                    string Suppliers1 = "SELECT COUNT(*) FROM Suppliers WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(Suppliers1, connection))
                    {
                        command.Parameters.AddWithValue("@Email", txtREmail.Text.Trim());
                        int emailCount = (int)command.ExecuteScalar();
                        if (emailCount > 0)
                        {
                            MessageBox.Show("This email address is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtREmail.Focus();
                            return;
                        }
                    }


                    string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(checkUsernameQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", txtRUsername.Text.Trim());
                        int usernameCount = (int)command.ExecuteScalar();
                        if (usernameCount > 0)
                        {
                            MessageBox.Show("This username is already taken.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtRUsername.Focus();
                            return;
                        }
                    }

                    string Users = "SELECT COUNT(*) FROM Users WHERE Phone = @Phone";
                    using (SqlCommand command = new SqlCommand(Users, connection))
                    {
                        command.Parameters.AddWithValue("@Phone", txtRPhone.Text.Trim());
                        int phoneCount = (int)command.ExecuteScalar();
                        if (phoneCount > 0)
                        {
                            MessageBox.Show("This phone number is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtRPhone.Focus();
                            return;
                        }
                    }

                    string Employees = "SELECT COUNT(*) FROM Employees WHERE Phone = @Phone";
                    using (SqlCommand command = new SqlCommand(Employees, connection))
                    {
                        command.Parameters.AddWithValue("@Phone", txtRPhone.Text.Trim());
                        int phoneCount = (int)command.ExecuteScalar();
                        if (phoneCount > 0)
                        {
                            MessageBox.Show("This phone number is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtRPhone.Focus();
                            return;
                        }
                    }


                    string Customers = "SELECT COUNT(*) FROM Customers WHERE Phone = @Phone";
                    using (SqlCommand command = new SqlCommand(Customers, connection))
                    {
                        command.Parameters.AddWithValue("@Phone", txtRPhone.Text.Trim());
                        int phoneCount = (int)command.ExecuteScalar();
                        if (phoneCount > 0)
                        {
                            MessageBox.Show("This phone number is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtRPhone.Focus();
                            return;
                        }
                    }


                    string Suppliers = "SELECT COUNT(*) FROM Suppliers WHERE Contact = @Contact";
                    using (SqlCommand command = new SqlCommand(Suppliers, connection))
                    {
                        command.Parameters.AddWithValue("@Contact", txtRPhone.Text.Trim());
                        int phoneCount = (int)command.ExecuteScalar();
                        if (phoneCount > 0)
                        {
                            MessageBox.Show("This phone number is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtRPhone.Focus();
                            return;
                        }
                    }


                    string query = "INSERT INTO Users (FullName, Username, Phone, Email, Password) " +
                                   "VALUES (@FullName, @Username, @Phone, @Email, @Password)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", txtRFullName.Text.Trim());
                        command.Parameters.AddWithValue("@Username", txtRUsername.Text.Trim());
                        command.Parameters.AddWithValue("@Phone", txtRPhone.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtREmail.Text.Trim());
                        command.Parameters.AddWithValue("@Password", txtRPassword.Text.Trim());  

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Login loginForm = new Login();
                            loginForm.Show();
                            this.Hide(); 
                            ClearFields();
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ClearFields()
        {
            txtRFullName.Clear();
            txtRUsername.Clear();
            txtRPassword.Clear();
            txtREmail.Clear();
            txtRPhone.Clear();
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {
            Login my = new Login();
            my.Show();
            this.Hide();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtRFullName_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel11_Click(object sender, EventArgs e)
        {

        }

        private void txtRUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtREmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCpass_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
