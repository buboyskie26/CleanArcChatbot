using CleanArchDemo.Application.DTOs;
using CleanArchDemo.Application.Services;
using CleanArchDemo.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanArchDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, ApplicationDbContext _context) : ControllerBase
    {
        [HttpGet("check-connection")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckConnection()
        {
            try
            {
                // 1. Verify if we can communicate with the database
                var canConnect = await _context.Database.CanConnectAsync();

                if (!canConnect)
                {
                    return StatusCode(500, new { Success = false, Message = "Could not establish connection to Supabase." });
                }

                // 2. Fetch the number of users currently in the Supabase database
                var userCount = await _context.Users.CountAsync();

                // 3. Fetch top 5 usernames to verify we can read actual data
                var previewUsers = await _context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Take(5)
                    .Select(u => new { u.Id, u.Username, u.Email, u.CreatedAt })
                    .ToListAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Successfully connected to Supabase PostgreSQL!",
                    DatabaseSchema = "SocialMediaDB",
                    TotalUsersInDb = userCount,
                    RecentUsersPreview = previewUsers
                });
            }
            catch (Exception ex)
            {
                // If there's an error, this will output the exact database error message
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while connecting to the database.",
                    Details = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await userService.GetAllUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByIdAsync(id, cancellationToken);
            if (user is null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserRequestDto dto, CancellationToken cancellationToken)
        {
            var user = await userService.CreateUserAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequestDto dto, CancellationToken cancellationToken)
        {
            var updated = await userService.UpdateUserAsync(id, dto, cancellationToken);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await userService.DeleteUserAsync(id, cancellationToken);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
