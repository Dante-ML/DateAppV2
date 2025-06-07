using System.Security.Cryptography;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers;

public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    IMapper mapper) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterAsync(RegisterRequest request)
    {
        if(await UserExistsAsync(request.UserName)){
            return BadRequest("Username already in use");
        }

        //Unicamente lo ejecuta y luego ejecuta el dispose
        var user = mapper.Map<AppUser>(request);
        user.UserName = request.Username.ToLowerInvariant();
        var result = await userManager.CreateAsync(user, request.Password);   

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.Users
        .Include(x => x.Photos)
        .FirstOrDefaultAsync(x => x.NormalizedUserName == request.Username.ToUpperInvariant());

        if (user == null || user.UserName == null){
            return Unauthorized("Invalid username");
        }
            
        var result = await userManager.CheckPasswordAsync(user, request.Password);

        if (!result){
            return Unauthorized("Invalid username or password");
        }

        return new UserResponse{
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
        };
    }

    private async Task<bool> UserExistsAsync(string username) => 
        await userManager.Users.AnyAsync(u => u.NormalizedUserName == username.ToUpperInvariant());
}