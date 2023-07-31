using AutoMapper;
using Bluedit.Entities;
using Bluedit.Models.DataModels.PostDtos;
using Bluedit.Services.Authentication;
using Bluedit.Services.Repositories;
using Bluedit.Services.StorageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/{topic}/posts")]
[Authorize]
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
    public async Task<ActionResult<PostInfoDto>> CreatePost([FromRoute] string topic, [FromForm] PostCreateDto postCreateDto)
    {
        //Save file to storage
        var imageNameGuid = Guid.NewGuid();
        await _azureStorageService.SaveFile(imageNameGuid, postCreateDto.image);

        var post = new Post()
        {
            Title = postCreateDto.Title,
            Description = postCreateDto.Description,
            UserId = _userContextService.GetUserId,
            ImageGuid = imageNameGuid,
            TopicName = topic
        };

        await _postRepository.AddPost(post);
        await _postRepository.SaveChangesAsync();
        await _postRepository.LoadPostUser(post);
        var postDto = _mapper.Map<PostInfoDto>(post);
        postDto.ImageContentLink = GetLinkToImage(topic, post.PostId);

        return CreatedAtRoute("GetPostInfo", new { topic, post.PostId }, postDto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PostInfoDto>>> GetAllMainTopicPosts([FromRoute] string topic)
    {
        var postsByTopic = await _postRepository.GetAllPostsByTopicAsync(topic);

        if (postsByTopic.IsNullOrEmpty() is true)
            return NotFound();

        var postsDtos = _mapper.Map<IEnumerable<PostInfoDto>>(postsByTopic);

        foreach (var postDto in postsDtos)
        {
            postDto.ImageContentLink = GetLinkToImage(topic, postDto.PostId);
        }

        return Ok(postsDtos);
    }


    [HttpGet("{postId}", Name = "GetPostInfo")]
    [AllowAnonymous]
    public async Task<ActionResult<PostInfoDto>> GetPostInfo([FromRoute] string topic, [FromRoute] Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        await _postRepository.LoadPostUser(post);

        var postDto = _mapper.Map<PostInfoDto>(post);

        postDto.ImageContentLink = GetLinkToImage(topic, postId);

        return Ok(postDto);
    }

    private string GetLinkToImage(string topic, Guid postId)
    {
        const string imageFuncName = "GetImage";
        var navigationParametersObject = new { topic, postId };

        return Url.Link(imageFuncName, navigationParametersObject);
    }


    [HttpGet("{postId}/image", Name = "GetImage")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImage([FromRoute] Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        var imageData = await _azureStorageService.GetFileData(post.ImageGuid);

        var contentProvider = new FileExtensionContentTypeProvider();

        contentProvider.TryGetContentType(post.ImageGuid.ToString(), out string? contentType);

        if (contentType is null)
            contentType = "image/jpeg";

        return File(imageData, contentType);
    }

    [HttpDelete("{postId}")]
    public async Task<ActionResult> DeletePost([FromRoute] Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{postId}")]
    public async Task<ActionResult<PostInfoDto>> UpdatePost([FromRoute] string topic, [FromRoute] Guid postId, [FromForm] PostUpdateDto postUpdateDto)
    {
        var postForUpdateFromRepo=await _postRepository.GetPostByIdAsync(postId);
        
        if(postForUpdateFromRepo is null)
            return NotFound();

        if(_userContextService.GetUserId != postForUpdateFromRepo.UserId)
            return Forbid("You are not an owner of this resource");

        //Save new image to storage
        var imageNameGuid = Guid.NewGuid();
        await _azureStorageService.SaveFile(imageNameGuid, postUpdateDto.image);
        //delete old file
        await _azureStorageService.DeleteImage(postForUpdateFromRepo.ImageGuid);
        
        // update edited field
        postForUpdateFromRepo.UpdateDate = DateTime.Now;

        //set new image GUID
        postForUpdateFromRepo.ImageGuid = imageNameGuid;
        
        // map dto to Post
        _mapper.Map(postUpdateDto, postForUpdateFromRepo);

        //update and save Post
        _postRepository.UpdatePost(postForUpdateFromRepo);
        await _postRepository.SaveChangesAsync();

        // create external dto
        var postDto=_mapper.Map<PostInfoDto>(postForUpdateFromRepo);
        postDto.ImageContentLink = GetLinkToImage(topic, postForUpdateFromRepo.PostId);

        return Ok(postDto);
    }
        
    //[HttpPatch]
    //public async Task<ActionResult> PartialyUpdatePost([FromForm] JsonPatchDocument)
    //{
        
    //}
    
    // Get Post Replies
    // Create Post Replay

}
