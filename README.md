# VillaAPI

## Overview

VillaAPI is a RESTful API built with ASP.NET Core that manages villa information. This API allows clients to perform CRUD operations on villa data, including creating, reading, updating, and deleting villa records. The project leverages Entity Framework Core for database interactions and follows best practices for API development.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete villa records.
- **Entity Framework Core**: Used for database interactions.
- **Dependency Injection**: Utilizes ASP.NET Core's built-in dependency injection.
- **Error Handling**: Comprehensive error handling and response codes.
- **DTOs**: Data Transfer Objects for data encapsulation and transfer.
- **Swagger**: Integrated Swagger for API documentation and testing.

## Technologies Used

- **ASP.NET Core**: Web framework for building the API.
- **Entity Framework Core**: ORM for database operations.
- **SQL Server**: Database provider.
- **Swagger**: API documentation and testing tool.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. **Clone the repository**:
   - git clone https://github.com/yourusername/VillaAPI.git
   - cd VillaAPI
2. **Set up the database**:
   - Update the connection string in `appsettings.json`:
   -  "ConnectionStrings": {
   "DefaultSQLConnection": "Server=your_server;Database=VillaDB;Trusted_Connection=True;MultipleActiveResultSets=true"
 }
3. **Apply Migrations**
4. **Update the Database**
5. **Run the application**

   
