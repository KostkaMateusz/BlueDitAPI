using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Application.Features.LikeFeature.Commands.CreateLike;
using Bluedit.Application.Features.LikeFeature.Commands.DeleteLike;
using Bluedit.Application.Features.LikeFeature.Queries.GetLikesWithUser;
using Bluedit.Domain.Entities.LikeEntities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bluedit.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services
            .AddTransient<IRequestHandler<GetLikesWithUserQuery<PostLike>,List<LikesDto>>,GetLikesWithUserQueryHandler<PostLike>>()
            .AddTransient<IRequestHandler<GetLikesWithUserQuery<ReplyLike>,List<LikesDto>>,GetLikesWithUserQueryHandler<ReplyLike>>();

        services
            .AddTransient<IRequestHandler<CreateLikeRequest<PostLike>, LikesDto>, CreateLikeRequestHandler<PostLike>>()
            .AddTransient<IRequestHandler<CreateLikeRequest<ReplyLike>, LikesDto>, CreateLikeRequestHandler<ReplyLike>>();
         
        services
            .AddTransient<IRequestHandler<DeleteLikeRequest<PostLike>>, DeleteLikeRequestHandler<PostLike>>()
            .AddTransient<IRequestHandler<DeleteLikeRequest<ReplyLike>>, DeleteLikeRequestHandler<ReplyLike>>();

        
        return services;
    }
}