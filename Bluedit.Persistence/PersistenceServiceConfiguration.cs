﻿using Bluedit.Application.Contracts;
using Bluedit.Domain.Entities.LikeEntities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bluedit.Persistence.Helpers.Sorting;
using Bluedit.Persistence.Repositories.LikeRepo;
using Bluedit.Persistence.Repositories.PostRepo;
using Bluedit.Persistence.Repositories.ReplyRepo;
using Bluedit.Persistence.Repositories.TopicRepo;
using Bluedit.Persistence.Repositories.UserRepo;

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

    private static WebApplicationBuilder AddDataBaseContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BlueditDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DbContextConnectionString")));

        return builder;
    }

    private static WebApplicationBuilder AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<IRepliesRepository, RepliesRepository>();

        builder.Services.AddScoped<ILikesRepository<PostLike>, LikesRepository<PostLike>>();
        builder.Services.AddScoped<ILikesRepository<ReplyLike>, LikesRepository<ReplyLike>>();

        builder.Services.AddScoped<ITopicRepository, TopicRepository>();

        //sorting
        builder.Services.AddTransient<IPropertyMappingService, PropertyMappingService>();

        return builder;
    }
}