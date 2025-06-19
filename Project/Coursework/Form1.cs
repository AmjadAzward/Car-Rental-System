using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports.ViewerObjectModel;
using CrystalDecisions.Windows.Forms;
using Guna.UI2.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Coursework
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=AmjadAzward\\SQLEXPRESS;Initial Catalog=rentals;Integrated Security=True";

        string username ;
        public Form1(string username)
        {
            InitializeComponent();
            this.username = username;
            
        }

        // form load 
        private void Form1_Load(object sender, EventArgs e)
        {

            LoadCars();
            PopulateStatusComboBox();
            LoadNextCarID();
            LoadmainSup();

            LoadCustomers();
            LoadNextCustomerID();

            LoadRentalsData();
            LoadNextRentalID();

            //cars
            LoadCarData();
            LoadCustomerData();
            txtbrand.SelectedIndex = -1;
            LoadSupplierNames();
            SUPPLIERCOMBO.SelectedIndex = -1;


            //rentals
            LoadRentalIDs();
            LoadPaymentData();
            LoadPaymentMethods();
            LoadNextPaymentID();

            //maintenance
            LoadMaintenanceRecords();
            LoadNextMaintenanceID();
            LoadMaintenanceMethods();

            //employees
            PopulatePositionComboBox();
            LoadEmployees();
            LoadNextEmployeeID();

            //suplliers
            LoadSuppliers();
            LoadNextSupplierID();
            PopulateServiceProvidedComboBox();

            //dashboard
            LoadAvailableCarsCounts();
            LoadRentedCarsCounts();
            LoadMaintenanceCarsCounts();
            LoadCustomerCount();
            LoadTotalPayments();
            LoadRentalCount();
            LoadEmployeeCount();
        }

  




        // Car
        // Car
        //load next carId from table to textbox 
        private void LoadNextCarID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ISNULL(MAX(CarID), 0) + 1 FROM Cars";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                int nextCarID = (int)cmd.ExecuteScalar();
                txtCarID.Text = nextCarID.ToString();
            }
        }

        //load cars in DGV
        private void LoadCars()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CarID,SupplierID,Brand, Model, RegistrationNumber,Price,Status  FROM Cars";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCars.DataSource = dt;

                dgvcardata();

                if (dt.Rows.Count > 0)
                {
                    dgvCars.DataSource = dt;
                }

            }
        }

        //car datagridview data customize
        private void dgvcardata()
        {
            dgvCars.Font = new Font("Microsoft Sans Serif", 09, FontStyle.Regular);

            dgvCars.Columns["CarID"].Width = 40;
            dgvCars.Columns["SupplierID"].Width = 50;
            dgvCars.Columns["SupplierID"].HeaderText = "Sup Id";

            dgvCars.Columns["CarID"].HeaderText = "ID";

            dgvCars.Columns["Brand"].Width = 95;
            dgvCars.Columns["Model"].Width = 95;
            dgvCars.Columns["RegistrationNumber"].Width = 120;
            dgvCars.Columns["RegistrationNumber"].HeaderText = "Reg Number";

            dgvCars.Columns["Price"].Width = 120;
            dgvCars.Columns["Price"].HeaderText = "Price(Rs)";

            dgvCars.Columns["Status"].Width = 100;

            dgvCars.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvCars.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        //car status add manully
        private void PopulateStatusComboBox()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Available");
            cmbStatus.Items.Add("Rented");
            cmbStatus.Items.Add("In Maintenance");
        }

        //validation method for car
        private bool CarCheck()
        {
            if (string.IsNullOrWhiteSpace(txtbrand.Text))
            {
                MessageBox.Show("Brand is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show("Model is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtRegNumber.Text))
            {
                MessageBox.Show("Registration number is required.");
                return false;
            }
            if (txtRegNumber.Text.Length < 5 || txtRegNumber.Text.Length > 10)
            {
                MessageBox.Show("Invalid registration number.");
                return false;
            }
            if (!decimal.TryParse(txtPrice.Text, out _))
            {
                MessageBox.Show("Please enter a valid numeric price.");
                return false;
            }
            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.");
                return false;
            }
            return true;
        }

        // load car models
        private void txtbrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtModel.Items.Clear();
            if (txtbrand.SelectedItem == null) return;

            string selectedBrand = txtbrand.SelectedItem.ToString();

            switch (selectedBrand)
            {
                case "Toyota":
                    txtModel.Items.AddRange(new[] { "Camry", "Corolla", "Prius", "Land Cruiser", "RAV4", "Hilux" });
                    break;
                case "Honda":
                    txtModel.Items.AddRange(new[] { "Civic", "Accord", "CR-V", "Pilot", "Odyssey" });
                    break;
                case "Ford":
                    txtModel.Items.AddRange(new[] { "Mustang", "F-150", "Explorer", "Escape", "Focus" });
                    break;
                case "Chevrolet":
                    txtModel.Items.AddRange(new[] { "Silverado", "Malibu", "Equinox", "Traverse", "Tahoe" });
                    break;
                case "BMW":
                    txtModel.Items.AddRange(new[] { "3 Series", "5 Series", "X5", "Z4", "M3" });
                    break;
                case "Mercedes":
                    txtModel.Items.AddRange(new[] { "C-Class", "E-Class", "S-Class", "GLE", "G-Class" });
                    break;
                case "Audi":
                    txtModel.Items.AddRange(new[] { "A3", "A4", "Q5", "Q7", "A6" });
                    break;
                case "Volkswagen":
                    txtModel.Items.AddRange(new[] { "Golf", "Passat", "Tiguan", "Jetta", "Arteon" });
                    break;
                case "Nissan":
                    txtModel.Items.AddRange(new[] { "Altima", "Maxima", "Rogue", "Frontier", "Pathfinder" });
                    break;
                case "Hyundai":
                    txtModel.Items.AddRange(new[] { "Elantra", "Sonata", "Tucson", "Santa Fe", "Kona" });
                    break;
                case "Porsche":
                    txtModel.Items.AddRange(new[] { "911", "Cayenne", "Macan", "Panamera" });
                    break;
                case "Jaguar":
                    txtModel.Items.AddRange(new[] { "XF", "F-Pace", "E-Pace", "I-Pace" });
                    break;
                case "Tesla":
                    txtModel.Items.AddRange(new[] { "Model S", "Model 3", "Model X", "Model Y" });
                    break;
                case "Mazda":
                    txtModel.Items.AddRange(new[] { "Mazda3", "Mazda6", "CX-5", "CX-9" });
                    break;
                case "Land Rover":
                    txtModel.Items.AddRange(new[] { "Range Rover", "Defender", "Discovery", "Evoque" });
                    break;
                case "Lexus":
                    txtModel.Items.AddRange(new[] { "RX", "NX", "ES", "GS" });
                    break;
            }
        }


        private void LoadSupplierNames()
        {
            string query = "SELECT SupplierID FROM Suppliers";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        SUPPLIERCOMBO.Items.Add(reader["SupplierID"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        //add car
        private void addbtn_Click(object sender, EventArgs e)
        {
            if (!CarCheck())
                return;

            if (string.IsNullOrEmpty(SUPPLIERCOMBO.SelectedItem?.ToString()))
            {
                MessageBox.Show("Please select a supplier.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string checkQuery = "SELECT COUNT(*) FROM Cars WHERE CarID = @CarID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@CarID", txtCarID.Text);

                    conn.Open();
                    int carCount = (int)checkCmd.ExecuteScalar();

                    if (carCount > 0)
                    {
                        MessageBox.Show("Car ID already exists. Please clear the fields and get a new ID.");
                        ClearAllFields();
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SET IDENTITY_INSERT Cars ON;\r\n\r\nINSERT INTO Cars (CarID, Brand, Model, RegistrationNumber, Price, Status, SupplierID)\r\nVALUES (@CarID, @Brand, @Model, @RegNo, @Price, @Status, @SupplierID);\r\n\r\nSET IDENTITY_INSERT Cars OFF;\r\n";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@CarID", txtCarID.Text);
                    cmd.Parameters.AddWithValue("@Brand", txtbrand.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Model", txtModel.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@RegNo", txtRegNumber.Text);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SupplierID", SUPPLIERCOMBO.SelectedItem.ToString());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Car added successfully.");
                ClearFields();
                LoadCars();
                LoadNextCarID();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding car: {ex.Message}");
            }
        }



        //update car
        private void updatebtn_Click(object sender, EventArgs e)
        {
            if (!CarCheck())
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string checkQuery = "SELECT COUNT(*) FROM Cars WHERE CarID = @CarID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@CarID", txtCarID.Text);

                    conn.Open();
                    int carCount = (int)checkCmd.ExecuteScalar();

                    if (carCount == 0)
                    {
                        MessageBox.Show("Car ID does not exist. Please check the Car ID.");
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Cars SET Brand = @Brand, Model = @Model, RegistrationNumber = @RegNo, " +
                                   "Price = @Price, Status = @Status, SupplierID = @SupplierID WHERE CarID = @CarID";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@CarID", txtCarID.Text);
                    cmd.Parameters.AddWithValue("@Brand", txtbrand.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Model", txtModel.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@RegNo", txtRegNumber.Text);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SupplierID", SUPPLIERCOMBO.SelectedItem.ToString());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Car details updated successfully.");
                ClearFields();
                LoadCars();
                LoadNextCarID();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car details: {ex.Message}");
            }
        }



        //delete car
        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (dgvCars.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a car to delete.");
                return;
            }

            try
            {
                int carID = Convert.ToInt32(dgvCars.SelectedRows[0].Cells["CarID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this car and its related maintenance records?",
                                                      "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string deleteMaintenanceQuery = "DELETE FROM Maintenance WHERE CarID = @CarID";
                        SqlCommand deleteMaintenanceCmd = new SqlCommand(deleteMaintenanceQuery, conn);
                        deleteMaintenanceCmd.Parameters.AddWithValue("@CarID", carID);
                        deleteMaintenanceCmd.ExecuteNonQuery(); 

                        string deleteCarQuery = "DELETE FROM Cars WHERE CarID = @CarID";
                        SqlCommand deleteCarCmd = new SqlCommand(deleteCarQuery, conn);
                        deleteCarCmd.Parameters.AddWithValue("@CarID", carID);
                        deleteCarCmd.ExecuteNonQuery(); 

                        MessageBox.Show("Car and related maintenance records deleted successfully.");
                        LoadCars(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting car: {ex.Message}");
            }
        }

        //click car data load 
        private void dgvCars_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCars.Rows[e.RowIndex];

                txtCarID.Text = row.Cells["CarID"].Value.ToString();
                txtbrand.Text = row.Cells["Brand"].Value.ToString();
                txtModel.Text = row.Cells["Model"].Value.ToString();
                txtRegNumber.Text = row.Cells["RegistrationNumber"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                cmbStatus.SelectedItem = row.Cells["Status"].Value.ToString();
                SUPPLIERCOMBO.SelectedItem = row.Cells["SupplierID"].Value.ToString();

            }
        }

        public int GetSupplierIDByName(string supplierName)
        {
            string query = "SELECT SupplierID FROM Suppliers WHERE Name = @SupplierName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SupplierName", supplierName);

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return -1; 
                }
            }
        }


        // clear car
        private void clearbtn_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        //clear car 
        private void ClearFields()
        {
            txtbrand.SelectedIndex=-1;
            txtModel.SelectedIndex = -1;
            txtRegNumber.Clear();
            txtPrice.Clear();
            cmbStatus.SelectedIndex = -1;
            LoadNextCarID();
        }



        // dashboard
        // dashboard

        //available car count
        private void LoadAvailableCarsCounts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Cars WHERE Status = 'Available'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    int availableCarsCount = (int)cmd.ExecuteScalar();

                    guna2TextBox12.Text = availableCarsCount.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //rented car count
        private void LoadRentedCarsCounts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Cars WHERE Status = 'Rented'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    int RentedCarsCount = (int)cmd.ExecuteScalar();

                    guna2TextBox5.Text = RentedCarsCount.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // maintenance car count
        private void LoadMaintenanceCarsCounts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Cars WHERE Status = 'In Maintenance'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    int InMaintenanceCarsCount = (int)cmd.ExecuteScalar();

                    guna2TextBox6.Text = InMaintenanceCarsCount.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //customer count
        private void LoadCustomerCount()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Customers";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int customerCount = (int)cmd.ExecuteScalar();

                    guna2TextBoxCustomerCount.Text = customerCount.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //rented car count
        private void LoadRentalCount()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Rental";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int rentalCount = (int)cmd.ExecuteScalar();

                    guna2TextBoxRentalCount.Text = rentalCount.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //sum of total payments 
        private void LoadTotalPayments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SUM(Amount) FROM Paymentss";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    decimal totalAmount = result != DBNull.Value ? (decimal)result : 0;

                    guna2TextBoxTotalPayments.Text = $"Rs.{totalAmount:N2}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //employee  count

        private void LoadEmployeeCount()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Employees";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    int employeeCount = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    guna2TextBoxEmployeeCount.Text = employeeCount.ToString("N0");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // customer
        // customer

        // load next customerId from table to textbox 
        private void LoadNextCustomerID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(CustomerID), 0) + 1 AS NextCustomerID FROM Customers";
                SqlCommand cmd = new SqlCommand(query, conn);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    txtCustomerID.Text = result.ToString();
                }
            }
        }

        //load in DGV
        private void LoadCustomers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Customers";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvCustomers.DataSource = dt;

                dgvcusdata();

            }
        }


        //customer datagridview data customize
        private void dgvcusdata()
        {
            dgvCustomers.Font = new Font("Microsoft Sans Serif", 09, FontStyle.Regular);

            dgvCustomers.Columns["CustomerID"].Width = 40;
            dgvCustomers.Columns["CustomerID"].HeaderText = "ID";

            dgvCustomers.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvCustomers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvCustomers.Columns["Name"].Width = 100;
            dgvCustomers.Columns["Phone"].Width = 80;
            dgvCustomers.Columns["License"].Width = 90;
            dgvCustomers.Columns["Email"].Width = 120;
        }


        // validate customer data
        private bool CustomerCheck()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.");
                return false;
            }

            if (!IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Phone number must be exactly 10 digits.");
                return false;
            }

            if (!IsPhoneUnique(txtPhone.Text))
            {
                MessageBox.Show("Phone number already exists. Please use a unique phone number.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLicense.Text))
            {
                MessageBox.Show("License is required.");
                return false;
            }

            if (!txtLicense.Text.All(char.IsLetterOrDigit))
            {
                MessageBox.Show("License should only contain letters and digits.");
                return false;
            }

            if (!IsLicenseUnique(txtLicense.Text))
            {
                MessageBox.Show("License already exists. Please use a unique license.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("A valid email is required.");
                return false;
            }

            if (!IsEmailUnique(txtEmail.Text))
            {
                MessageBox.Show("Email already exists. Please use a unique email.");
                return false;
            }

            return true;
        }

        // Check if phone number 
        private bool IsValidPhoneNumber(string phone)
        {
            return phone.Length == 10 && phone.All(char.IsDigit);
        }

        // Check if email regex pattern
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        // Check if phone number is unique
        private bool IsPhoneUnique(string phone)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Customers WHERE Phone = @Phone";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Phone", phone);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }

        // Check if license is unique 
        private bool IsLicenseUnique(string license)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Customers WHERE License = @License";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@License", license);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }

        // Check if email is unique in the database
        private bool IsEmailUnique(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Customers WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }


        // add customer
        private void addbtn1_Click(object sender, EventArgs e)
        {
            if (!CustomerCheck())
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Customers (Name, Phone, License, Email) VALUES (@Name, @Phone, @License, @Email)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@License", txtLicense.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Customer added successfully.");
                ClearCustomerFields();
                LoadCustomers();
                LoadNextCustomerID();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}");
            }
        }



        // update custmer
        private void updatebtn1_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            // Get the current values of the selected customer
            int customerID = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["CustomerID"].Value);
            string currentName = dgvCustomers.SelectedRows[0].Cells["Name"].Value.ToString();
            string currentPhone = dgvCustomers.SelectedRows[0].Cells["Phone"].Value.ToString();
            string currentLicense = dgvCustomers.SelectedRows[0].Cells["License"].Value.ToString();
            string currentEmail = dgvCustomers.SelectedRows[0].Cells["Email"].Value.ToString();

            // Ensure all fields are filled before updating
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace(txtLicense.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("All fields must be completed before updating.");
                return;
            }

            // Check if customer data is valid
            if (!CustomerCheck(currentName, currentPhone, currentLicense, currentEmail))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Customers SET 
                             Name = @Name, 
                             Phone = @Phone, 
                             License = @License, 
                             Email = @Email
                             WHERE CustomerID = @ID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", customerID);

                    // Update only the fields that have been modified
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@License", txtLicense.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Customer updated successfully.");
                ClearCustomerFields();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}");
            }
        }


        // Updated CustomerCheck method to check only modified fields
        private bool CustomerCheck(string currentName, string currentPhone, string currentLicense, string currentEmail)
        {
            // Check if Name is modified and validate
            if (!string.IsNullOrWhiteSpace(txtName.Text) && txtName.Text != currentName)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Name is required.");
                    return false;
                }
            }

            // Check if Phone is modified and validate
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && txtPhone.Text != currentPhone)
            {
                if (!IsValidPhoneNumber(txtPhone.Text))
                {
                    MessageBox.Show("Phone number must be exactly 10 digits.");
                    return false;
                }
                if (!IsPhoneUnique(txtPhone.Text))
                {
                    MessageBox.Show("Phone number already exists. Please use a unique phone number.");
                    return false;
                }
            }

            // Check if License is modified and validate
            if (!string.IsNullOrWhiteSpace(txtLicense.Text) && txtLicense.Text != currentLicense)
            {
                if (string.IsNullOrWhiteSpace(txtLicense.Text))
                {
                    MessageBox.Show("License is required.");
                    return false;
                }
                if (!txtLicense.Text.All(char.IsLetterOrDigit))
                {
                    MessageBox.Show("License should only contain letters and digits.");
                    return false;
                }
                if (!IsLicenseUnique(txtLicense.Text))
                {
                    MessageBox.Show("License already exists. Please use a unique license.");
                    return false;
                }
            }

            // Check if Email is modified and validate
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && txtEmail.Text != currentEmail)
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("A valid email is required.");
                    return false;
                }
                if (!IsEmailUnique(txtEmail.Text))
                {
                    MessageBox.Show("Email already exists. Please use a unique email.");
                    return false;
                }
            }

            return true;
        }


        //delete customer
        private void deletebtn1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerID.Text))
            {
                MessageBox.Show("No customer selected. Please select a customer to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this customer and their associated payments?",
                                                  "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string deletePaymentsQuery = "DELETE FROM Paymentss WHERE CustomerID = @CustomerID";
                        SqlCommand deletePaymentsCmd = new SqlCommand(deletePaymentsQuery, conn);
                        deletePaymentsCmd.Parameters.AddWithValue("@CustomerID", int.Parse(txtCustomerID.Text));
                        deletePaymentsCmd.ExecuteNonQuery(); 

                        string deleteCustomerQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                        SqlCommand deleteCustomerCmd = new SqlCommand(deleteCustomerQuery, conn);
                        deleteCustomerCmd.Parameters.AddWithValue("@CustomerID", int.Parse(txtCustomerID.Text));
                        int rowsAffected = deleteCustomerCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer and related payments deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Customer ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                LoadCustomers();        // Reload customer data
                ClearCustomerFields();  // Clear input fields
                LoadNextCustomerID();   // Load the next customer ID
                LoadCustomerCount();    // Update the customer count
            }
        }

        // click customer data load 
        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];

                txtCustomerID.Text = row.Cells["CustomerID"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtLicense.Text = row.Cells["License"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }
        }

        //clear customer data
        private void clearbtn1_Click(object sender, EventArgs e)
        {
            ClearCustomerFields1();
        }

        private void ClearCustomerFields1()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtLicense.Clear();
            txtEmail.Clear();
            LoadNextCustomerID();

        }


        //clear customer data
        private void ClearCustomerFields()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtLicense.Clear();
            txtEmail.Clear();
            LoadNextCustomerID();
        }


        // rental
        // rental

        // load next rentalId from table to textbox 
        private void LoadNextRentalID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(RentalID), 0) + 1 AS NextRentalID FROM Rental";
                SqlCommand cmd = new SqlCommand(query, conn);
                txtRentalID.Text = Convert.ToString(cmd.ExecuteScalar());
            }
        }

        // load car data to combo box
        private void LoadCarData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CarID, Model FROM Cars";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbCarID.DataSource = dt;
                    cmbCarID.DisplayMember = "CarID";
                    cmbCarID.ValueMember = "CarID";
                    cmbCarID.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading car data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // load customer data to combo box
        private void LoadCustomerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CustomerID, Name FROM Customers";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbCustomerID.DataSource = dt;
                    cmbCustomerID.DisplayMember = "CustomerID";
                    cmbCustomerID.ValueMember = "CustomerID";
                    cmbCustomerID.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customer data: {ex.Message}", "Database Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //  load model according to carID
        private void cmbCarID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCarID.SelectedValue != null && int.TryParse(cmbCarID.SelectedValue.ToString(), out int selectedCarID))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Model FROM Cars WHERE CarID = @CarID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CarID", selectedCarID);

                    object result = cmd.ExecuteScalar();
                    txtCarModel.Text = result?.ToString() ?? string.Empty;
                }
            }
        }

        //  load name according to customerID
        private void cmbCustomerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerID.SelectedValue != null && int.TryParse(cmbCustomerID.SelectedValue.ToString(), out int selectedCustomerID))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Name FROM Customers WHERE CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", selectedCustomerID);

                    object result = cmd.ExecuteScalar();
                    txtCustomerName.Text = result?.ToString() ?? string.Empty;
                }
            }
        }

        //load rentals into DGV
        private void LoadRentalsData()
        {
            string query = "SELECT RentalID, CustomerID, CarID, StartDate, EndDate, TotalCost FROM Rental";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                    DataTable dataTable = new DataTable();

                    dataAdapter.Fill(dataTable);

                    dgvRentals.DataSource = dataTable;
                    dgvrentdata();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        //rent datagridview data customize
        private void dgvrentdata()
        {
            dgvRentals.Font = new Font("Microsoft Sans Serif", 09, FontStyle.Regular);

            dgvRentals.Columns["RentalID"].Width = 50;
            dgvRentals.Columns["RentalID"].HeaderText = "ID";

            dgvRentals.Columns["CustomerID"].Width = 100;
            dgvRentals.Columns["CustomerID"].HeaderText = "Cus ID";

            dgvRentals.Columns["CarID"].Width = 100;
            dgvRentals.Columns["CarID"].HeaderText = "Car ID";

            dgvRentals.Columns["StartDate"].Width = 120;
            dgvRentals.Columns["StartDate"].HeaderText = "Start Date";

            dgvRentals.Columns["EndDate"].Width = 120;
            dgvRentals.Columns["EndDate"].HeaderText = "End Date";

            dgvRentals.Columns["TotalCost"].Width = 90;
            dgvRentals.Columns["TotalCost"].HeaderText = "Total Cost(Rs)";

            dgvRentals.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvRentals.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        // validate rentals
        private bool RentalCheck()
        {
            if (cmbCarID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a car.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cmbCustomerID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(txtTotalCost.Text, out _))
            {
                MessageBox.Show("Invalid total cost. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtpStartDate.Value > dtpEndDate.Value)
            {
                MessageBox.Show("Start date cannot be later than end date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        //add rentals
        private void addbtn2_Click(object sender, EventArgs e)
        {
            if (!RentalCheck())
                return;
            int rentalID = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["RentalID"].Value);
            int carID = Convert.ToInt32(cmbCarID.SelectedValue);
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date;

            if (IsCarRentedOnSameDay1( startDate, endDate, rentalID))
            {
                MessageBox.Show("This car is already rented during the selected period. Please choose a different date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Rental (CarID, CarModel, CustomerID, CustomerName, StartDate, EndDate, TotalCost) 
                             VALUES (@CarID, @CarModel, @CustomerID, @CustomerName, @StartDate, @EndDate, @TotalCost)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@CarID", cmbCarID.SelectedValue);
                    cmd.Parameters.AddWithValue("@CarModel", txtCarModel.Text);
                    cmd.Parameters.AddWithValue("@CustomerID", cmbCustomerID.SelectedValue);
                    cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                    cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                    cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDecimal(txtTotalCost.Text));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Rental added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRentalsData();
                ClearRentalFields();
                LoadNextRentalID();
                LoadRentalCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding rental: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //update rentals
        private void updatebtn2_Click(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a rental to update.");
                return;
            }

            if (!RentalCheck())
                return;

            try
            {
                // Get rental ID of the selected rental
                int rentalID = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["RentalID"].Value);
                int carID = Convert.ToInt32(cmbCarID.SelectedValue);
                DateTime startDate = dtpStartDate.Value.Date;
                DateTime endDate = dtpEndDate.Value.Date;  

                if (IsCarRentedOnSameDay(carID, startDate, endDate, rentalID))
                {
                    MessageBox.Show("This car is already rented during the selected period. Please choose a different date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Proceed with updating the rental details if no conflict
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Rental 
                             SET CustomerID = @CustomerID, CustomerName = @CustomerName, 
                                 CarID = @CarID, CarModel = @CarModel, 
                                 StartDate = @StartDate, EndDate = @EndDate, 
                                 TotalCost = @TotalCost 
                             WHERE RentalID = @RentalID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@RentalID", rentalID);
                    cmd.Parameters.AddWithValue("@CarID", cmbCarID.SelectedValue);
                    cmd.Parameters.AddWithValue("@CarModel", txtCarModel.Text.Trim());
                    cmd.Parameters.AddWithValue("@CustomerID", cmbCustomerID.SelectedValue);
                    cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                    cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                    cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDecimal(txtTotalCost.Text.Trim()));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Rental updated successfully.");
                ClearRentalFields();
                LoadRentalsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating rental: {ex.Message}");
            }
        }


        //check whether the car is rented already 
        public bool IsCarRentedOnSameDay(int carID, DateTime startDate, DateTime endDate, int rentalID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT COUNT(*)
            FROM Rental
            WHERE CarID = @CarID
            AND (
                (StartDate <= @EndDate AND EndDate >= @StartDate) -- Check for overlap between the new and existing rental periods
            )
            AND RentalID != @RentalID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CarID", carID);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cmd.Parameters.AddWithValue("@RentalID", rentalID); 

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking rental availability: {ex.Message}");
                return false;
            }
        }

        public bool IsCarRentedOnSameDay1( DateTime startDate, DateTime endDate, int rentalID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT COUNT(*)
                FROM Rental
                WHERE 
                    (StartDate <= @EndDate AND EndDate >= @StartDate) -- Check for overlap between the new and existing rental periods
                    AND RentalID != @RentalID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cmd.Parameters.AddWithValue("@RentalID", rentalID);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking rental availability: {ex.Message}");
                return false;
            }
        }
        //delete rentals
        private void deletebtn2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRentalID.Text))
            {
                MessageBox.Show("No rental selected. Please select a rental to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this rental and related data?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string deletePaymentsQuery = "DELETE FROM Paymentss WHERE RentalID = @RentalID";
                        SqlCommand deletePaymentsCmd = new SqlCommand(deletePaymentsQuery, conn);
                        deletePaymentsCmd.Parameters.AddWithValue("@RentalID", int.Parse(txtRentalID.Text));
                        deletePaymentsCmd.ExecuteNonQuery(); 

                        string deleteRentalQuery = "DELETE FROM Rental WHERE RentalID = @RentalID";
                        SqlCommand deleteRentalCmd = new SqlCommand(deleteRentalQuery, conn);
                        deleteRentalCmd.Parameters.AddWithValue("@RentalID", int.Parse(txtRentalID.Text));
                        int rowsAffected = deleteRentalCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rental and related data deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Rental ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                LoadRentalsData();   
                ClearRentalFields();   
                LoadNextRentalID();   
                LoadRentalCount();  
            }
        }

        //click rentals data load
        private void dgvRentals_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvRentals.Rows[e.RowIndex];

                txtRentalID.Text = row.Cells["RentalID"].Value.ToString();
                cmbCustomerID.SelectedValue = row.Cells["CustomerID"].Value.ToString();
                cmbCarID.SelectedValue = row.Cells["CarID"].Value.ToString();
                dtpStartDate.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
                dtpEndDate.Value = Convert.ToDateTime(row.Cells["EndDate"].Value);
                txtTotalCost.Text = row.Cells["TotalCost"].Value.ToString();
            }
        }


        // clear rental data
        private void ClearRentalFields()
        {
            txtRentalID.Clear();
            txtCarModel.Clear();
            cmbCarID.SelectedItem = null;
            txtCarModel.Text = null;
            txtCustomerName.Clear();
            txtTotalCost.Clear();
            cmbCarID.SelectedIndex = -1;
            cmbCustomerID.SelectedIndex = -1;
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now;
        }

        // print rental data

        private void printbtn_Click(object sender, EventArgs e)
        {
            rentreport  mainForm = new rentreport();
            mainForm.Show();
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }


        // payments
        // payments

        // load next paymentId in textbox
        private void LoadNextPaymentID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(PaymentID), 0) + 1 AS NextPaymentID FROM Paymentss";
                SqlCommand cmd = new SqlCommand(query, conn);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    txtPaymentID.Text = result.ToString();
                }
            }
        }

        // load rentalIds in combo box
        private void LoadRentalIDs()
        {
            cmbRentalID.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT RentalID FROM Rental";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbRentalID.Items.Add(reader["RentalID"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // get the customer name from rentals 
        private void cmbRentalID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRentalID.SelectedItem != null)
            {
                int rentalID = int.Parse(cmbRentalID.SelectedItem.ToString());
                GetCustomerNameByRentalID(rentalID);
            }
        }

        // get the customer name from rentals 
        private void GetCustomerNameByRentalID(int rentalID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CustomerName FROM Rental WHERE RentalID = @RentalID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RentalID", rentalID);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        txtCustomerName1.Text = result.ToString();
                    }
                    else
                    {
                        txtCustomerName1.Text = "Customer Not Found";
                    }
                }
            }
        }

        // load payment in DGV
        private void LoadPaymentData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                     SELECT 
                     p.PaymentID, 
                     p.RentalID, 
                     r.CustomerName,  
                     p.Amount, 
                     p.PaymentDate, 
                     p.PaymentMethod 
                     FROM 
                     Paymentss p
                     INNER JOIN 
                     Rental r ON p.RentalID = r.RentalID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvPayments.DataSource = dt;
                    dgvamountdata();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // amount datagridview data customize
        private void dgvamountdata()
        {
            dgvPayments.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);

            dgvPayments.Columns["PaymentID"].Width = 40;
            dgvPayments.Columns["PaymentID"].HeaderText = "ID";

            dgvPayments.Columns["RentalID"].Width = 70;
            dgvPayments.Columns["RentalID"].HeaderText = "Rent ID";

            dgvPayments.Columns["CustomerName"].Width = 120;
            dgvPayments.Columns["CustomerName"].HeaderText = "Customer Name";

            dgvPayments.Columns["Amount"].Width = 100;
            dgvPayments.Columns["Amount"].HeaderText = "Amount(Rs)";


            dgvPayments.Columns["PaymentDate"].Width = 120;
            dgvPayments.Columns["PaymentDate"].HeaderText = "Payment Date";

            dgvPayments.Columns["PaymentMethod"].Width = 90;
            dgvPayments.Columns["PaymentMethod"].HeaderText = "Method";

            dgvPayments.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvPayments.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        //validate amount
        private bool ValidateAmount()
        {
            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Amount cannot be empty.");
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Please enter a valid decimal amount.");
                return false;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Amount must be a positive number.");
                return false;
            }

            return true;
        }

        //add amount

        private void addbtn3_Click(object sender, EventArgs e)
        {
            if (!ValidateAmount())
            {
                return;
            }

            // Validate Date
            if (!ValidateDate(dtpPaymentDate.Value))
            {
                return;
            }

            int rentalID = int.Parse(cmbRentalID.SelectedItem.ToString());

            // Check if a payment already exists for the selected RentalID
            if (PaymentExists(rentalID))
            {
                MessageBox.Show("This rental already has a payment.");
                return;
            }

            string customerName = txtCustomerName1.Text;
            decimal amount = decimal.Parse(txtAmount.Text);
            DateTime paymentDate = dtpPaymentDate.Value;
            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Paymentss (RentalID, CustomerName, Amount, PaymentDate, PaymentMethod) " +
                               "VALUES (@RentalID, @CustomerName, @Amount, @PaymentDate, @PaymentMethod)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RentalID", rentalID);
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Payment added successfully.");
            LoadPaymentData();
            LoadNextPaymentID();
            ClearFieldss();
        }

        private void updatebtn3_Click(object sender, EventArgs e)
        {
            if (dgvPayments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a payment to update.");
                return;
            }

          
            int paymentID = Convert.ToInt32(dgvPayments.SelectedRows[0].Cells["PaymentID"].Value);
            if (cmbRentalID.SelectedIndex==-1)
            {
                MessageBox.Show("Please select a Rent Id.");
                return;
            }
            int rentalID = int.Parse(cmbRentalID.SelectedItem.ToString());
          
            // Validate Date
            if (!ValidateDate(dtpPaymentDate.Value))
            {
                return;
            }

            // Check if the rental has more than one payment
            if (MoreThanOnePaymentExists(rentalID, paymentID))
            {
                MessageBox.Show("This rental already has a payment. You cannot add or update another payment.");
                return;
            }

            if (!ValidateAmount())
            {
                return;
            }

            string customerName = txtCustomerName1.Text;
            decimal amount = decimal.Parse(txtAmount.Text);
            DateTime paymentDate = dtpPaymentDate.Value;
            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Paymentss SET RentalID = @RentalID, CustomerName = @CustomerName, " +
                               "Amount = @Amount, PaymentDate = @PaymentDate, PaymentMethod = @PaymentMethod " +
                               "WHERE PaymentID = @PaymentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaymentID", paymentID);
                    cmd.Parameters.AddWithValue("@RentalID", rentalID);
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Payment updated successfully.");
            LoadPaymentData();
            LoadNextPaymentID();
            ClearFieldss();
        }

        // Helper method to validate the date
        private bool ValidateDate(DateTime paymentDate)
        {
            DateTime currentDate = DateTime.Now;

            if (paymentDate > currentDate)
            {
                MessageBox.Show("Payment date cannot be in the future.");
                return false;
            }

            if (paymentDate < new DateTime(2000, 1, 1)) 
            {
                MessageBox.Show("Payment date cannot be too far in the past.");
                return false;
            }

            return true;
        }

        //payment exists for the RentalID
        private bool PaymentExists(int rentalID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Paymentss WHERE RentalID = @RentalID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RentalID", rentalID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        //payment exists for the RentalID (excluding the current PaymentID)
        private bool MoreThanOnePaymentExists(int rentalID, int paymentID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Paymentss WHERE RentalID = @RentalID AND PaymentID != @PaymentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RentalID", rentalID);
                    cmd.Parameters.AddWithValue("@PaymentID", paymentID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        //delete amount
        private void deletebtn3_Click(object sender, EventArgs e)
        {

            if (dgvPayments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a payment to delete.");
                return;
            }

            int paymentID = Convert.ToInt32(dgvPayments.SelectedRows[0].Cells["PaymentID"].Value);

            if (!ValidateAmount())
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Paymentss WHERE PaymentID = @PaymentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaymentID", paymentID);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Payment deleted successfully.");
            LoadPaymentData();
            LoadNextPaymentID();
            ClearFieldss();
        }

        // combo box for payment methods
        private void LoadPaymentMethods()
        {
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.Add("Credit Card");
            cmbPaymentMethod.Items.Add("Debit Card");
            cmbPaymentMethod.Items.Add("Cash");
        }

        // click amount data load 
        private void dgvPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPayments.Rows[e.RowIndex];

                txtPaymentID.Text = row.Cells["PaymentID"].Value.ToString();
                cmbRentalID.SelectedItem = row.Cells["RentalID"].Value.ToString();
                txtCustomerName.Text = row.Cells["CustomerName"].Value.ToString();
                txtAmount.Text = row.Cells["Amount"].Value.ToString();
                dtpPaymentDate.Value = Convert.ToDateTime(row.Cells["PaymentDate"].Value);
                cmbPaymentMethod.SelectedItem = row.Cells["PaymentMethod"].Value.ToString();
            }
        }

        // clear amount data
        private void ClearFieldss()
        {
            txtAmount.Clear();
            txtCustomerName1.Clear();
            cmbRentalID.SelectedIndex = -1;
            cmbPaymentMethod.SelectedIndex = -1;
            dtpPaymentDate.Value = DateTime.Today;
        }


        //print report
        private void printbtn2_Click(object sender, EventArgs e)
        {
            Payreport mainForm = new Payreport();
            mainForm.Show();
        }


        // maintenance
        // maintenance

        // load next maintenanceId in textbox
        private void LoadNextMaintenanceID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(MaintenanceID), 0) + 1 FROM Maintenance";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    txtMaintenanceID.Text = cmd.ExecuteScalar()?.ToString() ?? "1";
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error loading next Maintenance ID: {ex.Message}");
                }
            }
        }

        // load maintenance data in DGV
        private void LoadMaintenanceRecords()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Maintenance";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvMaintenance.DataSource = dt;
                    dgvmaintenancedata();

                    if (dt.Rows.Count == 0)
                    {
                        //MessageBox.Show("No maintenance records found.");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An error occurred while loading the records: {ex.Message}");
                }
            }
        }

        // maintenance datagridview data customize
       private void dgvmaintenancedata()
        {
            dgvMaintenance.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);

            dgvMaintenance.Columns["CarModel"].HeaderText = "Car Name";

            dgvMaintenance.Columns["CarId"].HeaderText = "Car Id"; 
            dgvMaintenance.Columns["CarId"].Width = 40;

            dgvMaintenance.Columns["MaintenanceID"].Width = 40;
            dgvMaintenance.Columns["MaintenanceID"].HeaderText = "ID";

            dgvMaintenance.Columns["CarModel"].Width = 130;

            dgvMaintenance.Columns["MaintenanceDate"].Width = 100;
            dgvMaintenance.Columns["MaintenanceDate"].HeaderText = "Maintenance Date";

            dgvMaintenance.Columns["Remarks"].Width = 100;

            dgvMaintenance.Columns["Status"].Width = 90;
            dgvMaintenance.Columns["SupplierID"].Width = 40;
            dgvMaintenance.Columns["SupplierID"].HeaderText = "Sup ID";


            dgvMaintenance.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMaintenance.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
  

        // load maintenance method in combo box
        private void LoadMaintenanceMethods()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.Add("Scheduled");
            cboStatus.Items.Add("Completed");
            cboStatus.Items.Add("Ongoing");
        }


        //search cars for maintenance
        private void searchbtn1_Click(object sender, EventArgs e)
        {
            string carId = txtSearchCarID.Text.Trim();
            if (string.IsNullOrWhiteSpace(carId))
            {
                MessageBox.Show("Please enter a Car ID to search.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CarID, Model , Brand FROM Cars WHERE CarID = @CarID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CarID", carId);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtCarID1.Text = reader["CarID"].ToString();
                            txtCarModel1.Text = reader["Brand"].ToString() + " " + reader["Model"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No records found for the given Car ID.");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An error occurred while searching: {ex.Message}");
                }
            }
        }

        // Validate maintenance 
        private bool ValidateMaintenance()
        {
            if (string.IsNullOrWhiteSpace(txtCarID1.Text))
            {
                MessageBox.Show("Car ID cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCarModel1.Text))
            {
                MessageBox.Show("Car Model cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRemarks.Text))
            {
                MessageBox.Show("Remarks cannot be empty.");
                return false;
            }

            if (cboStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.");
                return false;
            }

            if (dtpMaintenanceDate.Value.Date <= DateTime.Now.Date)
            {
                MessageBox.Show("Please select a maintenance date that is in the future.");
                return false;
            }

            return true;
        }

        //main sup load

        private void LoadmainSup()
        {
            string query = "SELECT [SupplierID] FROM [rentals].[dbo].[Suppliers] WHERE [ServiceProvided] = 'Maintenance Services'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mainsup.Items.Add(reader["SupplierID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        // add maintenance 
        private void addbtn4_Click(object sender, EventArgs e)
        {
            if (!ValidateMaintenance())
            {
                return; 
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                  INSERT INTO Maintenance (CarID, CarModel, MaintenanceDate, Remarks, Status, SupplierID) 
                  VALUES (@CarID, @CarModel, @MaintenanceDate, @Remarks, @Status, @SupplierID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CarID", txtCarID1.Text.Trim());
                cmd.Parameters.AddWithValue("@CarModel", txtCarModel1.Text.Trim());
                cmd.Parameters.AddWithValue("@MaintenanceDate", dtpMaintenanceDate.Value);
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@Status", cboStatus.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@SupplierID", mainsup.SelectedItem?.ToString());


                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Maintenance record added successfully.");
                    ClearAllFields();
                    LoadMaintenanceRecords();
                    mainsup.Items.Clear();

                    LoadmainSup();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An error occurred while adding the maintenance record: {ex.Message}");
                }
            }
        }

        // update maintenance
        private void updatebtn4_Click(object sender, EventArgs e)
        {
            if (dgvMaintenance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a payment to update.");
                return;
            }
            int MaintenanceID = Convert.ToInt32(dgvMaintenance.SelectedRows[0].Cells["MaintenanceID"].Value);

            if (!ValidateMaintenance())
            {
                return;
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                 UPDATE Maintenance 
                 SET CarID = @CarID, CarModel = @CarModel, 
                 MaintenanceDate = @MaintenanceDate, Remarks = @Remarks, SupplierID=@SupplierID,
                 Status = @Status 
                 WHERE MaintenanceID = @MaintenanceID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaintenanceID", txtMaintenanceID.Text);
                cmd.Parameters.AddWithValue("@CarID", txtCarID1.Text.Trim());
                cmd.Parameters.AddWithValue("@CarModel", txtCarModel1.Text.Trim());
                cmd.Parameters.AddWithValue("@MaintenanceDate", dtpMaintenanceDate.Value);
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@Status", cboStatus.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@SupplierID", mainsup.SelectedItem?.ToString());


                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Maintenance record updated successfully.");
                    ClearAllFields();
                    LoadMaintenanceRecords();
                    mainsup.Items.Clear();
                   LoadmainSup();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An error occurred while updating the maintenance record: {ex.Message}");
                }
            }
        }

        // clear maintenance data
        private void ClearAllFields()
        {
            txtCarID.Clear();
            txtCarModel.Clear();
            txtSearchCarID.Clear();
            txtRemarks.Clear();
            cmbStatus.SelectedIndex = -1;
            mainsup.SelectedIndex = -1;

            dtpMaintenanceDate.Value = DateTime.Now;
        }

        // click maintenance data load 

        private void dgvMaintenance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMaintenance.Rows[e.RowIndex];
                txtMaintenanceID.Text = row.Cells["MaintenanceID"].Value.ToString();
                txtCarID1.Text = row.Cells["CarID"].Value.ToString();
                txtCarModel1.Text = row.Cells["CarModel"].Value.ToString();
                dtpMaintenanceDate.Value = Convert.ToDateTime(row.Cells["MaintenanceDate"].Value);
                txtRemarks.Text = row.Cells["Remarks"].Value.ToString();
                cboStatus.SelectedItem = row.Cells["Status"].Value.ToString();
                mainsup.SelectedItem = row.Cells["SupplierID"].Value.ToString();


            }
        }

        // employees 
        // employees 

        // load next EmployeeId in text box
        private void LoadNextEmployeeID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ISNULL(MAX(EmployeeID), 0) + 1 FROM Employees";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int nextEmployeeID = (int)cmd.ExecuteScalar();
                txtEmployeeID.Text = nextEmployeeID.ToString(); 
            }
        }

        // load employee data in DGV
        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvEmployees.DataSource = dt;
                dgvemployeedata();
            }
        }

        // employee datagridview data customize
        private void dgvemployeedata()
        {
            dgvEmployees.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);

            dgvEmployees.Columns["EmployeeID"].Width = 40;
            dgvEmployees.Columns["EmployeeID"].HeaderText = "ID";

            dgvEmployees.Columns["Name"].Width = 80;

            dgvEmployees.Columns["Position"].Width = 110;

            dgvEmployees.Columns["Phone"].Width = 90;

            dgvEmployees.Columns["Email"].Width = 120;

            dgvEmployees.Columns["HireDate"].Width = 80;
            dgvEmployees.Columns["HireDate"].HeaderText = "Hire Date";

            dgvEmployees.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvEmployees.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        // phone number is unique for add
        private bool IsPhoneUnique1(string phone)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Employees WHERE Phone = @Phone";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Phone", phone);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }

        // phone number is unique for update
        private bool IsPhoneUnique(string phone, int employeeID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Employees WHERE Phone = @Phone AND EmployeeID != @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }

        // email is unique for add
        private bool IsEmailUnique1(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Employees WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }

        // email is unique for update
        private bool IsEmailUnique(string email, int employeeID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Employees WHERE Email = @Email AND EmployeeID != @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }



        // load employee position in combo box
        private void PopulatePositionComboBox()
        {
            cmbPosition.Items.Clear();
            cmbPosition.Items.Add("Manager");
            cmbPosition.Items.Add("Sales Executive");
            cmbPosition.Items.Add("Designer");
            cmbPosition.Items.Add("Technician");
        }

        // validate employee 
        private bool ValidateEmployee()
        {
            if (string.IsNullOrWhiteSpace(txtEName.Text))
            {
                MessageBox.Show("Please enter a valid name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cmbPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a valid position.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEPhone.Text) || txtEPhone.Text.Length != 10)
            {
                MessageBox.Show("Please enter a valid phone number (10 digits).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

          
            DateTime hireDate;
            if (!DateTime.TryParse(txtHireDate.Text, out hireDate))
            {
                MessageBox.Show("Please enter a valid hire date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        // Add Employee
        private void addbtn5_Click(object sender, EventArgs e)
        {
            if (!ValidateEmployee())
            {
                return;
            }

            string phone = txtEPhone.Text;
            string email = txtEEmail.Text;

            if (!IsPhoneUnique1(phone))
            {
                MessageBox.Show("The phone number is already used by another employee.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsEmailUnique1(email))
            {
                MessageBox.Show("The email address is already used by another employee.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employees (Name, Position, Phone, Email, HireDate) " +
                               "VALUES (@Name, @Position, @Phone, @Email, @HireDate)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtEName.Text);
                cmd.Parameters.AddWithValue("@Position", cmbPosition.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);

                DateTime hireDate;
                if (!DateTime.TryParse(txtHireDate.Text, out hireDate))
                {
                    MessageBox.Show("Please enter a valid hire date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cmd.Parameters.AddWithValue("@HireDate", hireDate);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadEmployees();
            ClearField();
            LoadNextEmployeeID();
            LoadEmployeeCount();

            MessageBox.Show("Employee added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Update Employee
        private void updatebtn5_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                if (!ValidateEmployee())
                {
                    return;
                }

                int employeeID = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["EmployeeID"].Value);
                string phone = txtEPhone.Text;
                string email = txtEEmail.Text;

                if (!IsPhoneUnique(phone, employeeID))
                {
                    MessageBox.Show("The phone number is already used by another employee.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!IsEmailUnique(email, employeeID))
                {
                    MessageBox.Show("The email address is already used by another employee.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Employees SET Name = @Name, Position = @Position, " +
                                   "Phone = @Phone, Email = @Email, HireDate = @HireDate WHERE EmployeeID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", employeeID);
                    cmd.Parameters.AddWithValue("@Name", txtEName.Text);
                    cmd.Parameters.AddWithValue("@Position", cmbPosition.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);

                    DateTime hireDate;
                    if (!DateTime.TryParse(txtHireDate.Text, out hireDate))
                    {
                        MessageBox.Show("Please enter a valid hire date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd.Parameters.AddWithValue("@HireDate", hireDate);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadEmployees();
                ClearField();
                LoadNextEmployeeID();
                LoadEmployeeCount();

                MessageBox.Show("Employee details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select an employee to update.");
            }
        }


        // delete employee
        private void deletebtn5_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete this employee?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int employeeID = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["EmployeeID"].Value);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Employees WHERE EmployeeID = @ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", employeeID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    LoadEmployees();
                    ClearField();
                    LoadNextEmployeeID();
                    LoadEmployeeCount();

                    MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.");
            }
        }

        // click employee data load
        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];

                    txtEmployeeID.Text = row.Cells["EmployeeID"].Value.ToString();
                    txtEName.Text = row.Cells["Name"].Value.ToString();
                    cmbPosition.SelectedItem = row.Cells["Position"].Value.ToString();
                    txtEPhone.Text = row.Cells["Phone"].Value.ToString();
                    txtEEmail.Text = row.Cells["Email"].Value.ToString();
                    txtHireDate.Text = Convert.ToDateTime(row.Cells["HireDate"].Value).ToString("yyyy-MM-dd");
                }
            }
        }

        // clear employee data
        private void clrbtnemp_Click(object sender, EventArgs e)
        {
            ClearField();
        }

        // clear employee data
        private void ClearField()
        {
            txtEName.Clear();
            cmbPosition.SelectedIndex = -1;
            txtEPhone.Clear();
            txtEEmail.Clear();
            txtHireDate.Value = DateTime.Now;
        }


        // supplier
        // supplier

        // load next supplierId in combo box
        private void LoadNextSupplierID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ISNULL(MAX(SupplierID), 0) + 1 FROM Suppliers";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int nextSupplierID = (int)cmd.ExecuteScalar();
                txtSupplierID.Text = nextSupplierID.ToString();
            }
        }

        // load suppliers to DGV
        private void LoadSuppliers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Suppliers";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSuppliers.DataSource = dt;
                dgvsuppliersdata();
            }
        }

        // suppliers datagridview data customize
        private void dgvsuppliersdata()
        {
            dgvSuppliers.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);

            dgvSuppliers.Columns["SupplierID"].Width = 40;
            dgvSuppliers.Columns["SupplierID"].HeaderText = "ID";

            dgvSuppliers.Columns["Name"].Width = 80;

            dgvSuppliers.Columns["Contact"].Width = 80;
            dgvSuppliers.Columns["Contact"].HeaderText = "Phone";

            dgvSuppliers.Columns["Email"].Width = 110;

            dgvSuppliers.Columns["ServiceProvided"].Width = 130;
            dgvSuppliers.Columns["ServiceProvided"].HeaderText = "Service Provided";

            dgvSuppliers.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvSuppliers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        // check supplier phone number
        private bool IsPhoneUniques(string phone)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Suppliers WHERE Contact = @Phone";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Phone", phone);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }

        // check supplier email
        private bool IsEmailUniques(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Suppliers WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }


        // load service provided by supplier in combo box
        private void PopulateServiceProvidedComboBox()
        {
            cmbServiceProvided.Items.Clear();
            cmbServiceProvided.Items.Add("Car Provider");
            cmbServiceProvided.Items.Add("Maintenance Services");
        }

        private bool ValidateSupplierData()
        {
            if (string.IsNullOrWhiteSpace(txtSName.Text))
            {
                MessageBox.Show("Please enter a valid name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSContact.Text) || txtSContact.Text.Length != 10)
            {
                MessageBox.Show("Please enter a valid 10-digit contact number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!IsPhoneUniques(txtSContact.Text))
            {
                MessageBox.Show("This contact number is already taken.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!IsEmailUniques(txtSEmail.Text))
            {
                MessageBox.Show("This email address is already taken.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (cmbServiceProvided.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a valid service provided.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Add Supplier
        private bool IsCarAvailableForDate(int carID, DateTime bookingDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Bookings WHERE CarID = @CarID AND BookingDate = @BookingDate";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CarID", carID);
                cmd.Parameters.AddWithValue("@BookingDate", bookingDate);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0; 
            }
        }

        private void addbtn6_Click(object sender, EventArgs e)
        {
            if (!ValidateSupplierData())
                return;

            string contact = txtSContact.Text;
            string email = txtSEmail.Text;

            if (!IsPhoneUniqueForSupplier(contact))
            {
                MessageBox.Show("The phone number is already used by another supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsEmailUniqueForSupplier(email))
            {
                MessageBox.Show("The email address is already used by another supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int carID = Convert.ToInt32(cmbCarID.SelectedItem); 
            DateTime bookingDate = dtpStartDate.Value; 

            if (!IsCarAvailableForDate(carID, bookingDate))
            {
                MessageBox.Show("The selected car is already booked on the chosen date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Suppliers (Name, Contact, Email, ServiceProvided) " +
                               "VALUES (@Name, @Contact, @Email, @ServiceProvided)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtSName.Text);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ServiceProvided", cmbServiceProvided.SelectedItem.ToString());

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadSuppliers();
            ClearFields();
            LoadNextSupplierID();
            mainsup.Items.Clear();

            LoadmainSup();

            MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Update Supplier
        private bool IsPhoneUniqueForSupplier1(string phone, int supplierID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Suppliers WHERE Contact = @Phone AND SupplierID != @SupplierID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0; 
            }
        }

        private bool IsEmailUniqueForSupplier1(string email, int supplierID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Suppliers WHERE Email = @Email AND SupplierID != @SupplierID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0; 
            }
        }

        // update validation
        private bool ValidateSupplierData1()
        {
            if (string.IsNullOrWhiteSpace(txtSName.Text))
            {
                MessageBox.Show("Please enter a valid name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSContact.Text) || txtSContact.Text.Length != 10)
            {
                MessageBox.Show("Please enter a valid 10-digit contact number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
     
            if (string.IsNullOrWhiteSpace(txtSEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
     
            if (cmbServiceProvided.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a valid service provided.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void updatebtn6_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count > 0)
            {
                if (!ValidateSupplierData1()) return;

                int supplierID = Convert.ToInt32(dgvSuppliers.SelectedRows[0].Cells["SupplierID"].Value);
                string contact = txtSContact.Text;
                string email = txtSEmail.Text;

                if (!IsPhoneUniqueForSupplier1(contact, supplierID))
                {
                    MessageBox.Show("The phone number is already used by another supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!IsEmailUniqueForSupplier1(email, supplierID))
                {
                    MessageBox.Show("The email address is already used by another supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Suppliers SET Name = @Name, Contact = @Contact, " +
                                   "Email = @Email, ServiceProvided = @ServiceProvided WHERE SupplierID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", supplierID);
                    cmd.Parameters.AddWithValue("@Name", txtSName.Text);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@ServiceProvided", cmbServiceProvided.SelectedItem.ToString());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadSuppliers();
                ClearFields();
                LoadNextSupplierID();
                mainsup.Items.Clear();

                LoadmainSup();

                MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a supplier to update.");
            }
        }

        //phone number is unique for suppliers 
        private bool IsPhoneUniqueForSupplier(string phone)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Suppliers WHERE Contact = @Phone";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Phone", phone);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0; 
            }
        }

        //phone number is unique excluding the current supplier 

        private bool IsEmailUniqueForSupplier(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Suppliers WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }


        // delete supplier
        private void deletebtn6_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this supplier?",
                                                      "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int supplierID = Convert.ToInt32(dgvSuppliers.SelectedRows[0].Cells["SupplierID"].Value);

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string deleteCarsQuery = "DELETE FROM Cars WHERE SupplierID = @SupplierID";
                            SqlCommand deleteCarsCmd = new SqlCommand(deleteCarsQuery, conn);
                            deleteCarsCmd.Parameters.AddWithValue("@SupplierID", supplierID);
                            deleteCarsCmd.ExecuteNonQuery(); 

                            string deleteSupplierQuery = "DELETE FROM Suppliers WHERE SupplierID = @ID";
                            SqlCommand deleteSupplierCmd = new SqlCommand(deleteSupplierQuery, conn);
                            deleteSupplierCmd.Parameters.AddWithValue("@ID", supplierID);
                            deleteSupplierCmd.ExecuteNonQuery(); 

                            MessageBox.Show("Supplier deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadSuppliers(); 
                            ClearF(); 
                            LoadNextSupplierID();
                            mainsup.Items.Clear();

                            LoadmainSup();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a supplier to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // click supplier data load
        private void dgvSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvSuppliers.Rows[e.RowIndex];

                    txtSupplierID.Text = row.Cells["SupplierID"].Value.ToString();
                    txtSName.Text = row.Cells["Name"].Value.ToString();
                    txtSContact.Text = row.Cells["Contact"].Value.ToString();
                    txtSEmail.Text = row.Cells["Email"].Value.ToString();
                    cmbServiceProvided.SelectedItem = row.Cells["ServiceProvided"].Value.ToString();
                }
            }
        }

        // clear supplier data
        private void cltbtnsup_Click(object sender, EventArgs e)
        {
            ClearF();
        }

        // clear supplier data
        private void ClearF()
        {
            txtSName.Clear();
            txtSContact.Clear();
            txtSEmail.Clear();
            cmbServiceProvided.SelectedIndex = -1;
        }

       

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            profile loginForm = new profile(username);
            loginForm.Show();
        }

    

        private void guna2TextBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
           guna2TabControl1.SelectedIndex = 0;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?",
                                          "Logout Confirmation",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Login loginForm = new Login();
                loginForm.Show();
            }
        }

        private void txtCarID1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBoxCustomerCount_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBoxRentalCount_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 2;
        }

        private void guna2TileButton4_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 3;
        }

        private void guna2TileButton3_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 6;
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 4;
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton5_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 1;
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox32_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox29_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtModel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtCustomerID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbServiceProvided_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void txtRentalID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}