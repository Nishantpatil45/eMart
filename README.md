# eMart.Service.Api

eMart.Service.Api is a RESTful API built with ASP.NET Core (.NET 8) for managing users, products, categories, and favorites in an e-commerce platform. It uses JWT authentication, Entity Framework Core with MySQL, and provides endpoints for user and product management, including favorite functionality.

## Features

- **User Authentication**: Secure login and registration using JWT tokens.
- **Product Management**: CRUD operations for products.
- **Category Management**: CRUD operations for categories.
- **Favorites**: Add/remove products to/from user favorites.
- **Swagger/OpenAPI**: Interactive API documentation.
- **HTTPS Enforcement**: Redirects HTTP requests to HTTPS.

## Technologies Used

- ASP.NET Core (.NET 8)
- Entity Framework Core (MySQL)
- JWT Authentication
- Swagger (Swashbuckle)
- Dependency Injection

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- Visual Studio 2022 (recommended)

### Configuration

1. **Database Connection**:  
   Update the `DefaultConnection` string in `appsettings.json` to point to your MySQL database.

2. **JWT Settings**:  
   Set the `JwtSettings` section in `appsettings.json`

### Build and Run

1. Restore NuGet packages:
2. Apply database migrations (if needed):
3. Run the API:
4. Access Swagger UI at [https://localhost:443/swagger](https://localhost:443/swagger).

## API Endpoints

- `POST /api/v1/Favorite/{id}`: Add product to favorites.
- `GET /api/v1/Favorite/list`: Get all favorite products for logged-in user.
- `DELETE /api/v1/Favorite/{id}`: Remove product from favorites.
- Additional endpoints for users, products, and categories.

## Project Structure

- `eMart.Service.Api`: Main API project.
- `eMart.Service.Core`: Business logic, DTOs, interfaces, repositories.
- `eMart.Service.EntityFrameworkCore`: EF Core DbContext and migrations.
- `eMart.Service.DataModels`: Entity models.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.