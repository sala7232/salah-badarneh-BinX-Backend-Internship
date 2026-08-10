# Week 4 - Day 2: JWT Authentication and Token Issuance

## Overview

This task adds JWT-based login and token validation to the Library API.

The implementation continues using the existing API project from Week 3:

```text
BinX Internship/week3/day3/MyFirstApi
```

The Week 4 Day 2 folder contains the documentation and exported Postman collection for this task.

## Objectives Completed

- Implemented a login endpoint using ASP.NET Core Identity.
- Verified user credentials using `SignInManager`.
- Returned `401 Unauthorized` for invalid login attempts.
- Generated a signed JWT for valid users.
- Added the user's ID and email as JWT claims.
- Configured JWT Bearer authentication.
- Added a protected endpoint for testing tokens.
- Configured the token to expire after 15 minutes.
- Tested valid, invalid, and expired tokens using Postman.
- Decoded the generated token and verified its claims.

## NuGet Package

The following package was added to the existing API project:

```text
Microsoft.AspNetCore.Authentication.JwtBearer
```

This package allows ASP.NET Core to read and validate JWT Bearer tokens.

## JWT Configuration

The following non-secret settings were added to `appsettings.json`:

```json
"Jwt": {
  "Issuer": "LibraryApi",
  "Audience": "LibraryApiClient",
  "ExpiryMinutes": 15
}
```

The JWT signing key was stored using .NET User Secrets and was not added to the repository:

```powershell
dotnet user-secrets set "Jwt:Key" "<your-secret-key>"
```

## Login Request DTO

A `LoginRequest` DTO was created with the following fields:

| Field | Type | Validation |
|---|---|---|
| `Email` | string | Required and must be a valid email |
| `Password` | string | Required |

File:

```text
DTOs/LoginRequest.cs
```

## Login Response DTO

A `LoginResponse` DTO was created to return the generated token:

| Field | Description |
|---|---|
| `AccessToken` | The generated JWT |
| `TokenType` | The authentication scheme, which is `Bearer` |
| `ExpiresAtUtc` | The UTC date and time when the token expires |

File:

```text
DTOs/LoginResponse.cs
```

## Login Endpoint

### Request

```http
POST /api/v1/auth/login
```

Example request body:

```json
{
  "email": "user@example.com",
  "password": "StrongPassword123!"
}
```

The endpoint searches for the user by email and verifies the password using:

```csharp
SignInManager.CheckPasswordSignInAsync()
```

### Successful Response

A successful login returns:

```http
200 OK
```

Example response:

```json
{
  "accessToken": "<generated-jwt>",
  "tokenType": "Bearer",
  "expiresAtUtc": "2026-08-10T12:15:00Z"
}
```

### Invalid Credentials

An incorrect email or password returns:

```http
401 Unauthorized
```

Example response:

```json
{
  "message": "Invalid email or password."
}
```

The same message is returned for both an invalid email and an invalid password to avoid exposing whether an account exists.

## JWT Claims

The generated token contains the following claims:

| Claim | Description |
|---|---|
| `sub` | The Identity user ID |
| `email` | The user's email address |
| `jti` | A unique identifier for the token |
| `exp` | The token expiration time |
| `iss` | The token issuer |
| `aud` | The intended token audience |

The token is signed using the `HMAC-SHA256` algorithm.

## JWT Bearer Authentication

JWT Bearer authentication was registered in `Program.cs`.

Token validation checks:

- The token issuer.
- The token audience.
- The signing key.
- The token signature.
- The token expiration time.

`ClockSkew` was set to zero so that a token is rejected immediately after it expires.

The authentication middleware was added before the authorization middleware:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

## Protected Test Endpoint

A protected endpoint was added to verify that JWT authentication is working:

```http
GET /api/v1/auth/validate-token
```

The endpoint uses the `[Authorize]` attribute.

The request must contain the following header:

```http
Authorization: Bearer <access-token>
```

A valid token returns:

```http
200 OK
```

An invalid, missing, or expired token returns:

```http
401 Unauthorized
```

## Postman Tests

The following requests were tested in Postman:

| Test | Expected Result |
|---|---|
| Login with valid credentials | `200 OK` and a JWT |
| Login with an incorrect password | `401 Unauthorized` |
| Access the protected endpoint with a valid token | `200 OK` |
| Access the protected endpoint without a token | `401 Unauthorized` |
| Access the protected endpoint with an expired token | `401 Unauthorized` |

The token was saved in the Postman `accessToken` variable and used in the Authorization header:

```text
Bearer {{accessToken}}
```

## JWT Inspection

The generated access token was decoded using `jwt.io`.

The decoded payload was checked to confirm the presence of:

```text
sub
email
jti
exp
iss
aud
```

Only the access token was pasted into the decoder. The JWT signing key was not shared.

## Token Expiration Test

The JWT expiry duration is configured as 15 minutes.

A short-lived token was issued for testing, and the protected endpoint was requested again after the token expired. The API rejected the expired token with:

```http
401 Unauthorized
```

The expiry duration was then restored to 15 minutes.

## Files Added

```text
DTOs/LoginRequest.cs
DTOs/LoginResponse.cs
week4/day2/README.md
week4/day2/Postman/Library API - Week 4.postman_collection.json
```

## Files Updated

```text
Controllers/AuthController.cs
Program.cs
appsettings.json
MyFirstApi.csproj
```

## Security Notes

- Passwords are verified by ASP.NET Core Identity and are never compared manually.
- Passwords are not included in the JWT.
- JWT payloads are readable and must not contain sensitive information.
- The signing key is stored in .NET User Secrets.
- The signing key is not committed to GitHub.
- Invalid login attempts return a generic error message.
- Tokens have a short expiration period to limit the impact of a stolen token.

## Database Changes

No new database migration was required for Day 2 because the Identity database tables were already created during Week 4 Day 1.

## Tools Used

- ASP.NET Core Identity
- ASP.NET Core JWT Bearer Authentication
- Entity Framework Core
- System.IdentityModel.Tokens.Jwt
- Postman
- jwt.io
- .NET User Secrets
````