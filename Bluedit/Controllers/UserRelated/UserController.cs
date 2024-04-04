using AutoMapper;
using Bluedit.Domain.Entities;
using Bluedit.Services.Authentication;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bluedit.Application.DataModels.UserDtos;
using Bluedit.Persistence.Repositories.UserRepo;

namespace Bluedit.Controllers.UserRelated;


[ApiController]
[Route("api/account")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IValidator<RegisterUserDto> _registerUserDtoValidator;

    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserController(
        IValidator<RegisterUserDto> registerUserDtoValidator, IUserRepository userRepository, IMapper mapper,
        IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IUserContextService userContextService)
    {
        _registerUserDtoValidator = registerUserDtoValidator;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _userContextService = userContextService;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    /// <summary>
    ///  Return bearer token for auth
    /// </summary>
    /// <returns>bearer token</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST account/login
    ///     {
    ///        "Email":"testmail1@gmail.com",
    ///        "Password":"password1"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns auth token</response>
    /// <response code="400">If the email or password is wrong</response>
    /// <param name="loginUserDto">Login Info</param>   
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequest))]
    [Produces("text/plain")]
    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserDto loginUserDto)
    {
        var user = await _userRepository.GetUserByMail(loginUserDto.Email);

        if (user is null)
            return BadRequest("Invalid User Name or Password");

        if (user.PasswordHash is null)
            return BadRequest("Invalid User Name or Password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);

        if (result == PasswordVerificationResult.Failed)
            return BadRequest("Invalid User Name or Password");

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer,
            claims, expires: expires, signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenString = tokenHandler.WriteToken(token);

        return Ok(tokenString);
    }


    /// <summary>
    /// Create new user
    /// </summary>
    /// <returns>status code</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST account
    ///     {
    ///         "Name":"User1",
    ///         "Email":"testmail@gmail.com",
    ///         "Password":"password1"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">If user is created</response>
    /// <response code="400">If the email or password is wrong or email is already taken</response>
    /// <param name="registerUserDto"></param>   
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblem))]
    [AllowAnonymous]
    [HttpPost(Name = "RegisterUser")]
    public async Task<ActionResult<UserInfoDto>> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        var validationResult = await _registerUserDtoValidator.ValidateAsync(registerUserDto);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem();
        }

        var newUser = new User()
        {
            Name = registerUserDto.Name,
            Email = registerUserDto.Email
        };

        var hashedPassword = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
        newUser.PasswordHash = hashedPassword;

        await _userRepository.AddNewUser(newUser);
        await _userRepository.SaveChangesAsync();

        return CreatedAtRoute("UserInfo", _mapper.Map<UserInfoDto>(newUser));
    }


    /// <summary>
    ///  Get information about user
    /// </summary>
    /// <returns>Action result of type UserInfoDto</returns>
    /// <response code="401">If user is unauthenticated</response>
    /// <response code="200">If user is authenticated</response>   
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpGet(Name = "UserInfo")]
    public async Task<ActionResult<UserInfoDto>> UserInfo()
    {
        var user = await _userRepository.GetUserById(_userContextService.GetUserId);
        var userInfo = _mapper.Map<UserInfoDto>(user);

        return Ok(userInfo);
    }


    /// <summary>
    ///  Allow user to update name, mail and password in the service
    /// </summary>
    /// <returns>status code</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /account
    ///     {
    ///         "Name":"User1",
    ///         "Email":"testmail@gmail.com",
    ///         "Password":"password1"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">If update user is possible</response>
    /// <response code="400">If update user casued validation error</response>
    /// <response code="401">If user is unauthenticated</response>
    /// <param name="updateUserDto"></param>  
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblem))]
    [Authorize]
    [HttpPut(Name = "UpdateUser")]
    public async Task<ActionResult<UpdateUserDto>> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        var validationResult = await _registerUserDtoValidator.ValidateAsync(updateUserDto);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem();
        }

        var user = await _userRepository.GetUserById(_userContextService.GetUserId);

        if (user is null)
            return Unauthorized();

        user.Name = updateUserDto.Name;
        user.Email = updateUserDto.Email;

        var hashedPassword = _passwordHasher.HashPassword(user, updateUserDto.Password);
        user.PasswordHash = hashedPassword;

        await _userRepository.SaveChangesAsync();

        return Ok(updateUserDto);
    }

    /// <summary>
    ///  Delete current user
    /// </summary>
    /// <returns>Action result</returns>
    /// <response code="401">If user is unauthenticated</response>
    /// <response code="204">If user was deleted</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpDelete()]
    public async Task<ActionResult> DeleteUser()
    {
        var user = await _userRepository.GetUserAndAllRelatedEntities(_userContextService.GetUserId);

        //foreach (var post in user.Posts)
        //    _fileService.DeleteImage(post.ImageGuid);

        _userRepository.DeleteUser(user);
        await _userRepository.SaveChangesAsync();

        return StatusCode(StatusCodes.Status204NoContent);
    }
}