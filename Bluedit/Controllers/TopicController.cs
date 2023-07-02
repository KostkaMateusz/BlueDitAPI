using AutoMapper;
using Bluedit.Entities;
using Bluedit.Helpers.Pagination;
using Bluedit.Models.DataModels.TopicDtos;
using Bluedit.ResourceParameters;
using Bluedit.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Bluedit.Controllers;

[ApiController]
[Route("api/topics")]
public class TopicController : ControllerBase
{
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;

    public TopicController(ITopicRepository topicRepository, IMapper mapper)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    [HttpPost]
    [Authorize(Roles ="Admin")]
    public async Task<ActionResult<TopicCreateDto>> CreateTopic([FromBody] TopicCreateDto topicCreateDto)
    {
        var topicExit=await _topicRepository.IsTopicExistAsync(topicCreateDto.TopicName);

        if(topicExit is true)                    
            return Conflict("Topic with given name Already Exist");        

        var topicEntity = new Topic { TopicName = topicCreateDto.TopicName, TopicDescription = topicCreateDto.TopicDescription };


        await _topicRepository.CreateTopicAync(topicEntity);
        await _topicRepository.SaveChangesAsync();

        return CreatedAtRoute("GetTopic", new {topicName= topicCreateDto.TopicName },topicCreateDto);
    }


    [HttpGet("{topicName}",Name ="GetTopic")]
    public async Task<ActionResult<TopicInfoDto>> GetTopic([FromRoute] string topicName) 
    {
        var topic = await _topicRepository.GetTopicWithNameAsync(topicName);

        if (topic is null)
            return NotFound();

        var topicInfoDto = _mapper.Map<TopicInfoDto>(topic);

        topicInfoDto.PostCount= await _topicRepository.GetTopicPostsCountAsync(topicName);

        return Ok(topicInfoDto);
    }

    [HttpDelete("{topicName}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteTopic([FromRoute] string topicName)
    {
        throw new NotImplementedException();
    }

    [HttpGet(Name ="GetTopics")]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<TopicInfoDto>>> GetTopicAsync([FromQuery] TopicResourceParameters topicResourceParameters)
    {
        // get topic from repo
        var topicsFromRepo =await _topicRepository.GetAllTopicAsync(topicResourceParameters);

        //calculate prev site
        var previousPageLink = topicsFromRepo.HasPrevious ? 
            CreateTopicResourceUri(topicResourceParameters, ResourceUriType.PreviousPage) : null;

        var nextPageLink = topicsFromRepo.HasNext ?
            CreateTopicResourceUri(topicResourceParameters, ResourceUriType.NextPage) : null;

        var paginationMetadata = new PaginationMetaData<Topic>(topicsFromRepo, previousPageLink, nextPageLink);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

        var topicInfoDtos = _mapper.Map<IEnumerable<TopicInfoDto>>(topicsFromRepo);

        foreach (var topicDto in topicInfoDtos) 
        {
            topicDto.PostCount = await _topicRepository.GetTopicPostsCountAsync(topicDto.TopicName);   
        }

        return Ok(topicInfoDtos);
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

    [HttpOptions]
    public ActionResult GetAuthorsOptions()
    {
        Response.Headers.Add("Allow", "GET,HEAD,POST,DELETE,PATCH,OPTIONS");
        return Ok();
    }

    [HttpPatch("{topicName}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PartiallyUpdateTopicDescription([FromRoute] string topicName,[FromBody] JsonPatchDocument<TopicForUpdateDto> patchDocument)
    {
        var topicFromRepo= await _topicRepository.GetTopicWithNameAsync(topicName);

        if (topicFromRepo is null)
            return NotFound();

        var topicToPatch = _mapper.Map<TopicForUpdateDto>(topicFromRepo);

        patchDocument.ApplyTo(topicToPatch, ModelState);

        if (!TryValidateModel(topicToPatch))
            return ValidationProblem(ModelState);

        _mapper.Map(topicToPatch, topicFromRepo);

        await _topicRepository.UpdateTopicAync(topicFromRepo);

        await _topicRepository.SaveChangesAsync();

        return Ok();
    }


}

