# Week 5 Summary - Testing and Error Handling

## Chosen Project and Scope

The chosen Phase 3 capstone is the Cardiac Patient Monitoring System, an ASP.NET Core REST API for managing patient profiles, vital-sign measurements, medications, and appointments. Its current scope includes SQL Server persistence through Entity Framework Core, Identity and JWT authentication, protected CRUD endpoints, FluentValidation, Postman documentation, automated tests, and centralized error handling.

## Risk-Based Testing Priorities

| Priority | High-risk logic | Why it matters | Test evidence |
|---|---|---|---|
| 1 | Heart-rate status classification | Incorrect boundary handling could misclassify a clinical reading. | Three xUnit facts cover the low, normal, and high branches. |
| 2 | Pulse-pressure calculation | An incorrect systolic-minus-diastolic calculation would produce misleading clinical data. | One xUnit theory covers three blood-pressure combinations. |
| 3 | Medical record number normalization | Differences in casing or surrounding spaces could bypass the duplicate-patient check. | A Moq unit test verifies normalization and the exact repository call. |

## Test Suite

The test project uses xUnit for unit and integration tests and Moq to isolate service dependencies.

- Nine executed unit-test cases cover vital-sign assessment, pulse-pressure calculation, patient mapping, repository failure behavior, and medical-record-number normalization.
- Two integration tests cover the primary `GET /api/v1/patients/{id}` endpoint: a successful response with the complete body and a `404 Not Found` response.
- One integration test deliberately triggers an unhandled service exception and verifies the global error response.
- Integration tests use `WebApplicationFactory<Program>`, an isolated EF Core In-Memory database, and a signed test JWT.

## Centralized Error Handling

The global exception-handling middleware catches unhandled exceptions, records structured logs containing the HTTP method, request path, and trace identifier, and returns a standardized `application/problem+json` response. Clients receive a safe generic message, while exception messages, types, and stack traces remain server-side.

## Full Test Run

Run the complete suite from the solution directory:

```powershell
dotnet test .\CardiacPatientMonitoringSystem.sln --configuration Release
```

Final result:

```text
Passed: 12
Failed: 0
Skipped: 0
```

## Sprint 1 Handoff

The project enters Sprint 1 with risk-focused unit tests, endpoint-level integration tests, an isolated test database, authenticated test requests, and consistent production-safe error responses. These patterns form the testing baseline for the endpoints added during the upcoming Phase 3 sprints.

This document is ready to copy into Notion for the Week 5 mentor check-in.
