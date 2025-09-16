using API.BussinessRules.Cryptography;
using API.BussinessRules.Cryptography.Entities;
using API.BussinessRules.JWT;
using API.BussinessRules.Users;
using API.BussinessRules.Users.Entities;
using API.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Routes
{
	[Authorize]
	[ApiController]
	[Route("user")]
	public class UserController : ControllerBase
	{

		[AllowAnonymous]
		[HttpPost("v1/register")]
		public async Task<IActionResult> Register([FromBody] User model,
											  [FromServices] Context context)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var user = new User
			{
				Name = model.Name,
				Login = model.Login,
				Password = ""
			};

			string? evString = Environment.GetEnvironmentVariable("EV");
			string? keyString = Environment.GetEnvironmentVariable("KEY");

			if (string.IsNullOrWhiteSpace(evString) || string.IsNullOrWhiteSpace(keyString))
			{
				CriptographyReturn returned = await Cryptography.Encrypt(model.Password);

				user.Password = returned.Output;
				Environment.SetEnvironmentVariable("EV", Convert.ToBase64String(returned.Iv));
				Environment.SetEnvironmentVariable("KEY", Convert.ToBase64String(returned.Key));

			} else
			{ 
				byte[] ev = Convert.FromBase64String(evString);
				byte[] key = Convert.FromBase64String(keyString);

				user.Password = await Cryptography.Encrypt(model.Password, key, ev);
			}

			try
			{
				new SaveUser(context).Save(user, true);
				return Ok($"{user.Login} {user.Password}");
			}
			catch
			{
				return StatusCode(500, "Internal Error");
			}
		}


		[AllowAnonymous]
		[HttpPost("v1/login")]
		public async Task<IActionResult> Login(
	   [FromBody] User model,
	   [FromServices] Context context,
	   [FromServices] TokenService tokenService)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState.Values);

			var user = await context
				.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Login == model.Login);

			if (user == null)
				return StatusCode(401, "User or password invalid");
			
			CriptographyReturn criptography = new CriptographyReturn
			{
				Output = user.Password,
				Iv = Convert.FromBase64String(Environment.GetEnvironmentVariable("EV") ?? ""),
				Key = Convert.FromBase64String(Environment.GetEnvironmentVariable("KEY") ?? "")
			};
			
			if (!model.Password.Equals(Cryptography.Decrypt(criptography)))
				return StatusCode(401, "User or password invalid");

			try
			{
				var token = tokenService.GenerateToken(model);
				return Ok(token);
			}
			catch
			{
				return StatusCode(500, "Internal Error");
			}
		}
	}
}
