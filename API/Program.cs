using API.BussinessRules.JWT;
using API.BussinessRules.Users.Entities;
using API.Database;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddDbContext<Context>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers(); 
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapControllers();

app.Run();

[JsonSerializable(typeof(User))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
