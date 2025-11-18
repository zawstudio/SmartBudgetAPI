namespace SmartBudgetAPI.Domain.Interfaces;

/// <summary>
/// Interface for JWT service
/// </summary>
public interface IJwtService
{
    string GenerateToken(Guid userId, string email, IEnumerable<string>? roles = null);
    string GenerateRefreshToken();
    Guid? ValidateToken(string token);
}

