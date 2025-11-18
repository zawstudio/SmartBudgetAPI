using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetAPI.Application.DTOs.Auth;
using SmartBudgetAPI.Application.DTOs.Common;
using SmartBudgetAPI.Application.Features.Auth.Commands.Login;
using SmartBudgetAPI.Application.Features.Auth.Commands.Register;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartBudgetAPI.Controllers;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register new user", Description = "Creates a new user account")]
    [SwaggerResponse(200, "User registered successfully", typeof(ApiResponse<AuthResponseDto>))]
    [SwaggerResponse(400, "Invalid input data")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var command = new RegisterCommand(registerDto);
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "User registered successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.FailureResponse(ex.Message));
        }
    }

    /// <summary>
    /// User login
    /// </summary>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login user", Description = "Authenticates user and returns JWT token")]
    [SwaggerResponse(200, "Login successful", typeof(ApiResponse<AuthResponseDto>))]
    [SwaggerResponse(401, "Invalid credentials")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var command = new LoginCommand(loginDto);
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Login successful"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.FailureResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.FailureResponse(ex.Message));
        }
    }
}

