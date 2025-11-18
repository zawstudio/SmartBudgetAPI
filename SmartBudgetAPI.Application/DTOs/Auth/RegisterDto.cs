namespace SmartBudgetAPI.Application.DTOs.Auth;

/// <summary>
/// DTO for user registration
/// </summary>
public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string DefaultCurrency { get; set; } = "USD";
}

