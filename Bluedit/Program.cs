using Bluedit.StartUpExtensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddControllersConfiguration();

//[*] Database Config
builder.AddDataBaseContext();

//[*] AutoMaper Config
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//[*] Authentication Setup
builder.ConfigureAuthentication();

builder.AddServices();

builder.ConfigureSwaggerDoc();

builder.ConfigureCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseHttpsRedirection();

//Cors
app.UseCors("FrontEndClient");

app.UseAuthorization();

app.MapControllers();

app.Run();