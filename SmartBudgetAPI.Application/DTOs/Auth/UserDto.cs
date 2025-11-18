namespace SmartBudgetAPI.Application.DTOs.Auth;

/// <summary>
/// User data transfer object
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string DefaultCurrency { get; set; } = "PLN";
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

