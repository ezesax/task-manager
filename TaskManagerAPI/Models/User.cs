using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; }  // Considera usar hash en lugar de guardar contraseñas directamente

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
