using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Coursework
{
    public partial class profile : Form
    {
        string connectionString = "Data Source=AmjadAzward\\SQLEXPRESS;Initial Catalog=rentals;Integrated Security=True";
        string loggedInUsername;
        string username="";

        
        public profile(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadUserProfile(username);

        }

        //profile to home page button 
        private void guna2PictureBox1_Click_1(object sender, EventArgs e)
        {
            Form1 my = new Form1(this.username);
            my.Show();
            this.Hide();

            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
        }

        //form load
        private void profile_Load(object sender, EventArgs e)
        {

        }

        //load the profile in relevat places 
        public void LoadUserProfile(string userName)
        {
            string query = "SELECT UserId, FullName, Username, Email, Phone, Password FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", userName);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            // Load user data from database
                            txtName.Text = reader["FullName"].ToString();
                            txtPUsername.Text = reader["Username"].ToString();
                            txtPPhone.Text = reader["Phone"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            pw.Text = reader["Password"].ToString();
                            cpw.Text = reader["Password"].ToString();

                            string imagePath = Path.Combine(@"C:\Users\USER\OneDrive\Pictures\Rentals", txtPUsername.Text + ".png");

                            if (File.Exists(imagePath))
                            {
                                pictureBox.Image = Image.FromFile(imagePath);
                            }
                            else
                            {
                                //pictureBox.Image = null; 
                            }
                        }
                        else
                        {
                            MessageBox.Show("User data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // update the user data 
        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Full Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPPhone.Text) || txtPPhone.Text.Length < 10 || !txtPPhone.Text.All(char.IsDigit))
                {
                    MessageBox.Show("A valid phone number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPPhone.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
                {
                    MessageBox.Show("A valid email address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                if (pw.Text != cpw.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cpw.Focus();
                    return;
                }

                string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @NewUsername AND Username != @LoggedInUsername";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(checkUsernameQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewUsername", txtPUsername.Text.Trim());
                        command.Parameters.AddWithValue("@LoggedInUsername", this.username);

                        connection.Open();
                        int usernameCount = (int)command.ExecuteScalar();

                        if (usernameCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPUsername.Focus();
                            return;
                        }
                    }
                }

                string updateQuery = "UPDATE Users SET FullName = @FullName, Username = @Username, Phone = @Phone, Email = @Email, Password = @Password WHERE Username = @LoggedInUsername";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        command.Parameters.AddWithValue("@Username", txtPUsername.Text.Trim()); 
                        command.Parameters.AddWithValue("@Phone", txtPPhone.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue("@Password", pw.Text.Trim()); 
                        command.Parameters.AddWithValue("@LoggedInUsername", this.username); 

                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.username = txtPUsername.Text.Trim();
                        }
                        else
                        {
                            MessageBox.Show("Profile update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // upload image
        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string username = txtPUsername.Text.Trim();

                    if (!string.IsNullOrEmpty(username))
                    {
                        string selectedFilePath = openFileDialog.FileName;

                        string targetDirectory = @"C:\Users\USER\OneDrive\Pictures\Rentals";

                        if (!Directory.Exists(targetDirectory))
                        {
                            Directory.CreateDirectory(targetDirectory);
                        }

                        string newFilePath = Path.Combine(targetDirectory, username + ".png");

                        if (pictureBox.Image != null)
                        {
                            pictureBox.Image.Dispose();
                        }

                        File.Copy(selectedFilePath, newFilePath, true);
                        pictureBox.Image = new System.Drawing.Bitmap(newFilePath);
                        MessageBox.Show("Image uploaded and updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid employee ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading or renaming image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No file selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

      

        // logout button with confirmation
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Login loginForm = new Login();
                loginForm.Show();
            }
        }

        //close the application
        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
      
        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {

        }

        private void txtPPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtPUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
