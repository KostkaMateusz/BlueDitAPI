using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Application.Exceptions;
using Bluedit.Domain.Entities.LikeEntities;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Commands.CreateLike;

internal sealed class CreateLikeRequestHandler<T> : IRequestHandler<CreateLikeRequest<T>,LikesDto>  where T : LikeBase, new()
{
    private readonly ILikesRepository<T> _likeRepository;
    private readonly IPostRepository _postRepository;
    private readonly IRepliesRepository _repliesRepository;
    private readonly IMapper _mapper;
    
    public CreateLikeRequestHandler(IMapper mapper, ILikesRepository<T> likeRepository, IPostRepository postRepository, IRepliesRepository repliesRepository)
    {
        _likeRepository = likeRepository;
        _postRepository = postRepository;
        _repliesRepository = repliesRepository;
        _mapper = mapper;
    }
    
    public async Task<LikesDto> Handle(CreateLikeRequest<T> request, CancellationToken cancellationToken)
    {
        var likeExist=await _likeRepository.CheckIfLikeExistAsync(request.UserId,request.ParentId);
        if (likeExist)
            throw new ConflictException();

        
        var postExsit= await _postRepository.PostWithGivenIdExistAsync(request.ParentId);
        var replyExist = await _repliesRepository.ReplyWithGivenIdExistAsync(request.ParentId);

        if (!postExsit && !replyExist)
            throw new NotFoundException("Resource not found");
        
        var newLike = new T() { UserId =request.UserId, ParentId = request.ParentId };
        
        await _likeRepository.AddLikeAsync(newLike);
        await _likeRepository.SaveChangesAsync();
        
        var likesDto = _mapper.Map<LikesDto>(newLike);

        return likesDto;
    }
}