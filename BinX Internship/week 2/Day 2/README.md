# Day 2 – Advanced LINQ & Deferred Execution

Day 2 focused on advanced LINQ operations — grouping, joining, and flattening nested collections — and understanding deferred vs immediate execution.

## What I Did

- Created `Customer`, `Order`, and `LineItem` model classes.
- Built two related collections (`customers` and `orders`) with 6 items each, linked by `CustomerId`.
- Used `GroupBy` to calculate the total order amount per customer.
- Used `Join` to combine customer names with their order amounts.
- Used `SelectMany` to flatten the line items across all orders into a single sequence.
- Demonstrated deferred execution by defining a `Where` query before adding a new order to the source list, then showing the new order still appeared when the query was enumerated.
- Explained how calling `ToList()` immediately after defining a query would have locked in the results and excluded the new order.

## Tools Used

- .NET SDK
- Visual Studio Code (C# Dev Kit)
- Terminal (PowerShell)
- Git & GitHub

## How to Run

bash
cd LinqPractice
dotnet run
