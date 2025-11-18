using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetAPI.Application.DTOs.Categories;
using SmartBudgetAPI.Application.DTOs.Common;
using SmartBudgetAPI.Application.Features.Categories.Commands.CreateCategory;
using SmartBudgetAPI.Application.Features.Categories.Queries.GetAllCategories;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartBudgetAPI.Controllers;

/// <summary>
/// Controller for category management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User not authenticated"));
    }

    /// <summary>
    /// Get all user categories
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all categories", Description = "Returns all categories for authenticated user")]
    [SwaggerResponse(200, "Categories retrieved successfully", typeof(ApiResponse<List<CategoryDto>>))]
    public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAll()
    {
        try
        {
            var userId = GetUserId();
            var query = new GetAllCategoriesQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<List<CategoryDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<CategoryDto>>.FailureResponse(ex.Message));
        }
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Create category", Description = "Creates a new category for authenticated user")]
    [SwaggerResponse(201, "Category created successfully", typeof(ApiResponse<CategoryDto>))]
    [SwaggerResponse(400, "Invalid input data")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Create([FromBody] CreateCategoryDto createCategoryDto)
    {
        try
        {
            var userId = GetUserId();
            var command = new CreateCategoryCommand(userId, createCategoryDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, 
                ApiResponse<CategoryDto>.SuccessResponse(result, "Category created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<CategoryDto>.FailureResponse(ex.Message));
        }
    }
}

