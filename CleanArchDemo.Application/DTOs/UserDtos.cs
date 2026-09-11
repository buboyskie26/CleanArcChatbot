using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchDemo.Application.DTOs
{
    public record UserResponseDto(
        int Id,
        string Username,
        string Email,
        string? DisplayName,
        string? Bio,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
    public record CreateUserRequestDto(
        string Username,
        string Email,
        string Password, // In real apps, hash this before saving
        string? DisplayName,
        string? Bio
    );
    public record UpdateUserRequestDto(
        string? DisplayName,
        string? Bio
    );
}
