using MediatR;
using SmartBudgetAPI.Application.DTOs.Auth;

namespace SmartBudgetAPI.Application.Features.Auth.Commands.Login;

/// <summary>
/// Command for user login
/// </summary>
public record LoginCommand(LoginDto LoginDto) : IRequest<AuthResponseDto>;

