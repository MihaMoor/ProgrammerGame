using RealTimePrototype.API.Controllers;
using RealTimePrototype.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies();

//builder.Services.AddOpenApi();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

app.MapControllers("api");

app.Run();
