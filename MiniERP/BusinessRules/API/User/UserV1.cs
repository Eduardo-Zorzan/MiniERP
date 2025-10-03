using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MiniERP.Database.Services.Users;
using System.Text;
using System.Threading.Tasks;

namespace MiniERP.BusinessRules.API.User
{
	public static class UserV1
	{
		private const string prefix = "api/User/v1";
		static HttpClient client = new HttpClient();

		public static async Task<Login.Entities.User?> Login(Login.Entities.User user, string token)
		{
			client.BaseAddress = new Uri("http://localhost:64195/");
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpResponseMessage response;
			if (!string.IsNullOrWhiteSpace(token))
				response = await client.PostAsJsonAsync($"{prefix}/login", token);
			else
				response = await client.PostAsJsonAsync($"{prefix}/login", user);

			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<Login.Entities.User>();
			
			throw new Exception("Login or Password incorrects");
		}

	}
}
