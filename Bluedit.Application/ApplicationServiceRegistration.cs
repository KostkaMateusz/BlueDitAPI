using Bluedit.Application.DataModels.LikesDto;
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

        services.AddTransient<IRequestHandler<GetLikesWithUserQuery<PostLike>,List<LikesDto>>,GetLikesWithUserQueryHandler<PostLike>>() ;
        services.AddTransient<IRequestHandler<GetLikesWithUserQuery<ReplyLike>,List<LikesDto>>,GetLikesWithUserQueryHandler<ReplyLike>>();
        
        return services;
    }
}