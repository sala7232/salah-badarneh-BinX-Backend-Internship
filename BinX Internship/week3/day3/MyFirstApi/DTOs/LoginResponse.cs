namespace MyFirstApi.DTOs;

public record LoginResponse(
    string AccessToken,
    string TokenType,
    DateTime ExpiresAtUtc);