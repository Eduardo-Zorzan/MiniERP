using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MiniERP.BusinessRules.API.Login;

public class LoginV1
{
    private const string _url = "api/Login/v1";
    
    public static async Task<BusinessRules.Login.Entities.User?> Login(BusinessRules.Login.Entities.User user, string token)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://192.168.18.38:5078");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response;
        if (!string.IsNullOrWhiteSpace(token))
            response = await client.PostAsJsonAsync(_url, token);
        else
            response = await client.PostAsJsonAsync(_url, user);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<BusinessRules.Login.Entities.User>();

        throw new Exception("Login or Password incorrects");
    }
}