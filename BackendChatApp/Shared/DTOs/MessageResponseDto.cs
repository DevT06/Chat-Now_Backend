using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class MessageResponseDto
{
    public int Id { get; set; }
    
    //maybe add string.Empty
    [Required(AllowEmptyStrings = false)]
    public string Text { get; set; }

    //?
    
    [Required]
    public DateTime CreatedAt { get; set; }

    // maybe make nullable
    public byte[]? Image { get; set; }
    
    public UserChatDto User { get; set; }
    
    //public int ChatId { get; set; }
}