# Human Capital Management

A comprehensive ASP.NET Core MVC application for managing human resources, departments, users, and HR processes.

## Overview

This repository implements a Human Capital Management system focused on employee, department, and user management, with authentication and role-based access. The application is built using C#, ASP.NET Core, and Entity Framework Core, and does **not** include any web.api project or libraries.

## Features

- **Employee Management:** Add, edit, and view employee details.
- **Department Management:** Organize employees into departments and manage department data.
- **User & Role Management:** Secure authentication and authorization with ASP.NET Identity, including admin, manager, and employee roles.
- **Authentication:** Login, logout, and registration (registration route is redirected to login for security).
- **Seeding:** Automatic creation of roles and demo users on startup.
- **Razor Pages & MVC:** UI built with Razor views, layouts, and partials for maintainable and reusable interfaces.

## Technology Stack

- **Backend:** ASP.NET Core MVC (C#)
- **ORM:** Entity Framework Core (SQL Server)
- **Frontend:** Razor Pages, Bootstrap, custom CSS/JavaScript
- **Authentication:** ASP.NET Core Identity (with roles)
- **Project Structure:**
  - `HCM.Web`: Main web application (views, controllers, identity)
  - `HCM.Data`: Entity Framework Core data models and context
  - `HCM.Services.Data`: Business logic and service layer (e.g., Employee, Department services)
  - `HCM.Common`: Constants and shared definitions
  - `HCM.Web.Infrastructure`: Middleware and application startup extensions

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or cloud instance)

### Setup

1. **Clone the repository:**
    ```bash
    git clone https://github.com/Martinesko/Human-Capital-Management.git
    cd Human-Capital-Management
    ```

2. **Configure the database:**
    - Update `appsettings.json` in `HCM.Web` with your SQL Server connection string.

3. **Apply migrations:**
    ```bash
    dotnet ef database update --project HCM.Data
    ```

4. **Run the application:**
    ```bash
    dotnet run --project HCM.Web
    ```
    - Open [http://localhost:5000](http://localhost:5000) in your browser.

### Default Users

On first run, the system seeds demo users for HR Admin, Managers, and Employees. You can use these to log in and explore features.
```
