using CleanArchDemo.Application.DTOs;
using CleanArchDemo.Application.Interfaces;
using CleanArchDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchDemo.Application.Services
{
    // Modern way of DI. instead of construction
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
           _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);

            return users.Select(u => new UserResponseDto(
                u.Id, u.Username, u.Email, u.DisplayName, u.Bio, u.CreatedAt, u.UpdatedAt
            ));
        }
        public async Task<UserResponseDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null) return null;

            return new UserResponseDto(
                user.Id, user.Username, user.Email, user.DisplayName, user.Bio, user.CreatedAt, user.UpdatedAt
            );
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto dto, CancellationToken cancellationToken = default)
        {

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.Password, // Tip: In production, hash this with BCrypt / Argon2 / ASP.NET Identity
                DisplayName = dto.DisplayName,
                Bio = dto.Bio,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _userRepository.AddAsync(user, cancellationToken);
            
            return new UserResponseDto(
                created.Id, created.Username, created.Email, created.DisplayName, created.Bio, created.CreatedAt, created.UpdatedAt
            );
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserRequestDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null) return false;

            user.DisplayName = dto.DisplayName ?? user.DisplayName;
            user.Bio = dto.Bio ?? user.Bio;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null) return false;

            // Soft delete implementation
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user, cancellationToken);

            return true;
        }
    }
}
