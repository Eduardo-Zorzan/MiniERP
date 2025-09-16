using API.BussinessRules.Users.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.BussinessRules.JWT
{
	public class TokenService
	{
		public string GenerateToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "");
			
			var claims = user.GetClaims();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims), 
				Expires = DateTime.UtcNow.AddHours(8),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}

	public static class RoleClaimExtention
	{
		public static IEnumerable<Claim> GetClaims(this User user)
		{
			var result = new List<Claim>
			{
				new(ClaimTypes.Name, user.Name ?? ""),
			};
			return result;
		}
	}

}
