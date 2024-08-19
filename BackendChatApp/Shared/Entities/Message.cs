using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using System.Reflection.Metadata;
using MySql.Data.Types;


namespace Shared.Entities;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    //use long instead of int (later)
    
    
    [Required(AllowEmptyStrings = false)]
    [Column(TypeName = "VARCHAR(2000)")]
    public string Text { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    
    [Column(TypeName = "MEDIUMBLOB")]
    public byte[]? Image { get; set; }
    
    
    //implement later
    //public bool Seen { get; set; }
    
    [Required]
    public User User { get; set; }
    
    [Required]
    public Chat Chat { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }
}