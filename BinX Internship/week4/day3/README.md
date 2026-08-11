# Week 4 - Day 3: Route Protection and Role-Based Authorization

## Overview

Day 3 focused on protecting the existing Library API routes using JWT authentication, creating User and Admin roles, applying role-based access control, and defining a custom claims-based authorization policy.

## Continuing the Existing Project

The Day 3 security changes were added directly to the existing API project created during Week 3.

The API source code remains located at:

```text
BinX Internship/week3/day3/MyFirstApi
```

The project was not copied into the Week 4 folder. The `week4/day3` folder contains the documentation and Postman files for this task.

## What I Implemented

- Protected the complete Books CRUD controller with `[Authorize]`.
- Created `User` and `Admin` roles.
- Created two separate local test users.
- Assigned each test user to the appropriate role using `UserManager`.
- Added user roles to the generated JWT.
- Configured ASP.NET Core to use the `role` JWT claim for role authorization.
- Restricted the Delete endpoint to the Admin role.
- Defined a named claims-based authorization policy.
- Applied the custom policy to the Create Book endpoint.
- Added a custom `permission` claim to the Admin user.
- Configured Postman to capture and reuse JWT access tokens.
- Stored local test-user credentials using .NET User Secrets.
- Kept passwords and JWT signing secrets out of source control.

## Authorization Constants

The following authorization values were defined in:

```text
Authorization/AppAuthorization.cs
```

Roles:

```text
User
Admin
```

Named policy:

```text
CanCreateBooks
```

Custom claim:

```text
permission = books.create
```

Keeping these values in one file prevents repeated authorization strings and reduces the possibility of typing errors.

## Identity Seeder

A development-only Identity seeder was created in:

```text
Data/IdentitySeeder.cs
```

The seeder uses:

```text
RoleManager<IdentityRole>
UserManager<IdentityUser>
```

It performs the following operations:

- Creates the `User` role if it does not exist.
- Creates the `Admin` role if it does not exist.
- Creates a local User test account.
- Creates a separate local Admin test account.
- Assigns the User account to the `User` role.
- Assigns the Admin account to the `Admin` role.
- Adds the `books.create` permission claim to the Admin account.
- Checks for existing roles, users, and claims to avoid duplicates.

The seeder only runs when the application environment is Development.

## User Secrets

The test-user emails and passwords are loaded from .NET User Secrets:

```text
SeedUsers:User:Email
SeedUsers:User:Password
SeedUsers:Admin:Email
SeedUsers:Admin:Password
```

These values are not stored in `appsettings.json` and are not committed to GitHub.

## JWT Changes

The login endpoint now loads the user's roles and stored Identity claims before generating the JWT.

The generated token contains:

```text
sub
email
jti
role
permission
exp
iss
aud
```

ASP.NET Core JWT Bearer authentication was configured with:

```csharp
RoleClaimType = "role"
```

This allows attributes such as the following to read the role from the JWT:

```csharp
[Authorize(Roles = "Admin")]
```

Users must log in again after receiving a new role or claim because an existing JWT does not update automatically.

## Protected Books Controller

The complete `BooksController` is protected with:

```csharp
[Authorize]
```

This requires a valid JWT for every Books endpoint.

A request without a token, with an invalid token, or with an expired token is rejected with:

```text
401 Unauthorized
```

## Role-Based Authorization

The Delete endpoint is restricted using:

```csharp
[Authorize(Roles = AppRoles.Admin)]
```

Expected behavior:

| Request | Expected Response |
|---|---|
| Delete without a token | `401 Unauthorized` |
| Delete using a User-role token | `403 Forbidden` |
| Delete an existing book using an Admin token | `204 No Content` |
| Delete a missing book using an Admin token | `404 Not Found` |

The endpoint restriction has been implemented. The final User and Admin Delete requests remain pending in Postman.

## Claims-Based Authorization Policy

A named policy called `CanCreateBooks` was registered in `Program.cs`.

The policy requires:

```text
permission = books.create
```

It was applied to the Create Book endpoint:

```csharp
[Authorize(Policy = AppPolicies.CanCreateBooks)]
```

Expected behavior:

| Request | Expected Response |
|---|---|
| Create without a token | `401 Unauthorized` |
| Create using a token without the permission | `403 Forbidden` |
| Create using a token containing `books.create` | `201 Created` |

This policy demonstrates claims-based authorization beyond a simple role check.

## Authorization Rules

| Endpoint | Authorization Requirement |
|---|---|
| `GET /api/v1/books` | Authenticated user |
| `GET /api/v1/books/{id}` | Authenticated user |
| `POST /api/v1/books` | `CanCreateBooks` policy |
| `PUT /api/v1/books/{id}` | Authenticated user |
| `DELETE /api/v1/books/{id}` | Admin role |

## Postman Environment

The Postman environment contains:

```text
baseUrl
userToken
adminToken
authorId
bookId
```

The User login request saves its token using:

```javascript
pm.environment.set(
    "userToken",
    response.accessToken
);
```

The Admin login request can save its token using:

```javascript
pm.environment.set(
    "adminToken",
    response.accessToken
);
```

Protected requests reuse these tokens through Postman's Bearer Token authorization.

User request:

```text
{{userToken}}
```

Admin request:

```text
{{adminToken}}
```

## HTTP Status Code Meaning

| Status | Meaning |
|---|---|
| `200 OK` | Login or protected read succeeded |
| `201 Created` | A book was created successfully |
| `204 No Content` | A book was deleted successfully |
| `401 Unauthorized` | A valid JWT was not provided |
| `403 Forbidden` | The user is authenticated but lacks permission |
| `404 Not Found` | The requested book does not exist |

## Files Added

```text
Authorization/AppAuthorization.cs
Data/IdentitySeeder.cs
week4/day3/README.md
```

## Files Updated

```text
Controllers/AuthController.cs
Controllers/BooksController.cs
Program.cs
```

## Database Changes

No Entity Framework Core migration was required for Day 3.

Roles, user-role assignments, and claims are data stored in the existing ASP.NET Core Identity tables created during Day 1.

## Testing Status

- The project builds successfully without errors.
- User login was tested successfully.
- JWT token generation includes roles and stored user claims.
- Protected route and policy behavior are implemented.
- Final Admin login and User/Admin Delete tests remain pending in Postman.

## Tools Used

- ASP.NET Core Identity
- ASP.NET Core Authorization
- JWT Bearer Authentication
- Entity Framework Core
- Postman
- .NET User Secrets