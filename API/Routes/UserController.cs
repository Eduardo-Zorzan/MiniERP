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
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{

		[AllowAnonymous]
		[HttpPost("v1/register")]
		public async Task<IActionResult> Register([FromBody] User model,
											  [FromServices] Context context)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = new User
			{
				Name = model.Name,
				Login = model.Login,
				Password = ""
			};

			string? evString = Environment.GetEnvironmentVariable("EV");
			string? keyString = Environment.GetEnvironmentVariable("KEY");

			Env.Load();
			if (string.IsNullOrWhiteSpace(evString) || string.IsNullOrWhiteSpace(keyString))
				throw new Exception("Key encryption not set on .env file");

			byte[] ev = Convert.FromBase64String(evString);
			byte[] key = Convert.FromBase64String(keyString);

			user.Password = await Cryptography.Encrypt(model.Password, key, ev);

			try
			{
				new SaveUser(context).Save(user, true);
				return Ok($"{user.Login} {model.Password}");
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
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

			if (string.IsNullOrWhiteSpace(model.Token))
			{
				try
				{
					var user = await context
								.Users
								.AsNoTracking()
								.FirstOrDefaultAsync(x => x.Email == model.Login);

					if (user == null)
						return StatusCode(401, "User or password invalid");

					CriptographyReturn criptography = new CriptographyReturn
					{
						Output = user.Password,
						Iv = Convert.FromBase64String(Environment.GetEnvironmentVariable("EV") ?? ""),
						Key = Convert.FromBase64String(Environment.GetEnvironmentVariable("KEY") ?? "")
					};

					if (!(model.Password ?? "").Equals(await Cryptography.Decrypt(criptography)))
						return StatusCode(401, "User or password invalid");

					model.Token = tokenService.GenerateToken(model);
					model.Name = user.Name;

					return Ok(model);
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
}
