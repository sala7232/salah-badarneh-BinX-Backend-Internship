
# Day 3: Async, Task Parallelism & Cancellation

Day 3 focused on working with asynchronous programming in C#, comparing sequential and parallel task execution, and handling task cancellation using `CancellationToken`.

## What I Did

- Created three asynchronous methods that simulate file downloads using `Task.Delay`
- Implemented sequential execution by awaiting each download operation one after another
- Implemented parallel execution using `Task.WhenAll` to run multiple tasks simultaneously
- Used `Stopwatch` to measure and compare execution time between sequential and parallel approaches
- Added cancellation support using `CancellationTokenSource` with a timeout of 5 seconds
- Handled `OperationCanceledException` when an asynchronous operation is cancelled

## Concepts Covered

- `async` / `await`
- `Task`
- `Task.WhenAll`
- `CancellationToken`
- `CancellationTokenSource`
- Exception handling in asynchronous operations
- Performance comparison between sequential and parallel execution

## Tools Used

- .NET SDK
- Visual Studio Code (C# Dev Kit)
- Terminal (PowerShell)
- Git & GitHub

## How to Run

```bash
cd ConcurrencyDemo
dotnet run