using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    
    public string? KnownAs { get; set; }
    public string? Gender { get; set; }
    public string? DateOfBirth { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    [Required]
    [StringLength(20,MinimumLength = 6)]
    public string password { get; set; }= string.Empty;
    
} 
