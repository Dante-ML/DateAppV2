using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _repository;

    public UsersController(IUserRepository repository){
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetAllAsync()
    {
        //Se determina en tiempo de ejecución
        var users = await _repository.GetAllAsync();

        return Ok(users);
    }

    [HttpGet("{Id:int}")]
    public async Task<ActionResult<AppUser>> GetByIdAsync(int id)
    {
        //Se determina en tiempo de ejecución
        var user = await _repository.GetByIdAsync(id);

        if(user == null) return NotFound();
 
        return user;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<AppUser>> GetUsersByUsernameAsync(string username)
    {
        var user = await _repository.GetByUsernameAsync(username);

        if(user == null) return NotFound();
 
        return user;
    }

}
