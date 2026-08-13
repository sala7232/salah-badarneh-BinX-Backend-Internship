using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.DTOs;
using MyFirstApi.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace MyFirstApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim();

        var user = new IdentityUser
        {
            UserName = normalizedEmail,
            Email = normalizedEmail
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = "Registration failed.",
                errors = result.Errors.Select(error => new
                {
                    error.Code,
                    error.Description
                })
            });
        }

        return StatusCode(
            StatusCodes.Status201Created,
            new
            {
                message = "User registered successfully.",
                user = new
                {
                    user.Id,
                    user.Email
                }
            });
    }

    [HttpPost("login")]
    [EnableRateLimiting(AppRateLimitPolicies.Login)]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request)
    {
        var normalizedEmail = request.Email.Trim();

        var user = await _userManager.FindByEmailAsync(
            normalizedEmail);

        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var result = await _signInManager
            .CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var response = await CreateTokenAsync(user);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("validate-token")]
    public IActionResult ValidateToken()
    {
        return Ok(new
        {
            message = "Token is valid.",
            userId = User.FindFirst(
                JwtRegisteredClaimNames.Sub)?.Value,
            email = User.FindFirst(
                JwtRegisteredClaimNames.Email)?.Value
        });
    }

    private async Task<LoginResponse> CreateTokenAsync(IdentityUser user)
    {
        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT issuer is missing.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "JWT audience is missing.");

        var signingKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT signing key is missing.");

        var expiryMinutes =
            _configuration.GetValue<int>("Jwt:ExpiryMinutes");

        var issuedAtUtc = DateTime.UtcNow;
        var expiresAtUtc =
            issuedAtUtc.AddMinutes(expiryMinutes);
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.Id),

            new(
                JwtRegisteredClaimNames.Email,
                user.Email!),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        claims.AddRange(
          roles.Select(role =>
           new Claim(
            AppClaimTypes.Role,
            role)));
        claims.AddRange(userClaims);



        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(signingKey));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: issuedAtUtc,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        var accessToken =
            new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse(
            accessToken,
            "Bearer",
            expiresAtUtc);
    }
}