using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class ChatDto
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public List<int> MessageIds { get; set; }
    
    [Required]
    public List<int> UserIds { get; set; }
}