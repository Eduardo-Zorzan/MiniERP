using API.BussinessRules.Users.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.BussinessRules.JWT
{
	public class TokenService
	{
		private readonly JwtSecurityTokenHandler _tokenHandler = new();

		public string GenerateToken(User user)
		{
			var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "");
			
			var claims = user.GetClaims();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims), 
				Expires = DateTime.UtcNow.AddHours(8),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = _tokenHandler.CreateToken(tokenDescriptor);
			return _tokenHandler.WriteToken(token);
		}

		public void ValidateToken(string token)
		{
			try
			{
				_tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "")),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);
			}
			catch (Exception ex)
			{
				throw new Exception("Invalid token", ex);
			}
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
