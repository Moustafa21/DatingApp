using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UserDTO
{
    public required string UserName { get; set;}
    public required string TokenKey { get; set; }
    public string? PhotoUrl { get; set; }
}
