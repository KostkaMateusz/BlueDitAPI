using AutoMapper;
using Bluedit.Domain.Entities;
using Bluedit.Models.DataModels.PostDtos;
using Bluedit.Services.Authentication;
using Bluedit.Services.Repositories.PostRepo;
using Bluedit.Services.Repositories.TopicRepo;
using Bluedit.Services.StorageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/topics/{topicName}/posts")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IAzureStorageService _azureStorageService;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;
    private readonly ITopicRepository _topicRepository;

    public PostController(IPostRepository postRepository, ITopicRepository topicRepository, IAzureStorageService azureStorageService, IUserContextService userContextService, IMapper mapper)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _azureStorageService = azureStorageService ?? throw new ArgumentNullException(nameof(azureStorageService));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _topicRepository= topicRepository ?? throw new ArgumentNullException( nameof(topicRepository));
    }


    [HttpPost]
    public async Task<ActionResult<PostInfoDto>> CreatePost([FromRoute] string topicName, [FromForm] PostCreateDto postCreateDto)
    {
        var topicExist =await _topicRepository.GetTopicWithNameAsync(topicName);
        if (topicExist is null)
            return NotFound($"Topic with name {topicName} not found");

        //Save file to storage
        var imageNameGuid = Guid.NewGuid();
        await _azureStorageService.SaveFile(imageNameGuid, postCreateDto.image);

        var post = new Post()
        {
            Title = postCreateDto.Title,
            Description = postCreateDto.Description,
            UserId = _userContextService.GetUserId,
            ImageGuid = imageNameGuid,
            TopicName = topicName
        };

        await _postRepository.AddPost(post);
        await _postRepository.SaveChangesAsync();
        await _postRepository.LoadPostUser(post);

        var postDto = _mapper.Map<PostInfoDto>(post);
        postDto.ImageContentLink = GetLinkToImage(topicName, post.PostId);

        return CreatedAtRoute("GetPostInfo", new { topicName, post.PostId }, postDto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PostInfoDto>>> GetAllMainTopicPosts([FromRoute] string topicName)
    {
        var postsByTopic = await _postRepository.GetAllPostsByTopicAsync(topicName);

        if (postsByTopic.IsNullOrEmpty() is true)
            return NotFound();

        var postsDtos = _mapper.Map<IEnumerable<PostInfoDto>>(postsByTopic);

        foreach (var postDto in postsDtos)
        {
            postDto.ImageContentLink = GetLinkToImage(topicName, postDto.PostId);
        }

        return Ok(postsDtos);
    }


    [HttpGet("{postId}", Name = "GetPostInfo")]
    [AllowAnonymous]
    public async Task<ActionResult<PostInfoDto>> GetPostInfo([FromRoute] string topicName, [FromRoute] Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        await _postRepository.LoadPostUser(post);

        var postDto = _mapper.Map<PostInfoDto>(post);

        postDto.ImageContentLink = GetLinkToImage(topicName, postId);

        return Ok(postDto);
    }

    private string GetLinkToImage(string topicName, Guid postId)
    {
        const string imageFuncName = "GetImage";
        var navigationParametersObject = new { topicName, postId };

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

    [HttpPut("{postId}")]
    public async Task<ActionResult<PostInfoDto>> UpdatePost([FromRoute] string topicName, [FromRoute] Guid postId, [FromForm] PostUpdateDto postUpdateDto)
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
        postDto.ImageContentLink = GetLinkToImage(topicName, postForUpdateFromRepo.PostId);

        return Ok(postDto);
    }

    //[HttpPatch]
    //public async Task<ActionResult> PartialyUpdatePost([FromForm] JsonPatchDocument)
    //{

    //}

    // Get Post Replies
    // Create Post Replay

    [HttpDelete("{postId}")]
    public async Task<ActionResult> DeletePost([FromRoute] Guid postId)
    {

        throw new NotImplementedException();
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        return NoContent();
    }


}
