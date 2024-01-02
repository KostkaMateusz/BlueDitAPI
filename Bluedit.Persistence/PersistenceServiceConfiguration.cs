using Bluedit.Domain.Entities.LikeEntities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Bluedit.Services.Repositories.LikeRepo;
using Bluedit.Services.Repositories.PostRepo;
using Bluedit.Services.Repositories.ReplyRepo;
using Bluedit.Services.Repositories.UserRepo;
//using Bluedit.Services.Repositories.TopicRepo;

namespace Bluedit.Persistence;

public static class PersistenceServiceConfiguration
{
    public static WebApplicationBuilder AddPersistenceServices(this WebApplicationBuilder builder)
    {
        builder.AddDataBaseContext();

        builder.AddRepositories();

        return builder;
    }

    public static WebApplicationBuilder AddDataBaseContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BlueditDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbContextConnectionString")));

        return builder;
    }

    public static WebApplicationBuilder AddRepositories(this WebApplicationBuilder builder)
    {

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<IRepliesRepository, RepliesRepository>();

        builder.Services.AddScoped<ILikesRepository<PostLike>, LikesRepository<PostLike>>();
        builder.Services.AddScoped<ILikesRepository<ReplyLike>, LikesRepository<ReplyLike>>();

        return builder;
    }


}