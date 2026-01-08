using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MiniERP.BusinessRules.API.User
{
    public static class UserV1
    {
        private const string _url = "api/User/v1";
        
        public static async Task Register(Register.Entities.User user)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://192.168.18.38:5078");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync(_url, user);

            if (response.IsSuccessStatusCode)
                return;

            throw new Exception(response.StatusCode.ToString());
        }

        public static async Task Update(Register.Entities.User user)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://192.168.18.38:5078");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PutAsJsonAsync(_url, user);

            if (response.IsSuccessStatusCode)
                return;

            throw new Exception("Login or Password incorrects");
        }

        public static async Task Delete(string login, string token)
        {
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("http://192.168.18.38:5078");
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			HttpResponseMessage response = await client.DeleteAsync(_url + $"?login={login}");

			if (response.IsSuccessStatusCode)
				return;

			throw new Exception("Login or Password incorrects");
		}
    }
}
