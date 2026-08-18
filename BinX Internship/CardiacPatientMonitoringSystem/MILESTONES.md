# Project Milestones

This file maps the retained project files to the required two-day milestone plan for the Cardiac Patient Monitoring System.

## M1 - Days 1-2

### Required Output

- Solution setup and Git structure.
- C# models and DTOs.
- Route plan and initial controllers.

### Evidence

- `CardiacPatientMonitoringSystem.sln`
- `CardiacPatientMonitoring.Api/CardiacPatientMonitoring.Api.csproj`
- `CardiacPatientMonitoring.Api/Models/`
- `CardiacPatientMonitoring.Api/DTOs/`
- `CardiacPatientMonitoring.Api/Controllers/`
- The endpoint tables in `README.md`

The solution contains separate API and test projects. The domain includes `Patient`, `VitalSign`, `Medication`, and `Appointment` models, request DTOs, response DTOs, and versioned controller routes under `/api/v1`.

### Review Gate

```powershell
dotnet build
```

The solution must compile, and the routes must be visible in Swagger.

## M2 - Days 3-4

### Required Output

- Routing and middleware.
- Dependency injection.
- Async patterns and LINQ inside the project.
- Clear request pipeline and code structure.

### Evidence

- `CardiacPatientMonitoring.Api/Program.cs`
- `CardiacPatientMonitoring.Api/Middleware/ExceptionHandlingMiddleware.cs`
- `CardiacPatientMonitoring.Api/Services/IPatientService.cs`
- `CardiacPatientMonitoring.Api/Services/PatientService.cs`
- Async controller actions in `CardiacPatientMonitoring.Api/Controllers/`

`Program.cs` registers controllers, Swagger, FluentValidation, EF Core, Identity, JWT authentication, authorization, and `IPatientService`. The middleware handles exceptions centrally. The service and controllers use `async`/`await`, LINQ filtering and projection, and `AsNoTracking` for read-only database queries.

### Review Gate

The request pipeline, dependency registrations, async methods, and LINQ queries can be explained from `Program.cs`, `PatientService`, and the controllers.

## M3 - Days 5-6

### Required Output

- EF Core `DbContext` and SQL Server schema.
- Migrations, relationships, and synthetic seed data.
- Patient and vital-sign CRUD.

### Evidence

- `CardiacPatientMonitoring.Api/Data/CardiacDbContext.cs`
- `CardiacPatientMonitoring.Api/Migrations/`
- `CardiacPatientMonitoring.Api/Models/Patient.cs`
- `CardiacPatientMonitoring.Api/Models/VitalSign.cs`
- `CardiacPatientMonitoring.Api/Controllers/PatientsController.cs`
- `CardiacPatientMonitoring.Api/Controllers/VitalSignsController.cs`

`CardiacDbContext` includes the four application `DbSet` properties and the ASP.NET Core Identity schema. The patient-to-vital-sign relationship uses `PatientId`. The initial migration includes all application and Identity tables. `SeedData` adds synthetic patient and vital-sign records.

### Review Gate

```powershell
cd .\CardiacPatientMonitoring.Api
dotnet ef database update
```

The migration must create `CardiacMonitoringDb`, and patient/vital-sign CRUD must work through Swagger or Postman.

## M4 - Days 7-8

### Required Output

- Medication and appointment CRUD.
- Filtering and search.
- DTO cleanup and correct HTTP status codes.

### Evidence

- `CardiacPatientMonitoring.Api/Controllers/MedicationsController.cs`
- `CardiacPatientMonitoring.Api/Controllers/AppointmentsController.cs`
- Medication and appointment files in `Models/`, `DTOs/`, and `Validators/`

Medication and appointment endpoints support asynchronous create, read, update, and delete operations. Medication queries can filter by `patientId` and search by medication name. Appointment queries can filter by `patientId` and `status`. The endpoints use request and response DTOs and return `200`, `201`, `204`, `400`, and `404` where appropriate.

### Review Gate

All four REST modules must be usable through Swagger or Postman, including the saved filtering and search requests.

## M5 - Days 9-10

### Required Output

- Registration and login.
- Identity and JWT authentication.
- Protected routes.
- Input validation.

### Evidence

- `CardiacPatientMonitoring.Api/Controllers/AuthController.cs`
- Authentication and Identity configuration in `CardiacPatientMonitoring.Api/Program.cs`
- `CardiacPatientMonitoring.Api/DTOs/RegisterRequest.cs`
- `CardiacPatientMonitoring.Api/DTOs/LoginRequest.cs`
- `CardiacPatientMonitoring.Api/DTOs/LoginResponse.cs`
- `CardiacPatientMonitoring.Api/Validators/`
- `[Authorize]` on all four CRUD controllers

Identity stores users and hashes passwords through `UserManager`. Login verifies credentials through `SignInManager` and returns a signed JWT containing the user ID, email, and token ID claims. The JWT expires after 15 minutes. FluentValidation validates authentication and CRUD request DTOs before controller actions run.

### Review Gate

The Postman authentication folder demonstrates valid registration and login, invalid credentials, an invalid registration request, and rejection of a protected request without a token.

## M6 - Days 11-12

### Required Output

- Central error handling.
- xUnit and Moq tests for selected controller or service behavior.
- Success and failure test cases.

### Evidence

- `CardiacPatientMonitoring.Api/Middleware/ExceptionHandlingMiddleware.cs`
- `CardiacPatientMonitoring.Api.Tests/Controllers/PatientsControllerTests.cs`
- `CardiacPatientMonitoring.Api.Tests/Middleware/ExceptionHandlingMiddlewareTests.cs`
- `CardiacPatientMonitoring.Api.Tests/CardiacPatientMonitoring.Api.Tests.csproj`

The patient controller tests use Moq to isolate `IPatientService` and verify successful and failed controller results. The middleware test verifies that an unexpected exception returns a controlled `500` problem response without exposing the internal exception message.

### Review Gate

```powershell
dotnet test
```

All tests must pass, and the failure responses must remain controlled.

## M7 - Days 13-14

### Required Output

- Swagger and Postman cleanup.
- Complete README.
- Clean database setup test.
- Final demo and submission package.

### Evidence

- Swagger/OpenAPI configuration in `CardiacPatientMonitoring.Api/Program.cs`
- `Postman/CardiacPatientMonitoring.postman_collection.json`
- `Postman/CardiacPatientMonitoring.Local.postman_environment.json`
- `README.md`
- `MILESTONES.md`
- Initial EF Core migration and synthetic seed data

Swagger includes JWT bearer authorization. The Postman collection contains authentication, protected-route, CRUD, filtering, validation, and not-found scenarios. The README contains configuration, database, run, authentication, Postman, test, and demo instructions.

### Review Gate

1. Restore and build the solution.
2. Set the local JWT signing key.
3. Apply the migration to a clean local database.
4. Run the API and open Swagger.
5. Run the Postman collection with its local environment.
6. Run the automated tests.
7. Demonstrate the database, authentication, protected routes, CRUD operations, validation, safe errors, and tests in five to ten minutes.

The final package is self-contained and uses only synthetic sample data.
