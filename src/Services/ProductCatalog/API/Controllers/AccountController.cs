using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Auth.Dtos;
using ProductCatalog.API.Services;
using ProductCatalog.Infrastructure.Auth;

namespace ProductCatalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _tokenService = tokenService;
        _userManager = userManager;

    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized();

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                Image = null
            };
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
        {
            return BadRequest("Username is already taken");
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            return BadRequest("Email is already taken");
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                Image = null
            };
        }

        return BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName,
            Image = null
        };
    }
}
