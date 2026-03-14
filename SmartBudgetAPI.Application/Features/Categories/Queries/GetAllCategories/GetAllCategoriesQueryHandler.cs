using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SmartBudgetAPI.Application.DTOs.Categories;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Categories.Queries.GetAllCategories;

/// <summary>
/// Handler for GetAllCategoriesQuery
/// </summary>
public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;

    public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IDistributedCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"categories_{request.UserId}";
        var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<List<CategoryDto>>(cachedData) ?? new List<CategoryDto>();
        }

        var categories = await _unitOfWork.Categories.FindAsync(
            c => c.UserId == request.UserId && !c.IsDeleted,
            cancellationToken);

        var result = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Color = c.Color,
            Icon = c.Icon,
            ParentCategoryId = c.ParentCategoryId,
            ParentCategoryName = c.ParentCategory?.Name,
            CreatedAt = c.CreatedAt
        }).ToList();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions, cancellationToken);

        return result;
    }
}

