using AutoMapper;
using Bluedit.Application.Features.TopicFeatures.Commands.CreateTopic;
using Bluedit.Application.Features.TopicFeatures.Queries.TopicExist;
using Bluedit.Domain.Entities;
using Bluedit.Helpers.DataShaping;
using Bluedit.Helpers.Pagination;
using Bluedit.Models.DataModels.TopicDtos;
using Bluedit.Services.Repositories.TopicRepo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace Bluedit.Controllers.TopicRelated;

[ApiController]
[Route("api/topics")]
public class TopicController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;
    private readonly IPropertyCheckerService _propertyCheckerService;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public TopicController(IMediator mediator,ITopicRepository topicRepository, IMapper mapper, IPropertyCheckerService propertyCheckerService, ProblemDetailsFactory problemDetailsFactory)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
    }

    private string? CreateTopicResourceUri(TopicResourceParameters topicResourceParameters, ResourceUriType type)
    {
        var pageNumber = type switch
        {
            ResourceUriType.PreviousPage => topicResourceParameters.PageNumber - 1,
            ResourceUriType.NextPage => topicResourceParameters.PageNumber + 1,
            _ => topicResourceParameters.PageNumber
        };

        var uri = Url.Link("GetTopics",
            new
            {
                pageNumber,
                pageSize = topicResourceParameters.PageSize,
                mainCategory = topicResourceParameters.TopicName,
                searchQuery = topicResourceParameters.SearchQuery
            });

        return uri;
    }


    /// <summary>
    /// Create Topics for posts
    /// </summary>
    /// <param name="topicCreate">Object Model required to create a topic</param>
    /// <returns>Action Result of type TopicCreateDto</returns>        
    /// <response code="409">When Topic with given name Already Exist</response>
    /// <response code="201">When Topic was created</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TopicCreateDto>> CreateTopic([FromBody] CreateTopicCommand topicCreate)
    {        
        var topicExit = await _mediator.Send(new TopicExistQuery(topicCreate.TopicName));

        if (topicExit is true)
            return Conflict("Topic with given name Already Exist");

        var newTopic = await _mediator.Send(topicCreate);

        return CreatedAtRoute("GetTopic", new { topicName = newTopic.TopicName }, topicCreate);
    }

    /// <summary>
    /// Get topic details by its name
    /// </summary>
    /// <param name="topicName">String with topic unique name</param>
    /// <returns>Action Result of type TopicInfoDto</returns>
    /// <response code="200">When Topic is present</response>
    /// <response code="404">When Topic does not exist</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{topicName}", Name = "GetTopic")]
    public async Task<ActionResult<TopicInfoDto>> GetTopic([FromRoute] string topicName)
    {
        var topic = await _topicRepository.GetTopicWithNameAsync(topicName);

        if (topic is null)
            return NotFound();

        var topicInfoDto = _mapper.Map<TopicInfoDto>(topic);

        topicInfoDto.PostCount = await _topicRepository.GetTopicPostsCountAsync(topicName);

        return Ok(topicInfoDto);
    }


    /// <summary>
    /// Delete given topic with all its posts
    /// </summary>
    /// <param name="topicName">String with topic unique name</param>
    /// <returns>No content</returns>
    /// <response code="404">When Topic does not exist</response>
    /// <response code="204">When Topic was deleted</response>
    /// <response code="403">When User is not authorized</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{topicName}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteTopic([FromRoute] string topicName)
    {
        var topicToDelete = await _topicRepository.GetTopicWithNameAsync(topicName);

        if (topicToDelete is null)
            return NotFound();

        _topicRepository.DeleteTopicAsync(topicToDelete);
        await _topicRepository.SaveChangesAsync();

        return NoContent();
    }

    ///// <summary>
    ///// Get list of all topics
    ///// </summary>
    ///// <param name="topicResourceParameters">Object with query parameters</param>
    ///// <response code="400">When Sorting or DataShaping fields are not valid</response>
    ///// <response code="200">When list of topics is returned</response>
    ///// <returns>Action Results</returns>
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[HttpHead]
    //[HttpGet(Name = "GetTopics")]
    //public async Task<ActionResult<IEnumerable<TopicInfoDto>>> GetTopicsAsync([FromQuery] TopicResourceParameters topicResourceParameters)
    //{
    //    // check if requested fields for data shape are valid
    //    if (_propertyCheckerService.TypeHasProperties<TopicInfoDto>(topicResourceParameters.Fields) is false)
    //    {
    //        var problemWithFields = _problemDetailsFactory.CreateProblemDetails(HttpContext, statusCode: 400,
    //            detail: $"Not all requested data shaping fields exist on the resource: {topicResourceParameters.Fields}");

    //        return BadRequest(problemWithFields);
    //    }

    //    // check if requested fields for sorting are valid
    //    if (_propertyCheckerService.TypeHasProperties<TopicInfoDto>(topicResourceParameters.OrderBy) is false)
    //    {
    //        var problemWithFields = _problemDetailsFactory.CreateProblemDetails(HttpContext, statusCode: 400,
    //            detail: $"Not all requested sorting fields exist on the resource: {topicResourceParameters.OrderBy}");

    //        return BadRequest(problemWithFields);
    //    }

    //    // get topic from repo
    //    var topicsFromRepo = await _topicRepository.GetAllTopicAsync(topicResourceParameters);

    //    //calculate prev site if exist
    //    var previousPageLink = topicsFromRepo.HasPrevious ?
    //        CreateTopicResourceUri(topicResourceParameters, ResourceUriType.PreviousPage) : null;
    //    //calculate next site if exist
    //    var nextPageLink = topicsFromRepo.HasNext ?
    //        CreateTopicResourceUri(topicResourceParameters, ResourceUriType.NextPage) : null;
    //    //include pagination metadata
    //    var paginationMetadata = new PaginationMetaData<Topic>(topicsFromRepo, previousPageLink, nextPageLink);

    //    Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
    //    //map topics to DTO
    //    var topicInfoDtos = _mapper.Map<IEnumerable<TopicInfoDto>>(topicsFromRepo);

    //    //shape data
    //    var shapedTopicInfoDtos = topicInfoDtos.ShapeData(topicResourceParameters.Fields);

    //    return Ok(shapedTopicInfoDtos);
    //}

    [HttpOptions]
    public ActionResult GetAuthorsOptions()
    {
        Response.Headers.Append("Allow", "GET,HEAD,POST,DELETE,PATCH,OPTIONS");
        return Ok();
    }

    /// <summary>
    /// Partually Update Given Topic
    /// </summary>
    /// <param name="topicName">String with topic unique name</param>
    /// <param name="patchDocument">Json PATCH Document</param>
    /// <returns>No content</returns>
    /// <response code="404">When Topic does not exist</response>
    /// <response code="400">When There is data validation problem</response>
    /// <response code="200">When Topic was edited</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPatch("{topicName}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PartiallyUpdateTopicDescription([FromRoute] string topicName, [FromBody] JsonPatchDocument<TopicForUpdateDto> patchDocument)
    {
        var topicFromRepo = await _topicRepository.GetTopicWithNameAsync(topicName);

        if (topicFromRepo is null)
            return NotFound();

        var topicToPatch = _mapper.Map<TopicForUpdateDto>(topicFromRepo);

        patchDocument.ApplyTo(topicToPatch, ModelState);

        if (!TryValidateModel(topicToPatch))
            return ValidationProblem(ModelState);

        _mapper.Map(topicToPatch, topicFromRepo);

        _topicRepository.UpdateTopicAync(topicFromRepo);

        await _topicRepository.SaveChangesAsync();

        return Ok();
    }
}