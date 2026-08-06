using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.DTOs;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string DemoEmail = "admin@taskflow.com";
    private const string DemoPassword = "TaskFlow123!";

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        if (!string.Equals(dto.Email, DemoEmail, StringComparison.OrdinalIgnoreCase) ||
            dto.Password != DemoPassword)
        {
            return Unauthorized(new { message = "Correo o contrasena incorrectos." });
        }

        return Ok(new
        {
            token = "taskflow-demo-session",
            user = new { name = "Administrador", email = DemoEmail }
        });
    }
}
