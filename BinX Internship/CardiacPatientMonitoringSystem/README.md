# Cardiac Patient Monitoring System

This project is a standalone ASP.NET Core REST API for a cardiac patient monitoring prototype. It manages patient profiles, vital-sign measurements, medications, and appointments.

The API uses SQL Server through Entity Framework Core, ASP.NET Core Identity for user storage and password hashing, JWT bearer authentication for protected routes, and FluentValidation for request validation. Swagger and Postman can be used to review the current API without a separate user interface.

## Phase 3 Project Scope

The selected Phase 3 project is a Healthcare Management API focused on cardiac patient monitoring. Its scope covers authentication, patient profiles, vital-sign measurements, medications, appointments, validation, SQL Server persistence, and documented REST workflows. By Week 9, it can realistically reach the professional baseline through role-based access control, unit and integration tests, deployment, CI/CD, and complete documentation.

## Main Features

- Register a user and log in with email and password.
- Issue a signed JWT access token with a 15-minute expiry.
- Protect all patient, vital-sign, medication, and appointment routes.
- Perform asynchronous CRUD operations with Entity Framework Core.
- Search patients by name or medical record number.
- Filter vital signs, medications, and appointments by patient.
- Search medications by name and filter appointments by status.
- Validate request DTOs and return structured `400 Bad Request` responses.
- Log request methods, paths, and response status codes with custom middleware.

## Project Structure

```text
CardiacPatientMonitoringSystem/
|-- CardiacPatientMonitoring.Api/
|   |-- Controllers/
|   |-- Data/
|   |-- DTOs/
|   |-- Middleware/
|   |-- Migrations/
|   |-- Models/
|   |-- Repositories/
|   |-- Services/
|   |-- Validators/
|   |-- Program.cs
|   `-- appsettings.json
|-- CardiacPatientMonitoring.Api.Tests/
|   |-- Integration/
|   `-- Services/
|-- Postman/
|-- CardiacPatientMonitoringSystem.sln
`-- README.md
```

## Requirements

- .NET 10 SDK
- SQL Server
- Entity Framework Core CLI 10.0.10
- Postman for the exported collection

Check the installed SDK and EF Core CLI:

```powershell
dotnet --version
dotnet ef --version
```

If `dotnet ef` is not installed, run:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.10
```

## Configuration

The default database connection is stored in `CardiacPatientMonitoring.Api/appsettings.json`:

