﻿using AutoMapper;
using Bluedit.Domain.Entities;
using Bluedit.Infrastructure.StorageService;
using Bluedit.Models.DataModels.PostDtos;
using Bluedit.Services.Authentication;
using Bluedit.Services.Repositories.PostRepo;
using Bluedit.Services.Repositories.ReplyRepo;
using Bluedit.Services.Repositories.TopicRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Bluedit.Controllers.PostRelated;


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
    private readonly IRepliesRepository _repliesRepository;

    public PostController(IPostRepository postRepository, ITopicRepository topicRepository, IRepliesRepository repliesRepository, IAzureStorageService azureStorageService, IUserContextService userContextService, IMapper mapper)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _azureStorageService = azureStorageService ?? throw new ArgumentNullException(nameof(azureStorageService));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
        _repliesRepository = repliesRepository ?? throw new ArgumentNullException(nameof(repliesRepository));
    }

    private string? GetLinkToImage(string topicName, Guid postId)
    {
        const string imageFuncName = "GetImage";
        var navigationParametersObject = new { topicName, postId };

        return Url.Link(imageFuncName, navigationParametersObject);
    }

    /// <summary>
    /// Create New Post
    /// </summary>
    /// <param name="topicName">Topic where new post will be creted</param>
    /// <param name="postCreateDto">Post data</param>
    /// <response code="404">When parent topic is not found</response>
    /// <response code="201">When resource is created</response>
    /// <returns>Action Results</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<PostInfoDto>> CreatePost([FromRoute] string topicName, [FromForm] PostCreateDto postCreateDto)
    {
        var topicExist = await _topicRepository.GetTopicWithNameAsync(topicName);
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

        await _postRepository.AddPostAsync(post);
        await _postRepository.SaveChangesAsync();
        await _postRepository.LoadPostUserAsync(post);

        var postDto = _mapper.Map<PostInfoDto>(post);
        postDto.ImageContentLink = GetLinkToImage(topicName, post.PostId);

        return CreatedAtRoute("GetPostInfo", new { topicName, post.PostId }, postDto);
    }

    /// <summary>
    /// Get Posts by Topic Name
    /// </summary>
    /// <param name="topicName">Topic where new post will be creted</param>
    /// <response code="404">When topic is not found</response>
    /// <response code="200">When resource present</response>
    /// <returns>Action Results</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Get Post Details
    /// </summary>
    /// <param name="topicName">Topic where new post will be creted</param>
    /// <param name="postId">Post Id</param>
    /// <response code="404">When topic is not found</response>
    /// <response code="200">When resource present</response>
    /// <returns>Action Results</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{postId}", Name = "GetPostInfo")]
    [AllowAnonymous]
    public async Task<ActionResult<PostInfoDto>> GetPostInfo([FromRoute] string topicName, [FromRoute] Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        await _postRepository.LoadPostUserAsync(post);

        var postDto = _mapper.Map<PostInfoDto>(post);

        postDto.ImageContentLink = GetLinkToImage(topicName, postId);

        return Ok(postDto);
    }

    /// <summary>
    /// Get Post Image
    /// </summary>
    /// <param name="postId">Post Id</param>
    /// <response code="404">When Image is not found</response>
    /// <response code="200">When resource present</response>
    /// <returns>Action Results</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Update Post
    /// </summary>
    /// <param name="postId">Post Id</param>
    /// <param name="topicName">Topic Name</param>
    /// <param name="postUpdateDto">Data for post update</param>
    /// <response code="404">When Image is not found</response>
    /// <response code="403">When access is not authorised</response>
    /// <response code="200">When resource present</response>
    /// <returns>Action Results</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut("{postId}")]
    public async Task<ActionResult<PostInfoDto>> UpdatePost([FromRoute] string topicName, [FromRoute] Guid postId, [FromForm] PostUpdateDto postUpdateDto)
    {
        var postForUpdateFromRepo = await _postRepository.GetPostByIdAsync(postId);

        if (postForUpdateFromRepo is null)
            return NotFound();

        if (_userContextService.GetUserId != postForUpdateFromRepo.UserId)
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
        var postDto = _mapper.Map<PostInfoDto>(postForUpdateFromRepo);
        postDto.ImageContentLink = GetLinkToImage(topicName, postForUpdateFromRepo.PostId);

        return Ok(postDto);
    }



    /// <summary>
    /// Partialy Update Given Topic
    /// </summary>
    /// <param name="postId">Post Id</param>
    /// <param name="patchDocument">Json PATCH Document</param>
    /// <returns>Action Result</returns>
    /// <response code="404">When Topic does not exist</response>
    /// <response code="400">When There is data validation problem</response>
    /// <response code="403">When access is not authorised</response>
    /// <response code="200">When Topic was edited</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPatch("{postId}")]
    public async Task<IActionResult> PartialyUpdatePost([FromRoute] Guid postId, [FromBody] JsonPatchDocument<PartialyUpdatePostDto> patchDocument)
    {
        var postFromRepo = await _postRepository.GetPostByIdAsync(postId);

        if (postFromRepo is null)
            return NotFound();

        if (_userContextService.GetUserId != postFromRepo.UserId)
            return Forbid("You are not an owner of this resource");

        var postToPatch = _mapper.Map<PartialyUpdatePostDto>(postFromRepo);

        patchDocument.ApplyTo(postToPatch, ModelState);

        if (!TryValidateModel(postToPatch))
            return ValidationProblem(ModelState);

        _mapper.Map(postToPatch, postFromRepo);

        _postRepository.UpdatePost(postFromRepo);

        await _postRepository.SaveChangesAsync();

        return Ok();
    }


    /// <summary>
    /// Delete Post
    /// </summary>
    /// <param name="postId">Post Id</param>
    /// <response code="404">When Post is not found</response>
    /// <response code="403">When access is not authorised</response>
    /// <response code="204">When resource were sucesfully deleted</response>
    /// <returns>Action Results</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("{postId}")]
    public async Task<ActionResult> DeletePost([FromRoute] Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return NotFound();

        var postReplies = (await _postRepository.LoadPostRepliesAsync(post)).Reply;

        if (_userContextService.GetUserId != post.UserId)
            return Forbid("You are not an owner of this resource");

        foreach (var postReply in postReplies)
        {
            await _repliesRepository.DeleteReplyTree(postReply);
        }

        _postRepository.DeletePost(post);

        await _postRepository.SaveChangesAsync();

        return NoContent();
    }

}