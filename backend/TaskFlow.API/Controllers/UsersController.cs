using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Models;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> users = new();

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }


    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        user.Id = users.Count + 1;
        users.Add(user);

        return Ok(user);
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = users.FirstOrDefault(x => x.Id == id);

        if(user == null)
            return NotFound();

        users.Remove(user);

        return NoContent();
    }
}