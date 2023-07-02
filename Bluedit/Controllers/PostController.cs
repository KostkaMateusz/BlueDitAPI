using AutoMapper;
using Bluedit.Entities;
using Bluedit.Models.DataModels.PostDtos;
using Bluedit.Services.Authentication;
using Bluedit.Services.Repositories;
using Bluedit.Services.StorageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/{topic}/posts")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IAzureStorageService _azureStorageService;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;

    public PostController(IPostRepository postRepository, IAzureStorageService azureStorageService, IUserContextService userContextService, IMapper mapper)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _azureStorageService = azureStorageService ?? throw new ArgumentNullException(nameof(azureStorageService));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreatePost([FromRoute]string topic,[FromForm] PostCreateDto postCreateDto)
    {
        //Save file to storage
        var imageNameGuid= Guid.NewGuid();
        await _azureStorageService.SaveFile(imageNameGuid, postCreateDto.image);

        var post = new Post()
        {
            Title = postCreateDto.Title,
            Description = postCreateDto.Description,
            UserId = _userContextService.GetUserId,
            ImageGuid=imageNameGuid,
            TopicName = topic
        };

        await _postRepository.AddPost(post);
        await _postRepository.SaveChangesAsync();
        await _postRepository.LoadPostUser(post);
        var postDto = _mapper.Map<PostInfoDto>(post);
        postDto.ImageContentLink = GetLinkToImage(topic, post.PostId);

        return CreatedAtRoute("GetPostInfo", new { topic, post.PostId }, postDto);
    }    

    [HttpGet("{postId}",Name = "GetPostInfo")]
    public async Task<ActionResult<PostInfoDto>> GetPostInfo([FromRoute] string topic,[FromRoute] Guid postId)
    {
        var post=await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        await _postRepository.LoadPostUser(post);

        var postDto=_mapper.Map<PostInfoDto>(post);

        postDto.ImageContentLink = GetLinkToImage(topic, postId);

        return Ok(postDto);
    }

    private string GetLinkToImage(string topic, Guid postId)
    {
        const string imageFuncName = "GetImage";
        var navigationParametersObject = new { topic, postId };

        return Url.Link(imageFuncName, navigationParametersObject);
    }


    [HttpGet("{postId}/image",Name = "GetImage")]
    public async Task<IActionResult> GetImage([FromRoute] Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if(post is null)
            return NotFound();

        var imageData=await _azureStorageService.GetFileData(post.ImageGuid);

        var contentProvider = new FileExtensionContentTypeProvider();

        contentProvider.TryGetContentType(post.ImageGuid.ToString(), out string? contentType);

        if(contentType is null)
            contentType = "image/jpeg";

        return File(imageData,contentType);
    }

    [HttpDelete("{postId}")]
    public async Task<ActionResult> DeletePost([FromRoute] Guid postId)
    {
        var post=await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        return NoContent();
    }

    //  Admin PUT
    //  Patch
    // Get All with Search Query Fields 
    // Get Post Replies

}
