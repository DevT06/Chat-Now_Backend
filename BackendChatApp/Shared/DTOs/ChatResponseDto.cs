using System.ComponentModel.DataAnnotations;
using Shared.Entities;

namespace Shared.DTOs;

public class ChatResponseDto
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public List<MessageResponseDto> Messages { get; set; }
    
    //Changed here /\
    
    [Required]
    public List<UserChatDto> Users { get; set; }
}