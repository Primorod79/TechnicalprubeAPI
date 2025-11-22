# EcommerceAPI - Technical Prube

RESTful E-commerce API built with .NET 8, PostgreSQL, and deployed on Railway.

## 🚀 Overview

Complete backend API for an e-commerce system providing user management, products, categories, and images. Features JWT authentication, FluentValidation, and xUnit testing.

**Live API:** https://technicalprubeapi-production.up.railway.app

**Swagger Documentation:** https://technicalprubeapi-production.up.railway.app/

## 🏗️ Tech Stack

- **.NET 8.0** - ASP.NET Core Web API
- **PostgreSQL** - Database with Entity Framework Core
- **JWT Bearer** - Authentication & Authorization
- **BCrypt.Net** - Password hashing
- **FluentValidation** - DTO validation
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Unit testing framework
- **Docker** - Containerization
- **Railway** - Deployment platform

## 📋 Data Models

### User
- Email (unique), Username (unique), PasswordHash
- FirstName, LastName (optional)
- Role: Admin | User
- Timestamps: CreatedAt, UpdatedAt

### Product
- Name, Description, Price, Stock, ImageUrl
- CategoryId (FK to Category)
- Timestamps: CreatedAt, UpdatedAt

### Category
- Name, Description
- Products (navigation property)
- Timestamps: CreatedAt, UpdatedAt

## 🔌 API Endpoints

### Authentication (`/api/auth`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | Login and get JWT token | No |
| GET | `/api/auth/me` | Get current user info | Yes |

### Products (`/api/products`)

| Method | Endpoint | Description | Auth Required | Role |
|--------|----------|-------------|---------------|------|
| GET | `/api/products` | List all products (paginated) | Yes | Any |
| GET | `/api/products/{id}` | Get product by ID | Yes | Any |
| POST | `/api/products` | Create new product | Yes | Admin |
| PUT | `/api/products/{id}` | Update product | Yes | Admin |
| DELETE | `/api/products/{id}` | Delete product | Yes | Admin |
| GET | `/api/products/category/{categoryId}` | Get products by category | Yes | Any |

**Query Parameters (GET /products):**
- `page` (default: 1)
- `pageSize` (default: 10)
- `search` (optional)
- `categoryId` (optional)

### Categories (`/api/categories`)

| Method | Endpoint | Description | Auth Required | Role |
|--------|----------|-------------|---------------|------|
| GET | `/api/categories` | List all categories | No | Any |
| GET | `/api/categories/{id}` | Get category by ID | No | Any |
| POST | `/api/categories` | Create category | Yes | Admin |
| PUT | `/api/categories/{id}` | Update category | Yes | Admin |
| DELETE | `/api/categories/{id}` | Delete category | Yes | Admin |

### Health Check

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/health` | API health status |

---

## 🧪 Unit Tests

The project includes **11 unit tests** covering critical API functionality.

### Testing Stack
- **xUnit 2.6** - Testing framework
- **EF Core InMemory** - In-memory database for testing
- **Moq 4.20** - Mocking framework

### Test Suite

#### AuthControllerTests (4 tests)
- ✅ `Register_WithValidData_ReturnsOk` - Successful user registration
- ✅ `Register_WithDuplicateEmail_ReturnsBadRequest` - Prevents duplicate emails
- ✅ `Login_WithValidCredentials_ReturnsToken` - Returns JWT on valid login
- ✅ `Login_WithInvalidPassword_ReturnsUnauthorized` - Rejects invalid passwords

#### ProductsControllerTests (7 tests)
- ✅ `GetAll_ReturnsProducts` - Lists all products
- ✅ `Get_ExistingProduct_ReturnsProduct` - Retrieves product by ID
- ✅ `Get_NonExistingProduct_ReturnsNotFound` - Returns 404 for missing products
- ✅ `Create_ValidProduct_ReturnsCreated` - Creates new product successfully
- ✅ `Delete_ExistingProduct_ReturnsOk` - Deletes product successfully

### Run Tests
```bash
dotnet test
```

**Results:** All 11 tests passing ✅

---

## 🔧 Configuration

### Environment Variables (Railway)
```
ASPNETCORE_ENVIRONMENT=Production
DATABASE_URL=${{Postgres.DATABASE_URL}}
```

### Local Setup (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=EcommerceDB;Username=postgres;Password=postgres"
  },
  "JwtSettings": {
    "Secret": "YOUR_SECRET_KEY_MIN_32_CHARACTERS",
    "Issuer": "EcommerceAPI",
    "Audience": "EcommerceFrontend",
    "ExpirationInHours": 24
  }
}
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL 14+
- Docker (optional)

### Local Development

```bash
# Clone repository
git clone https://github.com/Primorod79/TechnicalprubeAPI.git
cd TechnicalprubeAPI/EcommerceAPI

# Restore packages
dotnet restore

# Update connection string in appsettings.json

# Apply migrations
dotnet ef database update

# Run application
dotnet run

# Access Swagger UI
# http://localhost:5000
```

### Docker

```bash
# Build image
docker build -t ecommerce-api .

# Run container
docker run -p 8080:8080 ecommerce-api
```

---

## 🔐 Security Features

- ✅ JWT Authentication with configurable expiration
- ✅ BCrypt password hashing
- ✅ Role-based authorization (Admin/User)
- ✅ CORS configuration
- ✅ FluentValidation on all DTOs
- ✅ HTTPS enforcement (Railway)
- ✅ SQL Injection protection (EF Core)
- ✅ Centralized error handling middleware
- ✅ Structured logging with Serilog

---

## 📦 Project Structure

```
EcommerceAPI/
├── Controllers/         # API Controllers
├── Core/
│   ├── Enums/          # Role enum
│   └── Interfaces/     # Repository interfaces
├── Data/               # DbContext & seeding
├── DTOs/               # Request/Response DTOs
├── Helpers/            # ApiResponse, JwtHelper, PaginatedResult
├── Infrastructure/
│   └── Repositories/  # Repository implementations
├── Middleware/         # Custom middleware
├── Migrations/         # EF Core migrations
├── Models/             # Domain models
└── Validators/         # FluentValidation validators

EcommerceAPI.Tests/     # Unit test project
```

---

## 👤 Author

**Bryan Rodriguez**
- GitHub: [@Primorod79](https://github.com/Primorod79)
- Repository: [TechnicalprubeAPI](https://github.com/Primorod79/TechnicalprubeAPI)

---

## 📄 License

This project is for educational and technical assessment purposes.
