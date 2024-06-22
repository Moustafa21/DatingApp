using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class LoginDTO
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; }
    [Required]
    public string password { get; set; }
}
