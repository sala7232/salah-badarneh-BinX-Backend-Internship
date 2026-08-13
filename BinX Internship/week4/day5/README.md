# Week 4 - Day 5: API Security Hardening and Week 4 Synthesis

## Overview

Day 5 focused on hardening the existing Library API using rate limiting, a restricted CORS policy, HTTPS redirection, HSTS, and an SQL injection security review.

This day also summarizes the authentication, authorization, validation, and security work completed during Week 4.

## Continuing the Existing Project

The Day 5 security changes were added directly to the existing Library API:

```text
BinX Internship/week3/day3/MyFirstApi
```

The project was not copied into the Week 4 folder.

The `week4/day5` folder contains the Day 5 documentation and exported Postman collection.

## What I Implemented

- Added built-in ASP.NET Core rate limiting.
- Applied a general rate limit to all API requests.
- Applied a stricter rate limit to the login endpoint.
- Configured rate limiting per client IP address.
- Configured the API to return `429 Too Many Requests` when a limit is exceeded.
- Created a named CORS policy.
- Allowed only one explicitly configured frontend origin.
- Enabled HTTPS redirection.
- Enabled HSTS outside the Development environment.
- Reviewed the codebase for unsafe raw SQL queries.
- Confirmed that runtime database access uses Entity Framework Core LINQ queries.
- Added Postman requests for testing the general and login rate limits.

## Built-In Rate Limiting

The project uses the rate limiting features built into ASP.NET Core and does not require an additional NuGet package.

Two limits were configured.

### General API Limit

The global rate limiter allows:

```text
10 requests per minute per IP address
```

It applies to all API requests.

### Login Limit

The login endpoint has a stricter named policy:

```text
3 requests per minute per IP address
```

The policy is applied using:

```csharp
[EnableRateLimiting(AppRateLimitPolicies.Login)]
```

A request that exceeds either limit receives:

```text
429 Too Many Requests
```

The rate limiter uses a fixed one-minute window and does not queue rejected requests.

## Why Login Has a Stricter Limit

Login is a sensitive endpoint because repeated requests can indicate:

- A brute-force password attack.
- Automated credential testing.
- A denial-of-service attempt.

A stricter login limit slows repeated authentication attempts while allowing a higher limit for normal API operations.

## Rate-Limit Configuration

The rate limiter was registered in `Program.cs` using:

```csharp
builder.Services.AddRateLimiter(...)
```

The general limiter is configured as a global limiter.

The login limiter is configured as a named endpoint policy:

```text
LoginRateLimit
```

Rate-limit policy names are stored in:

```text
Authorization/AppAuthorization.cs
```

## Rate-Limit Tests

The following requests were added to the Postman collection:

```text
Security Hardening/Login Rate Limit - Invalid Credentials
Security Hardening/General Rate Limit - Get Books
```

### Login Test

The login request is executed four times within one minute.

Expected results:

| Attempt | Expected Response |
|---|---|
| 1 | `401 Unauthorized` |
| 2 | `401 Unauthorized` |
| 3 | `401 Unauthorized` |
| 4 | `429 Too Many Requests` |

Invalid credentials are used so that real passwords do not need to be stored in the exported Postman collection.

### General API Test

The protected Get Books request is executed eleven times with a valid JWT.

Expected results:

| Attempts | Expected Response |
|---|---|
| 1–10 | `200 OK` |
| 11 | `429 Too Many Requests` |

These limits demonstrate that the login endpoint is more strictly limited than general API endpoints.

## CORS Configuration

A named CORS policy was created:

```text
AllowFrontend
```

The policy allows only this configured frontend origin:

```text
http://localhost:3000
```

The allowed origin is stored in `appsettings.json`:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:3000"
  ]
}
```

The policy allows the frontend to use the required HTTP methods and request headers:

```csharp
policy
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod();
```

The API does not use:

```csharp
AllowAnyOrigin()
```

This prevents unknown browser-based origins from receiving permission to access API responses.

## CORS Verification

An allowed preflight request should receive:

```text
Access-Control-Allow-Origin: http://localhost:3000
```

A request from an origin such as:

```text
https://attacker.example
```

must not receive an `Access-Control-Allow-Origin` header.

A disallowed CORS origin does not necessarily receive an HTTP `403` response. The browser rejects access because the required CORS response header is absent.

Postman can inspect CORS headers, but it does not enforce CORS restrictions like a web browser.

## HTTPS Redirection

HTTPS redirection is enabled using:

```csharp
app.UseHttpsRedirection();
```

An HTTP request to:

```text
http://localhost:5267
```

is redirected to:

```text
https://localhost:7086
```

The expected redirect response is:

```text
307 Temporary Redirect
```

API requests containing JWT authorization headers are sent directly to the HTTPS URL to avoid losing the Author
```