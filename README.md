# 🎓 Student Attendance Management System

A complete **ASP.NET Core 8 MVC** web application for managing student attendance with role-based access control (Admin, Teacher, Student).

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue)
![SQLite](https://img.shields.io/badge/Database-SQLite-green)
![Bootstrap](https://img.shields.io/badge/UI-Bootstrap%205-purple)
![License](https://img.shields.io/badge/License-MIT-yellow)

---

## 📋 Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Installation Steps](#installation-steps)
- [How to Run](#how-to-run)
- [Default Login Credentials](#default-login-credentials)
- [Project Structure](#project-structure)
- [Usage Guide](#usage-guide)
- [Contributing](#contributing)
- [License](#license)

---

## ✨ Features

- ✅ **Role-Based Authentication** - Admin, Teacher, and Student with separate dashboards
- ✅ **Student Management** - Complete CRUD operations for student records
- ✅ **Teacher Management** - Add, edit, and manage teacher information
- ✅ **Subject Management** - Create and assign subjects to teachers and classes
- ✅ **Daily Attendance Marking** - Teachers can mark attendance for their subjects
- ✅ **Comprehensive Reports** - View attendance statistics and percentages
- ✅ **Excel Export** - Download attendance reports in Excel format
- ✅ **Responsive Design** - Modern Bootstrap 5 UI that works on all devices
- ✅ **SQLite Database** - No SQL Server installation required
- ✅ **Secure Authentication** - ASP.NET Core Identity with password hashing

---

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| **ASP.NET Core MVC** | 8.0 | Web Framework |
| **C#** | 12.0 | Programming Language |
| **Entity Framework Core** | 8.0 | ORM for Database |
| **SQLite** | 3.x | Database Engine |
| **ASP.NET Core Identity** | 8.0 | Authentication & Authorization |
| **Bootstrap** | 5.3 | Frontend UI Framework |
| **jQuery** | 3.6 | JavaScript Library |
| **ClosedXML** | 0.102 | Excel Export Functionality |

---

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 8 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio Code** or **Visual Studio 2022**
- **Git** (for cloning) - [Download here](https://git-scm.com/downloads)

---

## 🚀 Installation Steps

### 1. Clone the Repository
git clone https://github.com/nandanita251/StudentAttendanceSystem.git

cd StudentAttendanceSystem


### 2. Navigate to Project Directory
cd StudentAttendanceSystem


### 3. Restore NuGet Packages
dotnet restore


### 4. Apply Database Migrations
dotnet ef database update




This command will:
- Create the SQLite database (`StudentAttendance.db`)
- Create all required tables (Users, Students, Teachers, Subjects, Attendance)
- Seed default admin, teacher, and student accounts

---

## ▶️ How to Run the Project

### Method 1: Using Command Line
dotnet run


### Method 2: Using Visual Studio Code
1. Open the project folder in VS Code
2. Press `F5` or click **Run → Start Debugging**

### Method 3: Using Visual Studio 2022
1. Open the `.sln` file or project folder
2. Press `F5` or click the **▶ Play** button

### Access the Application
Once running, open your browser and navigate to:
http://localhost:5068

or
https://localhost:7068


---

## 🔐 Default Login Credentials

After running the application, use these credentials to log in:

### Admin Account
- **Email:** `admin@attendance.com`
- **Password:** `Admin@123`
- **Access:** Full system control - manage students, teachers, subjects, and view all reports

### Teacher Account
- **Email:** `teacher@attendance.com`
- **Password:** `Teacher@123`
- **Access:** Mark attendance, view assigned subjects, generate reports

### Student Account
- **Email:** `student@attendance.com`
- **Password:** `Student@123`
- **Access:** View personal attendance, subject-wise statistics, download reports


---

## 📖 Usage Guide

### For Administrators

1. **Login** with admin credentials
2. **Dashboard Overview:** View total students, teachers, subjects, and today's attendance count
3. **Manage Students:** 
   - Navigate to "Manage Students"
   - Add new students with Name, Email, Roll No, and Class
   - Edit or delete existing student records
4. **Manage Teachers:**
   - Add teachers with their subject specialization
   - Assign subjects to teachers
5. **Manage Subjects:**
   - Create subjects with Subject Name, Code, and Class
   - Assign teacher to each subject
6. **Generate Reports:**
   - Select class and optional subject/date range
   - View attendance percentage for each student
   - Export reports to Excel

### For Teachers

1. **Login** with teacher credentials
2. **View Dashboard:** See all assigned subjects with class information
3. **Mark Attendance:**
   - Click "Mark Attendance" on any subject card
   - System displays all students from that class
   - Mark Present/Absent for each student
   - Add optional remarks
   - Save attendance (can update if already marked for the day)
4. **View Attendance:** Check previously marked attendance by date
5. **Generate Reports:** View subject-wise attendance reports and export to Excel

### For Students

1. **Login** with student credentials
2. **View Dashboard:** See overall attendance percentage with subject breakdown
3. **Subject-wise Statistics:** Check attendance for each subject individually
4. **Attendance History:** 
   - Filter by subject or date range
   - View detailed day-by-day attendance records
5. **Track Progress:** Monitor attendance to maintain minimum 75% requirement

---


## 👨‍💻 Author

**Nandanita**
- GitHub: [@nandanita251](https://github.com/nandanita251)
- Project Link: [https://github.com/nandanita251/StudentAttendanceSystem](https://github.com/nandanita251/StudentAttendanceSystem)

---

