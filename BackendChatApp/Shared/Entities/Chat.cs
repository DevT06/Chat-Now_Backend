using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities;

public class Chat
{
    private ISet<Message> _messages = new HashSet<Message>();
    private ISet<User> _user = new HashSet<User>();
    
    [Key]       
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
    public int Id { get; set; }
    //use long instead of int (later)
    
    
    [Required]
    [Column(TypeName = "VARCHAR(255)")]
    public string Name { get; set; }
    
    
    [Required]
    public ISet<User> Users
    {
        get => _user;
        set => _user = value;
    }
    
    public ISet<Message> Messages
    {
        get => _messages;
        set => _messages = value;
    }
    
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