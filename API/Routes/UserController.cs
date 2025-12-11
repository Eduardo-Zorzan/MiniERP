using System.Security;
using API.BussinessRules.Cryptography;
using API.BussinessRules.Cryptography.Entities;
using API.BussinessRules.JWT;
using API.BussinessRules.Users;
using API.BussinessRules.Users.Entities;
using API.Database;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Routes
{
    [Authorize]
    [Route("api/user/v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User model,
            [FromServices] Context context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = model.Name,
                Login = model.Login,
                Password = "",
                ProfileImage = model.ProfileImage,
            };

            string? evString = Environment.GetEnvironmentVariable("EV");
            string? keyString = Environment.GetEnvironmentVariable("KEY");

            Env.Load();
            if (string.IsNullOrWhiteSpace(evString) || string.IsNullOrWhiteSpace(keyString))
                throw new Exception("Key encryption not set on .env file");

            byte[] ev = Convert.FromBase64String(evString);
            byte[] key = Convert.FromBase64String(keyString);

            user.Password = await Cryptography.Encrypt(model.Password ?? "", key, ev);

            try
            {
                new SaveUser(context).Save(user, true);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User model,
            [FromServices] Context context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = model.Name,
                Login = model.Login,
                Password = "",
                ProfileImage = model.ProfileImage,
            };

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                try
                {
                    await new BussinessRules.Login.TryLogin(context).Login(model);
                }
                catch (VerificationException ex)
                {
                    return StatusCode(401, ex.Message);
                }
            }

            string? evString = Environment.GetEnvironmentVariable("EV");
            string? keyString = Environment.GetEnvironmentVariable("KEY");

            Env.Load();
            if (string.IsNullOrWhiteSpace(evString) || string.IsNullOrWhiteSpace(keyString))
                throw new Exception("Key encryption not set on .env file");

            byte[] ev = Convert.FromBase64String(evString);
            byte[] key = Convert.FromBase64String(keyString);

            user.Password = await Cryptography.Encrypt(model.Password ?? "", key, ev);

            try
            {
                await new UpdateUser(context).Update(user, true);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}