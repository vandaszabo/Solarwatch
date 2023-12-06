using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Contracts;
using SolarWatch.Repository;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("List"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAll();
        
        if (users.IsNullOrEmpty())
        {
            return NotFound("Users are not found.");
        }

        return Ok(users);
    }

    [HttpDelete("Delete"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser([FromBody] UserRequest request)
    {
        if (string.IsNullOrEmpty(request.UserName))
        {
            return BadRequest("Invalid request. Provide a valid userName in the request body.");
        }

        var result = await _userService.DeleteUser(request.UserName);
        
        if (result == null)
        {
            return StatusCode(500, "Error deleting user");
        }

        return Ok(result);
    }
        
        
    [HttpPost("Roles")]
    public async Task<IActionResult> GetUserRoles([FromBody] UserRequest request)
    {
        if (string.IsNullOrEmpty(request.UserName))
        {
            return BadRequest("Invalid request. Provide a valid userName in the request body.");
        }

        var roles = await _userService.GetRoles(request.UserName);

        if (roles.IsNullOrEmpty())
        {
            return NotFound("Roles are not found.");
        }

        return Ok(roles);
    }
    
    [HttpPatch("SetRole"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> ChangeRole([FromBody] ChangeRoleRequest request)
    {
        if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Role))
        {
            return BadRequest("Invalid request. Provide a valid userName and role in the request body.");
        }
        var result = await _userService.SetRole(request.UserName, request.Role);

        if (result == null)
        {
            return StatusCode(500, "Error updating user role");
        }

        return Ok(result);
    }
}