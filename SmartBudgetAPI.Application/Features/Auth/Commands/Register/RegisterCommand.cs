using MediatR;
using SmartBudgetAPI.Application.DTOs.Auth;

namespace SmartBudgetAPI.Application.Features.Auth.Commands.Register;

/// <summary>
/// Command for user registration
/// </summary>
public record RegisterCommand(RegisterDto RegisterDto) : IRequest<AuthResponseDto>;

