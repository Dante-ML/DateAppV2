using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("API/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly DataContext _context;

    public UsersController(DataContext context){
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        //Se determina en tiempo de ejecución
        var users = _context.Users.ToList();

        return Ok(users);
    }

    [HttpGet("{Id}")]
    public ActionResult<AppUser> GetUsersById(int id)
    {
        //Se determina en tiempo de ejecución
        var user = _context.Users.Find(id);

        if(user == null) return NotFound();
 
        return user;
    }

}
