namespace Shared.DTOs;

using System.ComponentModel.DataAnnotations;

public class ChatUserDto
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    
    // implement later
    // public IEnumerable<int> UserIds { get; set; }
    
}