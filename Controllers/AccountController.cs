using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    SignInManager<AppUser> signinManager) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var createdUser = await userManager.CreateAsync(appUser, dto.Password);
            if (!createdUser.Succeeded) return StatusCode(500, createdUser.Errors);
            var roleResult = await userManager.AddToRoleAsync(appUser, "User");
            return roleResult.Succeeded
                ? Ok(new NewUserDto
                {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Token = tokenService.CreateToken(appUser)
                })
                : StatusCode(500, roleResult.Errors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == dto.Username.ToLower());
        if (user == null) return Unauthorized("Invalid Username!");

        var result = await signinManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded) return Unauthorized("Username or password invalid");

        return Ok(new NewUserDto
        {
            Username = user.UserName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        });
    }
}