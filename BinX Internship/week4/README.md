# Week 4 Summary

## Project

The Week 4 security features were added directly to the existing Library API:

`BinX Internship/week3/day3/MyFirstApi`

## Day 1 - ASP.NET Core Identity

- Integrated ASP.NET Core Identity with Entity Framework Core.
- Extended the existing DbContext from IdentityDbContext.
- Added the Identity database schema through an EF Core migration.
- Implemented user registration with UserManager.
- Configured password requirements and unique email validation.
- Confirmed that passwords are stored as secure hashes.

## Day 2 - JWT Authentication

- Implemented login using SignInManager.
- Returned 401 Unauthorized for invalid credentials.
- Generated signed JWT access tokens.
- Added user ID, email, role, and permission claims.
- Configured issuer, audience, signing key, and token validation.
- Configured a 15-minute token expiration.
- Stored the JWT signing key outside source control.

## Day 3 - Authorization

- Protected the Books CRUD controller with Authorize.
- Created User and Admin roles.
- Assigned two separate test users to the roles.
- Restricted Delete Book to the Admin role.
- Created the CanCreateBooks authorization policy.
- Required the books.create permission for Create Book.
- Confirmed the difference between 401 Unauthorized and 403 Forbidden.

## Day 4 - FluentValidation

- Added validators for CreateBookRequest and UpdateBookRequest.
- Validated required titles and maximum title length.
- Rejected future publication years.
- Required AuthorId to be greater than zero.
- Registered automatic validation in the ASP.NET Core pipeline.
- Returned structured 400 ValidationProblemDetails responses.
- Kept database-dependent author existence checks inside the controller.

## Day 5 - API Hardening

### Rate Limiting

- Configured a global fixed-window limit of 10 requests per minute per IP address.
- Configured a stricter login limit of 3 requests per minute per IP address.
- Confirmed that exceeding a limit returns 429 Too Many Requests.

### CORS

- Created a named CORS policy called AllowFrontend.
- Allowed only the specific origin: http://localhost:3000.
- Allowed the required request headers and HTTP methods.
- Confirmed that a disallowed origin does not receive an Access-Control-Allow-Origin header.

### HTTPS and HSTS

- Enabled HTTPS redirection.
- Confirmed that HTTP requests redirect to HTTPS with status 307.
- Enabled HSTS outside the Development environment.
- Kept HSTS disabled during local development.

### SQL Injection Review

- Searched the codebase for raw SQL APIs.
- Found no FromSqlRaw, ExecuteSqlRaw, or interpolated SQL queries.
- Confirmed that runtime database access uses Entity Framework Core LINQ queries.
- Confirmed that EF Core parameterizes query values by default.
- Confirmed that Verify-Database.sql is a manual database inspection file and is not executed by the API.

## Verification Results

| Test | Result |
|---|---|
| Login attempts 1-3 | 401 Unauthorized |
| Login attempt 4 | 429 Too Many Requests |
| General requests 1-10 | 200 OK |
| General request 11 | 429 Too Many Requests |
| Allowed CORS origin | Access-Control-Allow-Origin returned |
| Disallowed CORS origin | Access-Control-Allow-Origin absent |
| HTTP request | 307 redirect to HTTPS |
| HSTS | Enabled outside Development |
| Raw SQL audit | No unsafe raw SQL found |

## Tools Used

- ASP.NET Core Identity
- JWT Bearer Authentication
- ASP.NET Core Authorization
- FluentValidation
- Built-in ASP.NET Core Rate Limiting
- Entity Framework Core
- Postman
- SQL Server
