using System.Text.Json.Serialization;
using API.BussinessRules.Users.Entities;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

RouteGroupBuilder saveUser = app.MapGroup("/SaveUser");
saveUser.MapPost("/", (MV_SaveUser body, HttpRequest request) =>
{
	var authorizationToken = request.Headers["Authorization"].ToString();
	if (body is null)
		return Results.BadRequest("Body invalid");

	if (string.IsNullOrWhiteSpace(authorizationToken))
		return Results.Unauthorized();

	try
	{
		new API.BussinessRules.Users.SaveUser().Save(body, true);
	}
	catch (Exception ex)
	{
		return Results.InternalServerError(ex.Message);
	}

	return Results.Ok();
});

app.Run();

[JsonSerializable(typeof(MV_SaveUser))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
