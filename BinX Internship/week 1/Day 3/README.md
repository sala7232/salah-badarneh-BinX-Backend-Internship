# Day 3 — C# Fundamentals: Object-Oriented Programming

Day 3 focused on core OOP concepts in C#: classes, encapsulation, 
records, interfaces, and polymorphism, using a small book library 
domain as an example.

## What I Did

- Built a small domain with two related classes: `Book` and `Author`
- Added a `record` (`BookSummaryDto`) as an immutable DTO with 
  value-based equality
- Applied proper encapsulation: private setters and constructors that 
  reject invalid input (empty name/title, null author)
- Defined an `INotifiable` interface implemented by both `Book` and 
  `Author` (two unrelated classes), and demonstrated polymorphism by 
  passing both through a single method that accepts the interface type

## Tools Used

- .NET SDK
- Visual Studio Code (C# Dev Kit)
- Terminal (PowerShell)
- Git & GitHub

## How to Run

bash
cd LibraryDomain
dotnet run


## Output


Clean Code by Ahmad Karim (2008)
Summary: BookSummaryDto { Title = Clean Code, AuthorName = Ahmad Karim, PublishedYear = 2008 }

Sending notifications...
Library log for 'Clean Code': New library update available.
Email to Ahmad Karim: New library update available.
