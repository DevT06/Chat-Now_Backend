using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Interfaces;

namespace DataAccess.EFCore;

public class ChatRepository : IChatRepository
{
    private readonly ChatNowDbContext _context;

    public ChatRepository(ChatNowDbContext context)
    {
        _context = context;
    }

    public Chat? GetById(int id)
    {
        return _context.Chats
            .Include(c => c.Messages)
            .Include(c => c.Users)
            .FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Chat> GetAll()
    {
        return _context.Chats
            .Include(c => c.Messages)
            .Include(c => c.Users);
    }

    public Chat GetByName(string name)
    {
        return _context.Chats
                .Include(c => c.Messages)
                .Include(c => c.Users)
                .First(c => c.Name == name);
    }

    public Chat Add(Chat chat)
    {
        /*foreach (var user in chat.Users)
        {
            _context.Users.Attach(user);
        }
        
        foreach (var message in chat.Messages)
        {
            _context.Messages.Attach(message);
        }*/
        
        _context.Users.AttachRange(chat.Users);
        _context.Messages.AttachRange(chat.Messages);
        
        _context.Chats.Add(chat);       
        _context.SaveChanges();       
        return chat;
    }

    public Chat Update(Chat chat)
    {
        /*foreach (var user in chat.Users)
        {
            _context.Users.Attach(user);
        }*/
        
        /*foreach (var message in chat.Messages)
        {
            _context.Messages.Attach(message);
        }*/
        
        //changed from above
        _context.Users.AttachRange(chat.Users);
        _context.Messages.AttachRange(chat.Messages);
        
        _context.Chats.Update(chat);       
        _context.SaveChanges();       
        return chat;
    }

    public void Delete(Chat chat)
    {
        _context.Messages.RemoveRange(chat.Messages);
        
        _context.Chats.Remove(chat);
        _context.SaveChanges();
        //throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
        //implement that all messages in the chat are deleted too
        var existingChat = GetById(id);
        
        _context.Messages.RemoveRange(existingChat.Messages);
        _context.Chats.Remove(existingChat);
        _context.SaveChanges();
    }
    
    public bool Exists(int id)
    {
        return _context.Chats.Any(c => c.Id == id);
    }

    public bool ExistsByName(string name)
    {
        return _context.Chats.Any(c => c.Name == name);
    }
}