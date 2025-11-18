using MediatR;
using SmartBudgetAPI.Application.DTOs.Auth;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Auth.Commands.Login;

/// <summary>
/// Handler for LoginCommand
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher, 
        IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var dto = request.LoginDto;

        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == dto.Email, 
            cancellationToken);

        if (user == null || !_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive");
        }

        user.LastLoginAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtService.GenerateToken(user.Id, user.Email);
        var refreshToken = _jwtService.GenerateRefreshToken();

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                DefaultCurrency = user.DefaultCurrency,
                IsActive = user.IsActive,
                LastLoginAt = user.LastLoginAt,
                CreatedAt = user.CreatedAt
            }
        };
    }
}