```text
Server=localhost;Database=CardiacMonitoringDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Change the server name in that connection string if the local SQL Server instance uses a different name.

The JWT issuer, audience, and 15-minute expiry are also stored in `appsettings.json`. The signing key is not committed to source control. Store a local key with .NET User Secrets from the API project directory:

```powershell
cd .\CardiacPatientMonitoring.Api
dotnet user-secrets set "Jwt:Key" "Cardiac-Monitoring-Local-Key-At-Least-32-Characters"
```

Use a different secret for any non-local environment.

## Database Setup

From the project root, restore the solution and apply the existing migration:

```powershell
dotnet restore
cd .\CardiacPatientMonitoring.Api
dotnet ef database update
```

The migration creates the application tables, ASP.NET Core Identity tables, relationships, and constraints.

### Application Tables

| Table | Purpose |
|---|---|
| `Patients` | Stores patient profiles and unique medical record numbers. |
| `VitalSigns` | Stores heart rate, blood pressure, and oxygen saturation measurements. |
| `Medications` | Stores medication name, dosage, frequency, and treatment dates. |
| `Appointments` | Stores appointment date, purpose, and status. |

Each patient has a one-to-many relationship with vital signs, medications, and appointments. The related tables use `PatientId` as a foreign key. Deleting a patient deletes that patient's related records through cascade delete.

## Build and Run

From the solution directory:

```powershell
dotnet build
cd .\CardiacPatientMonitoring.Api
dotnet run --launch-profile https
```

The local addresses are:

```text
https://localhost:7147
http://localhost:5147
```

Swagger opens at:

```text
https://localhost:7147/swagger
```

If the local HTTPS certificate is not trusted, run:

```powershell
dotnet dev-certs https --trust
```

## Authentication

### Register

```http
POST /api/v1/auth/register
```

```json
{
  "email": "monitor@example.com",
  "password": "CardiacTest123!"
}
```

A successful registration returns `201 Created`. Identity hashes the password before storing it. An invalid password or an email that is already registered returns `400 Bad Request` with meaningful errors.

### Login

```http
POST /api/v1/auth/login
```

```json
{
  "email": "monitor@example.com",
  "password": "CardiacTest123!"
}
```

A successful login returns `200 OK` with an `accessToken`, token type, and expiry time. Invalid credentials return `401 Unauthorized`.

In Postman, the login request stores the token in the `accessToken` environment variable for the protected requests.

## API Endpoints

The authentication endpoints are public. Every other endpoint requires a valid JWT bearer token.

### Authentication

| Method | Route | Success | Purpose |
|---|---|---|---|
| `POST` | `/api/v1/auth/register` | `201 Created` | Register a user. |
| `POST` | `/api/v1/auth/login` | `200 OK` | Verify credentials and return a JWT. |

### Patients

| Method | Route | Success | Purpose |
|---|---|---|---|
| `GET` | `/api/v1/patients?search={value}` | `200 OK` | Get all patients or search by name/record number. |
| `GET` | `/api/v1/patients/{id}` | `200 OK` | Get one patient. |
| `POST` | `/api/v1/patients` | `201 Created` | Create a patient. |
| `PUT` | `/api/v1/patients/{id}` | `200 OK` | Update a patient. |
| `DELETE` | `/api/v1/patients/{id}` | `204 No Content` | Delete a patient. |

### Vital Signs

| Method | Route | Success | Purpose |
|---|---|---|---|
| `GET` | `/api/v1/vital-signs?patientId={id}` | `200 OK` | Get all vital signs or filter by patient. |
| `GET` | `/api/v1/vital-signs/{id}` | `200 OK` | Get one vital-sign record. |
| `POST` | `/api/v1/vital-signs` | `201 Created` | Create a vital-sign record. |
| `PUT` | `/api/v1/vital-signs/{id}` | `200 OK` | Update a vital-sign record. |
| `DELETE` | `/api/v1/vital-signs/{id}` | `204 No Content` | Delete a vital-sign record. |

### Medications

| Method | Route | Success | Purpose |
|---|---|---|---|
| `GET` | `/api/v1/medications?patientId={id}&search={value}` | `200 OK` | Get medications with optional patient and name filters. |
| `GET` | `/api/v1/medications/{id}` | `200 OK` | Get one medication. |
| `POST` | `/api/v1/medications` | `201 Created` | Create a medication. |
| `PUT` | `/api/v1/medications/{id}` | `200 OK` | Update a medication. |
| `DELETE` | `/api/v1/medications/{id}` | `204 No Content` | Delete a medication. |

### Appointments

| Method | Route | Success | Purpose |
|---|---|---|---|
| `GET` | `/api/v1/appointments?patientId={id}&status={status}` | `200 OK` | Get appointments with optional patient and status filters. |
| `GET` | `/api/v1/appointments/{id}` | `200 OK` | Get one appointment. |
| `POST` | `/api/v1/appointments` | `201 Created` | Create an appointment. |
| `PUT` | `/api/v1/appointments/{id}` | `200 OK` | Update an appointment. |
| `DELETE` | `/api/v1/appointments/{id}` | `204 No Content` | Delete an appointment. |

Appointment status values are `Scheduled`, `Completed`, and `Cancelled`.

## Validation and API Responses

FluentValidation checks every create, update, register, and login request. The business rules include:

- Required patient profile values and a valid medical record number format.
- A date of birth in the past.
- Vital-sign ranges and diastolic pressure lower than systolic pressure.
- Medication end date on or after the start date.
- Valid patient IDs and appointment status values.

Common responses are:

| Status | Meaning |
|---|---|
| `400 Bad Request` | Validation failed or a referenced patient does not exist. |
| `401 Unauthorized` | A valid JWT was not supplied. |
| `404 Not Found` | The requested record does not exist. |
| `409 Conflict` | A patient medical record number already exists. |

Validation errors use the structured ASP.NET Core validation response.

## Postman

Import both files from the `Postman` folder:

```text
Postman/CardiacPatientMonitoring.postman_collection.json
Postman/CardiacPatientMonitoring.Local.postman_environment.json
```

Select the **Cardiac Patient Monitoring - Local** environment and run the collection in its saved order. The environment uses:

```text
baseUrl = https://localhost:7147
```

The registration and login requests use the environment email and password. The login test saves the returned JWT as `accessToken`, and the create requests save record IDs for the later read, update, and delete requests.

The collection demonstrates:

- Registration and login success and failure cases.
- Rejection of a protected route without a token.
- CRUD success and not-found cases.
- Validation failures.
- Patient search and module filters.

## Tests

Run the test suite from the solution directory:

```powershell
dotnet test
```

The Week 5 Day 1 tests use xUnit and the Arrange-Act-Assert pattern. Three `[Fact]` tests verify low, normal, and high heart-rate statuses, while one `[Theory]` runs three pulse-pressure calculation cases.

The Week 5 Day 2 tests use Moq to isolate `PatientService` from `IPatientRepository`. They verify a configured return value, a repository exception, and that `GetByIdAsync` is called exactly once.

The Week 5 Day 3 integration tests use `WebApplicationFactory` to send real HTTP requests through the API pipeline. They use an isolated EF Core In-Memory database and a signed test JWT to verify the patient Get-by-id success response and its not-found response.

## Tools Used

- C# and .NET 10
- ASP.NET Core Web API
- Entity Framework Core and SQL Server
- ASP.NET Core Identity and JWT bearer authentication
- FluentValidation
- Swagger and Postman
- xUnit
- Moq
- Microsoft.AspNetCore.Mvc.Testing
- EF Core In-Memory provider
- Git and GitHub
