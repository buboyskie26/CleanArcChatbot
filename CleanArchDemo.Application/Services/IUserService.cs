using CleanArchDemo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchDemo.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<UserResponseDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserAsync(int id, UpdateUserRequestDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    }
}
