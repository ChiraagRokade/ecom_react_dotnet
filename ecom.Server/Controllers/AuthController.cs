using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using ecom.Server.Data;

namespace ecom.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    // Demo users - In production, these should be in a database
    private static readonly List<ApplicationUsers> _users = new()
    {
        new ApplicationUsers
        {
            Id = Guid.NewGuid(),
            Email = "demo@example.com",
            Password = HashPassword("Demo@123"), // Demo password
            UserName = "demo_user",
            FirstName = "Demo",
            LastName = "User",
            IsActive = true
        },
        new ApplicationUsers
        {
            Id = Guid.NewGuid(),
            Email = "admin@example.com",
            Password = HashPassword("Admin@123"), // Admin password
            UserName = "admin_user",
            FirstName = "Admin",
            LastName = "User",
            IsActive = true
        }
    };

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            // Validate request
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            // Find user
            var user = _users.FirstOrDefault(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Verify password
            if (!VerifyPassword(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Create response without sensitive data
            var response = new
            {
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName
                },
                message = "Login successful"
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        var inputHash = HashPassword(inputPassword);
        return inputHash.Equals(hashedPassword);
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}