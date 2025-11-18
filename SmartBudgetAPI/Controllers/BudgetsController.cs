using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetAPI.Application.DTOs.Budgets;
using SmartBudgetAPI.Application.DTOs.Common;
using SmartBudgetAPI.Application.Features.Budgets.Commands.CreateBudget;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartBudgetAPI.Controllers;

/// <summary>
/// Controller for budget management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User not authenticated"));
    }

    /// <summary>
    /// Create a new budget
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Create budget", Description = "Creates a new budget for authenticated user")]
    [SwaggerResponse(201, "Budget created successfully", typeof(ApiResponse<BudgetDto>))]
    [SwaggerResponse(400, "Invalid input data")]
    public async Task<ActionResult<ApiResponse<BudgetDto>>> Create([FromBody] CreateBudgetDto createBudgetDto)
    {
        try
        {
            var userId = GetUserId();
            var command = new CreateBudgetCommand(userId, createBudgetDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id = result.Id }, 
                ApiResponse<BudgetDto>.SuccessResponse(result, "Budget created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<BudgetDto>.FailureResponse(ex.Message));
        }
    }
}

