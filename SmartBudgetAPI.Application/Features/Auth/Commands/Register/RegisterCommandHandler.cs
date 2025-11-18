using MediatR;
using SmartBudgetAPI.Application.DTOs.Auth;
using SmartBudgetAPI.Domain.Entities;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Auth.Commands.Register;

/// <summary>
/// Handler for RegisterCommand
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher, 
        IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RegisterDto;

        var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == dto.Email, 
            cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            DefaultCurrency = dto.DefaultCurrency,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
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
                CreatedAt = user.CreatedAt
            }
        };
    }
}

