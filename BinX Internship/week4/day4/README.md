# Week 4 - Day 4: Input Validation with FluentValidation

## Overview

Day 4 focused on replacing request-model DataAnnotations and manual controller validation with dedicated FluentValidation validators.

The Create and Update Book endpoints now validate incoming requests automatically and return structured `400 Bad Request` responses before invalid input reaches the controller action.

## Continuing the Existing Project

The Day 4 validation changes were added directly to the existing Library API project created during Week 3.

The API source code remains located at:

```text
BinX Internship/week3/day3/MyFirstApi
```

The project was not copied into the Week 4 folder.

The `week4/day4` folder contains the Day 4 documentation and exported Postman files.

## Objectives

- Installed FluentValidation.
- Installed the ASP.NET Core integration package.
- Created a validator for `CreateBookRequest`.
- Created a validator for `UpdateBookRequest`.
- Added real business validation rules.
- Registered the validators in the ASP.NET Core validation pipeline.
- Removed duplicate DataAnnotations from the Create and Update DTOs.
- Removed duplicate manual title validation from `BooksController`.
- Preserved database-dependent author existence checks.
- Returned structured validation error responses automatically.
- Tested validation rules individually using Postman.

## NuGet Packages

The following packages were installed:

```text
FluentValidation 11.12.0
FluentValidation.AspNetCore 11.3.1
```

Installation commands:

```powershell
dotnet add package FluentValidation --version 11.12.0
```

```powershell
dotnet add package FluentValidation.AspNetCore --version 11.3.1
```

## DataAnnotations vs. FluentValidation

DataAnnotations are suitable for simple validation rules directly attached to model properties.

Examples include:

```csharp
[Required]
[MaxLength(250)]
```

FluentValidation keeps validation rules in separate classes and provides clearer support for business rules, custom messages, rule chaining, and rules that depend on other values.

For Day 4, the DataAnnotations were removed from `CreateBookRequest` and `UpdateBookRequest`, and their validation rules were moved into dedicated validators.

The DataAnnotations used by the registration and login request models were not removed.

## Create Book Validator

The following validator was created:

```text
Validators/CreateBookRequestValidator.cs
```

It validates `CreateBookRequest` using the following rules:

| Field | Business Rule | Error Message |
|---|---|---|
| `Title` | Must not be empty or whitespace | `Book title is required.` |
| `Title` | Must not exceed 250 characters | `Book title must not exceed 250 characters.` |
| `PublishedYear` | Must be between 1000 and the current year | `Published year must be between 1000 and the current year.` |
| `AuthorId` | Must be greater than zero | `Author ID must be greater than 0.` |

The published-year rule prevents books with a future publication year from being accepted.

## Update Book Validator

The following validator was created:

```text
Validators/UpdateBookRequestValidator.cs
```

It applies the same business rules to `UpdateBookRequest`:

- The title is required.
- The title cannot exceed 250 characters.
- The publication year must be between 1000 and the current year.
- The author ID must be greater than zero.

Create and Update use separate validator classes so that their rules can be changed independently in the future.

## Validator Registration

Automatic FluentValidation integration was registered in `Program.cs`:

```csharp
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<
    CreateBookRequestValidator>();
```

The assembly scanning registration discovers both the Create and Update validators.

Because `BooksController` uses `[ApiController]`, an invalid request automatically returns `400 Bad Request` without manually checking the validation result inside the action.

## Controller Changes

The manual title checks were removed from the Create and Update actions:

```csharp
if (string.IsNullOrWhiteSpace(request.Title))
{
    return BadRequest(...);
}
```

These checks are now handled by FluentValidation before the controller action executes.

The database-dependent author check remains inside the controller:

```csharp
if (author is null)
{
    return BadRequest(new
    {
        message =
            $"Author with ID {request.AuthorId} does not exist."
    });
}
```

FluentValidation confirms that `AuthorId` is greater than zero. The controller then confirms that the positive ID belongs to an existing database record.

## Structured Validation Response

An invalid request returns a structured response similar to:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": [
      "Book title is required."
    ]
  },
  "traceId": "request-trace-id"
}
```

The `errors` object groups validation messages by field so that an API client can display the correct message beside each input.

## Authorization During Validation Tests

The Books endpoints remain protected by the authorization rules implemented during Day 3.

Create requests require an Admin token containing the `books.create` permission:

```text
Bearer {{adminToken}}
```

Update requests require a valid authenticated-user token:

```text
Bearer {{userToken}}
```

A missing or expired token returns `401 Unauthorized` before validation runs.

A valid token without the required Create permission returns `403 Forbidden` before validation runs.

## Postman Validation Requests

The validation requests are organized inside:

```text
Books - Validation
```

### Create Tests

| Test | Expected Result | Status |
|---|---|---|
| Valid Create request | `201 Created` | Completed |
| Empty title | `400 Bad Request` with the required-title message | Completed |
| Title longer than 250 characters | `400 Bad Request` with the maximum-length message | Completed |
| Future publication year | `400 Bad Request` with the year-range message | Completed |
| Author ID equal to zero | `400 Bad Request` with the Author ID message | Pending |

### Update Tests

| Test | Expected Result | Status |
|---|---|---|
| Empty title | `400 Bad Request` with the required-title message | Completed |
| Title longer than 250 characters | `400 Bad Request` with the maximum-length message | Pending |
| Future publication year | `400 Bad Request` with the year-range message | Pending |
| Author ID equal to zero | `400 Bad Request` with the Author ID message | Pending |

Each request changes only one field to an invalid value while keeping all other fields valid. This confirms the exact rule and error message independently.

## Files Added

```text
Validators/CreateBookRequestValidator.cs
Validators/UpdateBookRequestValidator.cs
week4/day4/README.md
week4/day4/Postman/Library-API-Week4-Day4.postman_collection.json
```

## Files Updated

```text
DTOs/CreateBookRequest.cs
DTOs/UpdateBookRequest.cs
Controllers/BooksController.cs
Program.cs
MyFirstApi.csproj
```

## Database Changes

No Entity Framework Core migration was required because validation changes do not modify the database schema.

## Tools Used

- FluentValidation
- ASP.NET Core
- Postman
- Visual Studio Code