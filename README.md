# 🎁 Loyalty Coupon System

A **Loyalty Management System** that enables businesses to create and manage discount coupons for their loyal customers. Built with **ASP.NET Web Forms** using a **3-tier architecture** to separate concerns between the Presentation Layer, Business Logic Layer, and Data Access Layer.

---

## 📦 Project Structure

Loyalty-coupon-system/
├── LoyaltyCouponsSystem.PL/ # Presentation Layer (UI)
├── LoyaltyCouponsSystem.BLL/ # Business Logic Layer
├── LoyaltyCouponsSystem.DAL/ # Data Access Layer
├── LoyaltyCouponsSystem.PL.sln # Solution File
└── README.md

markdown
Copy
Edit

---

## 🚀 Getting Started

### ✅ Prerequisites

- **Operating System:** Windows 10 or later
- **IDE:** Visual Studio 2019 or newer
- **Database:** SQL Server
- **Framework:** .NET Framework 4.7.2 or later

### ⚙️ Installation Steps

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/mohamedelsawah345/Loyalty-coupon-system.git
Open Solution:

Open LoyaltyCouponsSystem.PL.sln in Visual Studio.

Setup the Database:

Create a SQL Server database.

Run the SQL scripts (tables, stored procedures, etc.).

Update the connection string in web.config under LoyaltyCouponsSystem.PL.

Run the Application:

Press Start in Visual Studio or F5.

🧱 System Architecture
Presentation Layer (PL): Handles user interface using ASP.NET Web Forms.

Business Logic Layer (BLL): Manages business rules (e.g. CustomerManager, CouponManager).

Data Access Layer (DAL): Interacts directly with the SQL database.

📋 Features
👥 Customer Management
Add New Customer: via AddCustomer.aspx

Edit/Delete Customers: via EditCustomer.aspx and CustomerList.aspx

🎟️ Coupon Management
Create Coupons: via AddCoupon.aspx

Edit/Delete Coupons: via EditCoupon.aspx and CouponList.aspx

Fields: Code, Value, Expiry Date, Usage Limit

📊 Reporting
Coupon Usage Report

Top Active Customers

🔐 Authentication
Login page: Login.aspx

Simple session-based user authentication

💡 Screenshots
/////


🛡️ Security Notes
Ensure strong validation on all inputs.

Always protect your connection strings and sensitive data.

Consider adding role-based access control for different user types.

📄 License
This project is licensed under the MIT License.
Feel free to use and modify as needed.

🤝 Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

🧑‍💻 Author
Mohamed Elsawah – GitHub
