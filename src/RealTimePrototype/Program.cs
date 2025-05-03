using RealTimePrototype.API.Endpoints;
using RealTimePrototype.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies();
builder.Services.AddHostedServices();

//builder.Services.AddOpenApi();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

app.MapEndpoints("api");

app.Run();
