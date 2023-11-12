using Bluedit.Helpers.DataShaping;
using Bluedit.Helpers.Sorting;
using Bluedit.Models.DataModels.UserDtos;
using Bluedit.Models.ModelsValidators;
using Bluedit.Services.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;
using Bluedit.Domain.Entities;
using Bluedit.Services.Repositories.PostRepo;
using Bluedit.Services.Repositories.LikeRepo;
using Bluedit.Services.Repositories.ReplyRepo;
using Bluedit.Services.Repositories.TopicRepo;
using Bluedit.Services.Repositories.UserRepo;
using Bluedit.Infrastructure;
using Bluedit.Domain.Entities.LikeEntities;

namespace Bluedit.StartUpExtensions;

internal static class AppConfigHelperExtensions
{
    public static WebApplicationBuilder AddDataBaseContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BlueditDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbContextConnectionString")));

        return builder;
    }

    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var authenticationSettings = new AuthenticationSettings();
        builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
        builder.Services.AddSingleton(authenticationSettings);

        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = "Bearer";
            option.DefaultScheme = "Bearer";
            option.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = authenticationSettings.JwtIssuer,
                ValidAudience = authenticationSettings.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
            };
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureSwaggerDoc(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "BlueditApi", Version = "v1", Description = "This project is to create small backend service for social media service like Redit in ASP.NET" });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.AddSecurityDefinition("PostGramApiBearerAuth", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Input a valid token to access this API"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "PostGramApiBearerAuth" }
                    }, new List<string>() 
                }                
            });
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontEndClient", policyBuilder => policyBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedHosts"]));
        });

        return builder;
    }

    public static WebApplicationBuilder AddControllersConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(configure =>
        {
            configure.ReturnHttpNotAcceptable = true;
        })
            .AddNewtonsoftJson(setupAction =>
        {
            setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        });

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserContextService, UserContextService>();

        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

        builder.Services.AddScoped<ITopicRepository, TopicRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<IRepliesRepository, RepliesRepository>();

        builder.Services.AddScoped<ILikesRepository<PostLike>, LikesRepository<PostLike>>();
        builder.Services.AddScoped<ILikesRepository<ReplyLike>, LikesRepository<ReplyLike>>();

        //Add Azure Blob Service
        builder.Services.AddInfrastructureServices();

        //data shaping
        builder.Services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();
        //sorting
        builder.Services.AddTransient<IPropertyMappingService,PropertyMappingService>();

        return builder;
    }
}