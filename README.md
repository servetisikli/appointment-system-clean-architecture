# Appointment System - Clean Architecture with Authentication

A modern, multi-layered, and scalable appointment management API built with **.NET 9**, Entity Framework Core, Clean Architecture principles, and a robust authentication system with role-based permissions.

---

## 🏗️ Architecture

- **API (Presentation):**  
  Handles HTTP requests, authentication, and authorization. Serves Swagger documentation.
- **Application:**  
  Business logic, interfaces, DTOs, and service contracts.
- **Domain:**  
  Core business entities, value objects, and rules.
- **Infrastructure:**  
  Data access (EF Core, repositories), authentication services, password hashing, JWT token generation.

---

## ⚡ Tech Stack

- [.NET 9 (Web API)](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT Authentication](https://jwt.io/)
- [Role-Based Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles)
- [Swagger / OpenAPI](https://swagger.io/)
- [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## 🔐 Authentication & Authorization

This API uses JWT (JSON Web Token) based authentication with role-based authorization:

### Available Roles
- **Admin**: Full access to all features, user management
- **Manager**: Limited administrative access
- **User**: Access only to own appointments and basic features

### Authentication Endpoints
- **POST /api/auth/register**: Register a new user (default role: User)
- **POST /api/auth/login**: Authenticate and receive a JWT token

### User Management Endpoints (Requires Admin role)
- **GET /api/users**: List all users
- **GET /api/users/{id}**: Get user details
- **PUT /api/users/{id}**: Update user details
- **DELETE /api/users/{id}**: Delete user
- **POST /api/users/{userId}/roles/{roleName}**: Assign role to user
- **DELETE /api/users/{userId}/roles/{roleName}**: Remove role from user

### Protected Appointment Endpoints
- All appointment endpoints require authentication
- Regular users can only access their own appointments
- Admins can access all appointments

---

## 🚀 Getting Started

### 1. Solution & Project Structure

This repository uses a layered folder structure.  
You can scaffold a similar structure with the following commands:

```sh
dotnet new sln -n AppointmentSystem
dotnet new webapi -n AppointmentSystem
dotnet new classlib -n AppointmentSystem.Domain
dotnet new classlib -n AppointmentSystem.Infrastructure
dotnet new classlib -n AppointmentSystem.Application

dotnet sln add AppointmentSystem/
dotnet sln add AppointmentSystem.Domain/
dotnet sln add AppointmentSystem.Infrastructure/
dotnet sln add AppointmentSystem.Application/

dotnet add AppointmentSystem reference AppointmentSystem.Domain
dotnet add AppointmentSystem reference AppointmentSystem.Infrastructure
dotnet add AppointmentSystem reference AppointmentSystem.Application
dotnet add AppointmentSystem.Infrastructure reference AppointmentSystem.Domain
dotnet add AppointmentSystem.Application reference AppointmentSystem.Domain
```

### 2. Install Dependencies

#### API Layer

```sh
dotnet add AppointmentSystem package Swashbuckle.AspNetCore
dotnet add AppointmentSystem package Microsoft.EntityFrameworkCore.Design
```

#### Infrastructure Layer

```sh
dotnet add AppointmentSystem.Infrastructure package Microsoft.EntityFrameworkCore
dotnet add AppointmentSystem.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
dotnet add AppointmentSystem.Infrastructure package Microsoft.EntityFrameworkCore.Tools
```

### 3. Configure Database Connection

Edit `appsettings.json` and set your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=AppointmentDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 4. Entity & DbContext Example

Define your domain entities in `AppointmentSystem.Domain` and your context in `AppointmentSystem.Infrastructure`:

```csharp
// AppointmentSystem.Domain/Entities/Appointment.cs
public class Appointment
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}
```

```csharp
// AppointmentSystem.Infrastructure/Data/AppDbContext.cs
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Appointment> Appointments { get; set; }
}
```

### 5. Migrations & Database Update

```sh
dotnet ef migrations add InitialCreate --project AppointmentSystem.Infrastructure --startup-project AppointmentSystem
dotnet ef database update --project AppointmentSystem.Infrastructure --startup-project AppointmentSystem
```

### 6. Register Services in `Program.cs`

```csharp
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
```

### 7. Run the API

```sh
dotnet run --project AppointmentSystem
```
Then open [http://localhost:5000/swagger](http://localhost:5000/swagger) (check your terminal for the correct port).

---

## 🗂️ Project Structure

```
appointment-system-clean-architecture/
│
├── AppointmentSystem/           # API Layer (Presentation)
├── AppointmentSystem.Application/   # Application Layer (Services, Interfaces)
├── AppointmentSystem.Domain/        # Domain Layer (Entities)
├── AppointmentSystem.Infrastructure/ # Infrastructure Layer (EF Core, Repositories)
│
├── .editorconfig
├── .gitignore
└── README.md
```

---

## 📚 References

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft Docs: ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
- [EF Core Docs](https://learn.microsoft.com/en-us/ef/core/)
- [Swagger/OpenAPI](https://swagger.io/)

---
