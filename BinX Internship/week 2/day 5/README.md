# Day 5 — Middleware Pipeline & Dependency Injection

This project continues the ASP.NET Core Web API work from Day 4. It focuses on the middleware pipeline, middleware ordering, dependency injection, service lifetimes, and constructor injection.

## Project Structure


MyFirstApi/
├── Controllers/
│   └── BooksController.cs
├── DTOs/
│   └── BookSummaryResponse.cs
├── Middleware/
│   └── RequestLoggingMiddleware.cs
├── Services/
│   ├── IBookSummaryService.cs
│   └── BookSummaryService.cs
├── Data/
│   └── BookStore.cs
├── Program.cs
└── MyFirstApi.csproj


## What I Built

- Created a separate ASP.NET Core Web API project for Day 5.
- Added a project reference to the Day 3 `LibraryDomain` project.
- Reused the existing `Book` and `Author` domain models.
- Added a `BookStore` class with hardcoded book data.
- Created a custom request logging middleware.
- Logged each request method and path to the console.
- Demonstrated the effect of incorrect middleware ordering.
- Created an interface-based service using dependency injection.
- Registered the service with the scoped lifetime.
- Injected the service into `BooksController` through constructor injection.
- Returned book summary DTOs from the controller endpoints.

## Middleware Pipeline

Every request passes through the middleware pipeline in the order middleware is registered in `Program.cs`.

The request logging middleware is registered before the other middleware:


app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


The middleware logs output similar to:

Request: GET /api/books
Response: 200


## Middleware Ordering Experiment

A terminal middleware was temporarily placed before the request logging middleware:


app.Run(async context =>
{
    await context.Response.WriteAsync("The pipeline stopped early.");
});

app.UseMiddleware<RequestLoggingMiddleware>();


When this code was used, the request stopped before reaching the logging middleware and the controller endpoint.

After observing the result, the terminal middleware was removed and the request logging middleware was placed at the beginning of the pipeline.

## Dependency Injection

The service is registered in `Program.cs` with a scoped lifetime:


builder.Services.AddScoped<IBookSummaryService, BookSummaryService>();


`AddScoped` creates one service instance per HTTP request.

The controller receives the service through constructor injection:


private readonly IBookSummaryService _bookSummaryService;

public BooksController(IBookSummaryService bookSummaryService)
{
    _bookSummaryService = bookSummaryService;
}


The controller depends on the `IBookSummaryService` interface instead of the concrete service class.

## Endpoints

| Method | Endpoint | Description |

| GET | `/api/books` | Returns summaries for all books |
| GET | `/api/books/2008` | Returns a book summary by published year |

> The existing `Book` domain model does not contain an `Id` property, so `PublishedYear` is used as the route parameter.

## Running the Project

```bash
cd MyFirstApi
dotnet run
```

The API runs locally using the URL shown in the terminal

Example:


http://localhost:5267


Swagger is available at:


http://localhost:5267/swagger


## Testing

The following requests can be tested in Swagger or Postman:

GET /api/books
GET /api/books/2008

There are screenshots of them in folder PostmanScreenshots


## Tools Used

- .NET SDK
- ASP.NET Core
- Visual Studio Code
- Swagger
- Postman
- Git and GitHub