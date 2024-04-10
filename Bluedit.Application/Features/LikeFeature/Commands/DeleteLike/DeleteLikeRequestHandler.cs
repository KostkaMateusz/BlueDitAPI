using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.Exceptions;
using Bluedit.Domain.Entities.LikeEntities;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Commands.DeleteLike;

internal sealed class DeleteLikeRequestHandler<T> : IRequestHandler<DeleteLikeRequest<T>>  where T : LikeBase, new()
{
    private readonly ILikesRepository<T> _likeRepository;
    
    public DeleteLikeRequestHandler( ILikesRepository<T> likeRepository)
    {
        _likeRepository = likeRepository;
    }
    
    public async Task Handle(DeleteLikeRequest<T> request, CancellationToken cancellationToken)
    {
        var like = await _likeRepository.GetLike(request.ParentId,request.UserId );

        if (like is null)
            throw new NotFoundException("Like not found");

        _likeRepository.DeleteLike(like);
        await _likeRepository.SaveChangesAsync();
    }
}