# Week 4 - Day 1: ASP.NET Core Identity and User Registration

Day 1 focused on integrating ASP.NET Core Identity into the existing Library API, adding secure user storage, applying the Identity database schema, and implementing a user registration endpoint.

## Continuing the Week 3 Project

The Week 4 security features were added directly to the existing API created during Week 3.

The API source code remains located at:

```text
BinX Internship/week3/day3/MyFirstApi
```

The project was not copied into the Week 4 folder. The Week 4 changes are isolated in the following Git branch:

```text
week4-auth-identity-validation
```

The `week4/day1` folder contains the Day 1 documentation, exported Postman files, and testing screenshots.

## What I Did

- Added ASP.NET Core Identity to the existing Week 3 Library API.
- Added the Identity Entity Framework Core NuGet package.
- Extended `LibraryDbContext` from `IdentityDbContext<IdentityUser>`.
- Preserved the existing `Authors` and `Books` entities and tables.
- Generated an `AddIdentity` Entity Framework Core migration.
- Applied the migration to the existing `LibraryEfDb` database.
- Registered `IdentityUser`, `IdentityRole`, and Identity services in `Program.cs`.
- Configured password requirements and unique email validation.
- Created a `RegisterRequest` DTO.
- Created an `AuthController`.
- Implemented a registration endpoint using `UserManager.CreateAsync`.
- Returned meaningful Identity errors for invalid registration requests.
- Tested successful registration and weak-password rejection using Postman.
- Verified that passwords are stored as hashes rather than plain text.

## NuGet Package

The following Identity package was added to the existing .NET 10 project:

```text
Microsoft.AspNetCore.Identity.EntityFrameworkCore 10.0.10
```

It was installed using:

```powershell
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 10.0.10
```

The existing Entity Framework Core SQL Server, Tools, and Design packages remain installed from Week 3.

## Identity DbContext

`LibraryDbContext` previously inherited from:

```csharp
DbContext
```

It now inherits from:

```csharp
IdentityDbContext<IdentityUser>
```

The context still exposes the application entities:

```csharp
public DbSet<Author> Authors => Set<Author>();
public DbSet<Book> Books => Set<Book>();
```
