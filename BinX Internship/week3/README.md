# Week 3 Summary - REST API, EF Core and SQL Server

## Project

Library Catalog API

## Day 1 - REST API Design

Designed RESTful endpoints for the Books resource using plural resource
names, HTTP methods, correct status codes, nested resources, and URL
versioning through `/api/v1`.

Main endpoints:

- GET `/api/v1/books`
- GET `/api/v1/books/{id}`
- POST `/api/v1/books`
- PUT `/api/v1/books/{id}`
- DELETE `/api/v1/books/{id}`
- GET `/api/v1/authors/{authorId}/books`

## Day 2 - Normalized Database Schema

Designed the Authors and Books tables in third normal form.

- `Authors.Id` is the Authors primary key.
- `Books.Id` is the Books primary key.
- `Books.AuthorId` is a foreign key referencing `Authors.Id`.
- The relationship is one author to many books.
- SQL Server column types and constraints were selected for every field.

## Day 3 - Entity Framework Core

- Added the EF Core SQL Server, Tools, and Design packages.
- Created Author and Book entity classes.
- Created and registered LibraryDbContext.
- Configured the SQL Server connection string.
- Generated the InitialCreate migration.
- Applied the migration using `dotnet ef database update`.
- Verified the generated tables in SQL Server Management Studio.

## Day 4 - Full CRUD

Implemented asynchronous EF Core CRUD operations:

- Create returns 201 Created with a Location header.
- Get All and Get By ID use async read queries and AsNoTracking.
- Update uses EF Core change tracking.
- Delete returns 204 No Content.
- Invalid input returns 400 Bad Request.
- Missing resources return 404 Not Found.

## Day 5 - Postman Testing and Documentation

- Organized all requests into a Books resource folder.
- Added success and error-path requests for every endpoint.
- Added Post-response test scripts asserting expected status codes.
- Created a reusable Postman environment using `baseUrl`.
- Saved request descriptions and response examples.
- Ran the complete collection successfully.
- Exported the Postman collection and environment.

## Deliverables

- REST Resource Design Document
- Normalized Database Schema
- Library ERD
- EF Core Initial Migration
- Full Async CRUD API
- Exported Postman Collection
- Exported Postman Environment
- Collection Runner Results