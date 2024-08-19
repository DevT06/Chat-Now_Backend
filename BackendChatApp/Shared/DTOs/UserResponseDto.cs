using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class UserResponseDto
{
    //maybe add string.Empty
    //maybe add ? for nullable
    
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public List<int> MessageIds { get; set; }
    
    public List<ChatUserDto> Chats { get; set; }
}