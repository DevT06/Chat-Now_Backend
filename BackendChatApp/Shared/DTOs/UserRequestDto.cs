using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class UserRequestDto
{
    //later change for authentication
    //maybe add string.Empty
    
    public string Name { get; set; }
    
    //[EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}