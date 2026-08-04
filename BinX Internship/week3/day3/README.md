# Day 3 - Entity Framework Core Setup and Code-First Migrations

Day 3 focused on connecting the Week 2 ASP.NET Core Web API to SQL Server using Entity Framework Core. The Day 2 database design was converted into EF Core entity classes, registered through a `DbContext`, and applied to SQL Server using a code-first migration.

## What I Did

- Copied the Week 2 Day 5 Web API into the Week 3 Day 3 folder so the previous submission remains unchanged.
- Added the Entity Framework Core SQL Server, Tools, and Design NuGet packages.
- Created `Author` and `Book` entity classes matching the normalized Day 2 schema.
- Added navigation properties representing the one-to-many relationship between authors and books.
- Created `LibraryDbContext` with a `DbSet` for each entity.
- Configured entity constraints and the `Books.AuthorId` foreign key.
- Registered `LibraryDbContext` in the dependency-injection container.
- Added a SQL Server connection string for the local `LibraryEfDb` database.
- Generated and inspected the `InitialCreate` migration.
- Applied the migration to SQL Server.
- Confirmed the generated tables and foreign-key relationship using SQL Server Management Studio.

## Entity Models

### Author

The `Author` entity maps to the `Authors` table and contains:

- `Id` - primary key.
- `Name` - required, with a maximum length of 150 characters.
- `Country` - required, with a maximum length of 100 characters.
- `Books` - collection navigation property containing the author's books.

### Book

The `Book` entity maps to the `Books` table and contains:

- `Id` - primary key.
- `Title` - required, with a maximum length of 250 characters.
- `PublishedYear` - stored as `SMALLINT` and restricted to values between 1000 and 9999.
- `AuthorId` - foreign key referencing `Authors.Id`.
- `Author` - reference navigation property for the related author.

## Relationship

The schema has a one-to-many relationship:

- One author can have many books.
- Every book belongs to one author.
- `Books.AuthorId` references `Authors.Id`.
- Delete behavior is restricted so an author referenced by a book cannot be deleted accidentally.

## Entity Framework Core Packages

The project uses the following EF Core 10.0.10 packages:

```text
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design
```

- `SqlServer` provides the SQL Server database provider.
- `Tools` supports Entity Framework tooling and migrations.
- `Design` provides the design-time services required by `dotnet ef`.

## DbContext

`Data/LibraryDbContext.cs` is the central EF Core database context. It exposes:

```csharp
public DbSet<Author> Authors => Set<Author>();
public DbSet<Book> Books => Set<Book>();
```

The context also configures:

- The relationship between `Author` and `Book`.
- The `AuthorId` foreign key.
- Restricted delete behavior.
- The `CK_Books_PublishedYear` check constraint.

`LibraryDbContext` is registered in `Program.cs` using `AddDbContext` and the SQL Server provider.

## Database Configuration

The connection string is stored in `appsettings.json`:

```text
Server=localhost;
Database=LibraryEfDb;
Trusted_Connection=True;
TrustServerCertificate=True;
```

- `localhost` connects to the SQL Server instance on the local machine.
- `Trusted_Connection=True` uses Windows Authentication and does not store a SQL username or password.
- `TrustServerCertificate=True` allows the local development certificate.

Production credentials must never be committed to source control. Production connection strings should be supplied through environment variables or a secrets-management service.

## Code-First Migration

The initial migration was created with:

```powershell
dotnet ef migrations add InitialCreate
```

The generated migration creates:

- The `Authors` table.
- The `Books` table.
- Primary keys for both tables.
- The foreign key from `Books.AuthorId` to `Authors.Id`.
- An index on `Books.AuthorId`.
- The publication-year check constraint.

The migration was applied using:

```powershell
dotnet ef database update
```

## Database Verification

The following tables were checked using SQL Server Management Studio:

```text
dbo.Authors
dbo.Books
dbo.__EFMigrationsHistory
```

`__EFMigrationsHistory` records which Entity Framework Core migrations have been applied to the database.

The database was also checked to confirm that:

- All columns use the expected SQL Server types and lengths.
- `Authors.Id` and `Books.Id` are primary keys.
- `Books.AuthorId` is a foreign key referencing `Authors.Id`.

## Project Structure

```text
MyFirstApi/
|-- Controllers/
|-- Data/
|   |-- BookStore.cs
|   `-- LibraryDbContext.cs
|-- DTOs/
|-- Middleware/
|-- Models/
|   |-- Author.cs
|   `-- Book.cs
|-- Services/
|-- Migrations/
|   |-- 20260804153444_InitialCreate.cs
|   |-- 20260804153444_InitialCreate.Designer.cs
|   `-- LibraryDbContextModelSnapshot.cs
|-- Program.cs
|-- appsettings.json
`-- MyFirstApi.csproj
```

## Commands

Run these commands from the `MyFirstApi` directory:

```powershell
dotnet restore
dotnet build
dotnet ef migrations list
dotnet run
```

## Day 3 Scope

The purpose of Day 3 was to configure EF Core and create the database schema. The existing controller and services still use the Week 2 in-memory `BookStore`. Replacing that storage with asynchronous EF Core CRUD operations is part of Day 4.

## Tools Used

- .NET 10 SDK
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server 2025 Developer
- SQL Server Management Studio 22
- Visual Studio Code
- PowerShell
