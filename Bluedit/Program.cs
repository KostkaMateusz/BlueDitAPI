using Bluedit.StartUpExtensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//[*] Database Config
builder.AddDataBaseContext();

//[*] AutoMaper Config
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//[*] Authentication Setup
builder.ConfigureAuthentication();

builder.AddServices();


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