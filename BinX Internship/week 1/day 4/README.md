# Day 4 — C# Fundamentals: Collections, LINQ & Async

Day 4 focused on working with collections using LINQ, writing a 
simple async method, and handling exceptions from user input

## What I Did

- Created a list of 8 books using the domain model from Day 3
- Wrote 3 LINQ queries: filtering by genre, projecting titles only, 
  and calculating the average published year
- Wrote an async method that simulates an I/O delay using `Task.Delay` 
  and awaited it from the entry point
- Wrapped year parsing in a try/catch that handles `FormatException` 
  specifically, with both a valid and an invalid input example

## Tools Used

- .NET SDK
- Visual Studio Code (C# Dev Kit)
- Terminal (PowerShell)
- Git & GitHub

## How to Run

bash
cd LibraryQueries
dotnet run
