# Day 4 - Full CRUD Operations with Entity Framework Core

Day 4 continued the ASP.NET Core Web API created on Day 3. The in-memory Books API was replaced with asynchronous CRUD operations backed by Entity Framework Core and the SQL Server `LibraryEfDb` database.

The API code remains in `week3/day3/MyFirstApi` because Day 4 was completed by extending the existing Day 3 project rather than copying it into another project. The exported Postman collection and this documentation are stored in `week3/day4`.

## What I Did

- Replaced the in-memory `BookStore` usage in `BooksController` with `LibraryDbContext`.
- Implemented Create, Get All, Get By ID, Update, and Delete endpoints for books.
- Used asynchronous EF Core methods throughout the controller.
- Used `AsNoTracking` for read-only queries.
- Used EF Core change tracking for update and delete operations.
- Called `SaveChangesAsync` after every database-changing operation.
- Added request and response DTOs so API contracts are separate from database entities.
- Added DataAnnotations validation for titles, publication years, and author IDs.
- Checked that an author exists before creating or updating a book.
- Returned correct HTTP status codes for successful and unsuccessful requests.
- Tested every endpoint in Postman with both success and deliberate error cases.
- Exported the documented Postman collection into the Day 4 folder.

## API Base Route

```text
/api/v1/books
```

The `v1` URL segment follows the versioning convention designed on Day 1.

## Endpoints

| Operation | Method | Endpoint | Success | Error cases |
|---|---|---|---|---|
| Get all books | `GET` | `/api/v1/books` | `200 OK` | `400 Bad Request` for an invalid publication-year filter |
| Get one book | `GET` | `/api/v1/books/{id}` | `200 OK` | `404 Not Found` when the book does not exist |
| Create a book | `POST` | `/api/v1/books` | `201 Created` with a `Location` header | `400 Bad Request` for invalid data or a missing author |
| Update a book | `PUT` | `/api/v1/books/{id}` | `200 OK` | `400 Bad Request` for invalid data; `404 Not Found` for a missing book |
| Delete a book | `DELETE` | `/api/v1/books/{id}` | `204 No Content` | `404 Not Found` when the book does not exist |

## Data Transfer Objects

Three DTOs were added under `MyFirstApi/DTOs`:

### CreateBookRequest

Contains the data accepted by the Create endpoint:

- `Title`
- `PublishedYear`
- `AuthorId`

### UpdateBookRequest

Contains the replacement values accepted by the Update endpoint:

- `Title`
- `PublishedYear`
- `AuthorId`

### BookResponse

Defines the book data returned to an API client:

- `Id`
- `Title`
- `PublishedYear`
- `AuthorId`
- `AuthorName`

Using response DTOs avoids returning EF Core navigation properties directly and prevents circular JSON responses.

## Create Operation

```http
POST /api/v1/books
```

The endpoint:

1. Validates the request body.
2. Checks that `AuthorId` references an existing author.
3. Adds a new `Book` entity to `LibraryDbContext`.
4. Persists it using `SaveChangesAsync`.
5. Returns `201 Created` using `CreatedAtAction`.

The response includes a `Location` header pointing to:

```text
/api/v1/books/{newId}
```

## Read Operations

The Get All endpoint uses:

```csharp
AsNoTracking()
ToListAsync()
```

The Get By ID endpoint uses:

```csharp
AsNoTracking()
FirstOrDefaultAsync()
```

`AsNoTracking` avoids change-tracking overhead because the returned entities are used only for reading. Get By ID returns `404 Not Found` when no matching ID exists.

The Get All endpoint also accepts an optional `publishedYear` query parameter:

```http
GET /api/v1/books?publishedYear=2008
```

Values outside the accepted range of 1000 through 9999 return `400 Bad Request`.

## Update Operation and Change Tracking

```http
PUT /api/v1/books/{id}
```

The endpoint loads the book using `FindAsync`. The returned entity is tracked by EF Core. After its properties are changed, `SaveChangesAsync` detects the modifications and sends the required SQL update to the database.

The endpoint returns:

- `200 OK` when the update succeeds.
- `400 Bad Request` when the request data or author ID is invalid.
- `404 Not Found` when the book ID does not exist.

## Delete Operation

```http
DELETE /api/v1/books/{id}
```

The endpoint loads the tracked entity using `FindAsync`, removes it with `DbSet.Remove`, and persists the deletion using `SaveChangesAsync`.

It returns:

- `204 No Content` after successful deletion.
- `404 Not Found` when the book ID does not exist.

## Validation

`CreateBookRequest` and `UpdateBookRequest` use DataAnnotations:

- `[Required]` prevents a missing or empty title.
- `[MaxLength(250)]` limits the title length.
- `[Range(1000, 9999)]` validates the publication year.
- `[Range(1, int.MaxValue)]` validates the author ID format.

The controller also rejects whitespace-only titles and verifies that the supplied author exists before changing the database.

Because the controller uses `[ApiController]`, invalid DataAnnotations input automatically produces a `400 Bad Request` validation response.

## Async EF Core Methods

The controller uses the following asynchronous EF Core methods:

```text
ToListAsync
FirstOrDefaultAsync
FindAsync
SaveChangesAsync
```

All database tasks are awaited so the API does not block a request thread while waiting for SQL Server.

## Postman Testing

The exported collection is located at:

```text
PostmanDocumentation/Library API - Week 3 Day 4.postman_collection.json
```

The collection contains these requests:

| Request | Expected status |
|---|---|
| Create Book - Success | `201 Created` |
| Create Book - Invalid Input | `400 Bad Request` |
| Get All Books - Success | `200 OK` |
| Get All Books - Invalid Year | `400 Bad Request` |
| Get Book By ID - Success | `200 OK` |
| Get Book By ID - Not Found | `404 Not Found` |
| Update Book - Success | `200 OK` |
| Update Book - Invalid Input | `400 Bad Request` |
| Update Book - Not Found | `404 Not Found` |
| Delete Book - Success | `204 No Content` |
| Delete Book - Not Found | `404 Not Found` |

The Postman environment uses:

```text
baseUrl = http://localhost:5267
authorId = an existing author ID
bookId = the ID returned by the successful Create request
```

## Running the API

Run these commands from `week3/day3/MyFirstApi`:

```powershell
dotnet build
dotnet run
```

The local API URL is:

```text
http://localhost:5267
```

## Verification

The project was compiled after implementing CRUD:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Tools Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2025 Developer
- SQL Server Management Studio 22
- Postman
- Visual Studio Code
- PowerShell
