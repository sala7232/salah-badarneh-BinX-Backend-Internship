# Day 1 – Generic Repository

Day 1 focused on building a reusable generic repository using C# generics, applying type constraints, and practicing basic collection management.

## What I Did

- Created a reusable generic `Repository<T>` class using C# Generics.
- Applied the `where T : class` constraint to support nullable reference types.
- Implemented an `Add` method with `ArgumentNullException.ThrowIfNull`.
- Implemented a `GetAll` method that returns `IReadOnlyList<T>`.
- Implemented a `Find` method using `Func<T, bool>` and `FirstOrDefault`.
- Created repositories for both `Author` and `Book`.
- Added sample data and searched for a book by its title.
- Displayed all books stored in the repository.
- Used `IReadOnlyList<T>` to prevent external modification of the collection.

## Tools Used

- .NET SDK
- Visual Studio Code (C# Dev Kit)
- Terminal (PowerShell)
- Git & GitHub

## How to Run

bash
cd GenericRepository
dotnet run
