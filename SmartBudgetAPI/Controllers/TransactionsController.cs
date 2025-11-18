using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetAPI.Application.DTOs.Common;
using SmartBudgetAPI.Application.DTOs.Transactions;
using SmartBudgetAPI.Application.Features.Transactions.Commands.CreateTransaction;
using SmartBudgetAPI.Application.Features.Transactions.Queries.GetTransactionsPaged;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartBudgetAPI.Controllers;

/// <summary>
/// Controller for transaction management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User not authenticated"));
    }

    /// <summary>
    /// Get transactions with pagination
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Get transactions", Description = "Returns paginated transactions for authenticated user")]
    [SwaggerResponse(200, "Transactions retrieved successfully", typeof(ApiResponse<PagedResultDto<TransactionDto>>))]
    public async Task<ActionResult<ApiResponse<PagedResultDto<TransactionDto>>>> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] Guid? categoryId = null)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetTransactionsPagedQuery(userId, pageNumber, pageSize, startDate, endDate, categoryId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<PagedResultDto<TransactionDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResultDto<TransactionDto>>.FailureResponse(ex.Message));
        }
    }

    /// <summary>
    /// Create a new transaction
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Create transaction", Description = "Creates a new transaction for authenticated user")]
    [SwaggerResponse(201, "Transaction created successfully", typeof(ApiResponse<TransactionDto>))]
    [SwaggerResponse(400, "Invalid input data")]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> Create([FromBody] CreateTransactionDto createTransactionDto)
    {
        try
        {
            var userId = GetUserId();
            var command = new CreateTransactionCommand(userId, createTransactionDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPaged), new { id = result.Id }, 
                ApiResponse<TransactionDto>.SuccessResponse(result, "Transaction created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<TransactionDto>.FailureResponse(ex.Message));
        }
    }
}

