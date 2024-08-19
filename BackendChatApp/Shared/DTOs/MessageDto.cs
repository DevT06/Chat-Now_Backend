using System.ComponentModel.DataAnnotations;
using MySql.Data.Types;

namespace Shared.DTOs;

public class MessageDto
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
    
    public int UserId { get; set; }
    
    public int ChatId { get; set; }
}