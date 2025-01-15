# ShopSphere_UserServiceAPI

Shopping Cart User Service API
This is a RESTful API for managing shopping cart users, developed using .NET 8 with a focus on Domain-Driven Design (DDD) and Clean Architecture principles. The application utilizes Entity Framework Core for database operations and implements JWT Authentication for secure user management.

Features
User Management: Create, read, user profiles in the system.
JWT Authentication: Secure API access using JSON Web Tokens.
Data Persistence: SQL Server database with Entity Framework Core for ORM-based data management.
Clean Architecture: The application follows a layered architecture, separating concerns into distinct components for maintainability, scalability, and testability.
Domain-Driven Design: The design is based on domain entities and services, ensuring that the business logic remains at the heart of the application.

Technologies Used
.NET 8: The latest version of .NET for building modern, high-performance web APIs.
Entity Framework Core: ORM for database interaction.
SQL Server: Relational database for data persistence.
JWT Authentication: Secure user authentication and authorization.
Swagger/OpenAPI: API documentation for easy testing and exploration.

Architecture
This API follows Clean Architecture and Domain-Driven Design principles:

* Core: Contains the domain models and business logic.
* Application: Contains application services and use cases.
* Infrastructure: Manages database and external system interactions (e.g., EF Core).
* API: The outer layer exposing RESTful endpoints for clients.

Getting Started
Prerequisites
.NET 8 SDK
SQL Server (or compatible database)

Installation
- Clone this repository.
- cd shopping-cart-user-service.
- Restore NuGet packages.
- Update the appsettings.json file with your database connection string.
- Apply database migrations.
- Run the application.
  

Contributing
Feel free to fork this project and submit pull requests for any improvements, bug fixes, or features.
