# Day 4 — ASP.NET Core Project Setup & Routing

This project introduces the basics of building an ASP.NET Core Web API. It demonstrates project setup, routing, Controllers, Minimal APIs, Swagger, and API testing with Postman.

## Project Structure


MyFirstApi/
├── Controllers/
│   └── BooksController.cs
├── Data/
│   └── BookStore.cs
├── Program.cs
└── MyFirstApi.csproj


## What I Built

- Created a new ASP.NET Core Web API project using the .NET CLI.
- Added a project reference to the Day 3 `LibraryDomain` project.
- Reused the existing `Book` and `Author` domain models.
- Created a `BookStore` class that contains hardcoded book data.
- Added Controller endpoints for retrieving books.
- Added equivalent Minimal API endpoints in `Program.cs`.
- Added Swagger for API documentation and testing.
- Tested all endpoints in Postman and saved them in a collection.

## Project Setup

bash
dotnet new webapi -o MyFirstApi
cd MyFirstApi
dotnet add reference "..\..\..\week 1\Day 3\LibraryDomain\LibraryDomain.csproj"
dotnet add package Swashbuckle.AspNetCore


## Endpoints

### Controller Endpoints

| Method | Endpoint | Description |

| GET | `/api/books` | Returns all books |
| GET | `/api/books/2008` | Returns a single book by published year |

### Minimal API Endpoints

| Method | Endpoint | Description |

| GET | `/minimal/books` | Returns all books |
| GET | `/minimal/books/2008` | Returns a single book by published year |

> The existing `Book` domain model does not contain an `Id` property, so `PublishedYear` is used as the route parameter.

## Controllers and Minimal APIs

Controllers organize related endpoints inside a class. They are useful for larger APIs because related operations can be grouped together.

Minimal APIs define endpoints directly inside `Program.cs`. They require less code and are useful for small APIs or simple endpoints.

Both approaches in this project return the same book data.

## Running the Project

bash
cd MyFirstApi
dotnet run


The API runs locally at:

http://localhost:5064


Swagger is available at:

http://localhost:5064/swagger

## Postman Testing

A Postman collection named `MyFirstApi - Day 4` was created and contains the following requests:


GET /api/books
GET /api/books/2008
GET /minimal/books
GET /minimal/books/2008
```

## Tools Used

- .NET SDK
- ASP.NET Core Web API
- Visual Studio Code
- Swagger
- Postman
- Git and GitHub