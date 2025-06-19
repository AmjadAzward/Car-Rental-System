#  Car Rent Management System

##  Objective

The objective of this project is to streamline and automate the day-to-day operations of a car rental company. The proposed system addresses the core challenges typically faced in car rental workflows by digitalizing processes such as vehicle tracking, customer handling, payment management, and more—reducing manual workload and improving efficiency.

![Car Rental System Screenshot](https://raw.githubusercontent.com/AmjadAzward/Car-Rental-System/main/Images/Screenshot%202025-06-19%20133416.png)

![Car Rental System Screenshot](https://raw.githubusercontent.com/AmjadAzward/Car-Rental-System/main/Images/Screenshot%202025-06-19%20133433.png)

![Car Rental System Screenshot](https://raw.githubusercontent.com/AmjadAzward/Car-Rental-System/main/Images/Screenshot%202025-06-19%20133718.png)

---

##  Tech Stack

| Category       | Details                         |
|----------------|---------------------------------|
| Language       | C# (C-Sharp)                   |
| Framework      | .NET Framework / .NET 6+       |
| UI Technology  | Windows Forms (WinForms)       |
| IDE            | Microsoft Visual Studio 2022   |
| Database       | Microsoft SQL Server           |
| Reporting      | Crystal Reports               |
| Data Access    | ADO.NET                       |
| UI Enhancements| Guna.UI Framework             |

---

##  Main Functionalities

-  Car Management  
-  Customer Management  
-  Employee Management  
-  Rental (Booking) Management  
-  Payment/Billing Management  
-  Maintenance Management  
-  Supplier Management  
-  Tab-Based Navigation and User Profile Management

---

##  System Overview

The system is built using a **tab control interface** that organizes key functions into separate modules:

###  Car Management
- Add, update, delete, and view cars  
- Fields:  
  - ID  
  - Model  
  - Brand  
  - Registration Number  
  - Rental Price  
  - Availability Status  

###  Customer Management
- Add, update, delete, and view customer profiles  
- Fields:  
  - Customer ID  
  - Name  
  - Phone  
  - Driving License  
  - Email  

### 🔹 Supplier Management
- Add, update, delete, and view suppliers  
- Fields:  
  - Supplier ID  
  - Name  
  - Phone  
  - Email  
  - Services Provided  

###  Rental Management
- Link cars with customers for rental processing  
- Shows only added customers and cars — must exist in system  

###  Payment Management
- Create, update, and delete payment records for rentals  

###  Maintenance Management
- Log car service/maintenance details  
- Fields:  
  - Maintenance ID  
  - Car ID  
  - Date  
  - Remarks  
  - Status  

###  Employee Management
- Register and manage employees  
- Fields:  
  - Employee ID  
  - Name  
  - Position  
  - Phone  
  - Email  
  - Hire Date  

---

##  Installation & Setup

###  1. Download and Install Visual Studio
- [Download Visual Studio 2022](https://visualstudio.microsoft.com/downloads)  
- Select: **.NET Desktop Development** workload  
- Install and restart if prompted  

###  2. Create or Open the Project

#### New Project:
- `Create a new project` → Search: **Windows Forms App**  
- Choose:
  - `.NET Framework` (e.g., 4.8)  
  - or `.NET 6/7/8` for modern projects  
- Name: `CarRentalSystem`  
- Location: `D:\CSharpProjects`  
- Click **Create**  

#### Open Existing:
- Open `.sln` file from folder  

---

###  3. Design the UI
- Use drag-and-drop Form Designer  
- Add controls from Toolbox (TextBox, Buttons, etc.)  
- Use **TabControl** to create sections  
- Double-click controls to add logic in `Form1.cs`  

---

###  4. Build and Run
- Build: `Ctrl + Shift + B`  
- Run: `F5` or green Play button  

---

##  Notes

- DB code is separate and integrated using ADO.NET (not included in GitHub version if not required)  
- Crystal Reports used for invoice generation  
- Guna.UI enhances the form appearance  

---

##  Database Setup

This project uses Microsoft SQL Server for data storage. Follow these steps to create and configure the database.

### Prerequisites

- Microsoft SQL Server installed (Express or Developer edition recommended)  
- SQL Server Management Studio (SSMS) installed  
  - Download SSMS here: https://aka.ms/ssms  

### Steps to Set Up the Database

1. **Open SSMS and Connect to SQL Server**  
   - Launch SSMS  
   - Connect to your server instance (e.g., localhost or .\SQLEXPRESS)  

2. **Create the Database**  
   - Right-click `Databases` → `New Database...`  
   - Name it `CarRentalDB`  
   - Click **OK**  

3. **Create Tables**  
   - Open a new query window  
   - Run the SQL script

