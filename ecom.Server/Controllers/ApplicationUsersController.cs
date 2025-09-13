using Microsoft.AspNetCore.Mvc;
using ecom.Server.Models;
using ecom.Server.ViewModels;

namespace ecom.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationUsersController : ControllerBase
{
    // Example: Replace with your actual data source
    private static readonly List<ApplicationUsers> users = new();

    [HttpGet]
    public IEnumerable<ApplicationUserViewModel> Get()
    {
        return users.Select(u => new ApplicationUserViewModel
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            IsActive = u.IsActive
        });
    }
}