using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Interfaces;

namespace DataAccess.EFCore;

public class MessageRepository : IMessageRepository
{
    private readonly ChatNowDbContext _context;

    public MessageRepository(ChatNowDbContext context)
    {
        _context = context;
    }

    public Message? GetById(int id)
    {
        return _context.Messages
            .Include(m => m.Chat)
            .Include(m => m.User)
            .FirstOrDefault(m => m.Id == id);;
    }
    
    public IEnumerable<Message> GetByIds(IEnumerable<int> ids)
    {
        return _context.Messages.Where(m => ids.Contains(m.Id));
    }

    public IEnumerable<Message> GetAll()
    {
        return _context.Messages
            .Include(m => m.Chat)
            .Include(m => m.User);
    }

    public Message Add(Message message)
    {
        _context.Users.Attach(message.User);
        _context.Chats.Attach(message.Chat);
        _context.Messages.Add(message);       
        _context.SaveChanges();       
        return message;
    }

    public Message Update(Message message)
    {
        _context.Messages.Update(message);       
        _context.SaveChanges();       
        return message;
    }

    public void Delete(Message message)
    {
        _context.Messages.Remove(message);
        _context.SaveChanges();
        //throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
        var existingMessage = GetById(id);      
        _context.Messages.Remove(existingMessage);       
        _context.SaveChanges();
    }
    
    public bool Exists (int id)
    {
        return _context.Messages.Any(m => m.Id == id);
    }
}