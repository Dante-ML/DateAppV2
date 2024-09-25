using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;

    public UsersController(DataContext context){
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync()
    {
        //Se determina en tiempo de ejecución
        var users = await _context.Users.ToListAsync();

        return Ok(users);
    }

    [Authorize]
    [HttpGet("{Id:int}")]
    public async Task<ActionResult<AppUser>> GetUsersByIdAsync(int id)
    {
        //Se determina en tiempo de ejecución
        var user = await _context.Users.FindAsync(id);

        if(user == null) return NotFound();
 
        return user;
    }

    [HttpGet("{name}")]
    public ActionResult<string> GetUsersByName(string name)
    {
        return $"Hi {name}";
    }

}
