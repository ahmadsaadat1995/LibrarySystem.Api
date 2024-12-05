# Library Management System

A comprehensive library management system designed to streamline the management of library operations, including user authentication, book inventory, and transaction management. This project incorporates modern backend development practices and technologies to ensure scalability and performance.

---

## Features

- **User Authentication:**  
  Secure login and registration using ASP.NET Identity with role-based access control.
  
- **Book Inventory Management:**  
  Add, update, and manage book information seamlessly.

- **Transaction Management:**  
  Track borrowing and return transactions with automated reminders via email.

- **Payment Integration:**  
  Online payments for fines and fees using Stripe.

- **Scalable Caching:**  
  Implemented Redis for efficient caching to improve system performance.

- **Unit of Work & Repository Pattern:**  
  Ensures clean and maintainable code with centralized data access logic.

- **Union for Business Logic:**  
  Simplified complex operations using the Union pattern.

---

## Technologies Used

- **Framework:** .NET Core
- **Database:** SQL Server
- **Caching:** Redis
- **Payment Gateway:** Stripe
- **Authentication:** ASP.NET Identity
- **Design Patterns:** Unit of Work, Repository
- **Email Server:** SMTP (for automated notifications)
- **Programming Language:** C#

---

## How to Run

1. Clone the repository:  
   ```bash
   git clone https://github.com/nadaawad25/LibrarySystem.Api.git
