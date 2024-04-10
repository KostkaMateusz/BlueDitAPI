using Bluedit.StartUpExtensions;
using System.Reflection;
using Bluedit.Persistence;
using Bluedit.Application;
using Bluedit.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddControllersConfiguration();

//[*] Database Config
builder.AddPersistenceServices();

//[*] AutoMaper Config
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//[*] Authentication Setup
builder.ConfigureAuthentication();

builder.Services.AddApplicationServices();

builder.AddServices();

builder.ConfigureSwaggerDoc();

builder.ConfigureCors();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
// Configure the HTTP request pipeline.

app.MapGet("/checkEnviroment", () => app.Environment.EnvironmentName);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.EnableTryItOutByDefault();
});

app.UseStaticFiles();

app.UseAuthentication();

app.UseHttpsRedirection();

//Cors
app.UseCors("FrontEndClient");

app.UseAuthorization();

app.MapControllers();

app.Run();