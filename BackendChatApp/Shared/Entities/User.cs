using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities;

public class User
{
    private ISet<Chat> _chats = new HashSet<Chat>();

    private ISet<Message> _messages = new HashSet<Message>();
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    //maybe use long instead of int (later)
    
    [Required(AllowEmptyStrings = false)]
    [Column(TypeName = "VARCHAR(255)")]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    [Column(TypeName = "VARCHAR(255)")]
    public string Email { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [PasswordPropertyText]
    [Column(TypeName = "VARCHAR(255)")]
    public string Password { get; set; }
    
    //public ICollection<Chat> Chats { get; set; }

    public ISet<Chat> Chats
    {
        get => _chats;
        set => _chats = value;
    }
    
    public ISet<Message> Messages
    {
        get => _messages;
        set => _messages = value;
    }
    
    protected bool Equals(User other)
    {
        return Id == other.Id;
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