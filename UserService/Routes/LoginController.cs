using System.Security;
using UserService.BussinessRules.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.BussinessRules.JWT;
using UserService.BussinessRules.Users.Entities;
using UserService.Database;

namespace UserService.Routes;

[Authorize]
[Route("api/login/v1")]
[ApiController]
public class LoginController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(
        [FromBody] User model,
        [FromServices] Context context,
        [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.Values);

        if (string.IsNullOrWhiteSpace(model.Token))
        {
            try
            {
                model = await new TryLogin(context, tokenService).Login(model);
                return Ok(model);
            }
            catch (VerificationException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal Error");
            }
        }
        else
        {
            try
            {
                tokenService.ValidateToken(model.Token);
                return Ok();
            }
            catch
            {
                return StatusCode(401, "Unauthorized");
            }
        }
    }
}