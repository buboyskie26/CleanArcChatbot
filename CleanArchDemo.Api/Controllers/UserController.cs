using CleanArchDemo.Application.DTOs;
using CleanArchDemo.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
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
