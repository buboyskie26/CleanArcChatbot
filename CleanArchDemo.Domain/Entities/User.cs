using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchDemo.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string PasswordHash { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
