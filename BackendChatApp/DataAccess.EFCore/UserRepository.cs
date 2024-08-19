using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Interfaces;

namespace DataAccess.EFCore;

public class UserRepository : IUserRepository
{
    private readonly ChatNowDbContext _context;

    public UserRepository(ChatNowDbContext context)
    {
        _context = context;
    }

    public User? GetByIdFast(int id)
    {
        return _context.Users
            .Include(u => u.Messages)
            .FirstOrDefault(u => u.Id == id);
    }

    public User? GetById(int id)
    {
        return _context.Users
            .Include(u => u.Messages)
            .Include(u => u.Chats)
            .FirstOrDefault(u => u.Id == id);
    }
    
    public IEnumerable<User> GetByIds(IEnumerable<int> ids)
    {
        return _context.Users.Where(u => ids.Contains(u.Id));
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users
            .Include(u => u.Messages)
            .Include(u => u.Chats);
    }

    public User GetByEmail(string email)
    {
        return _context.Users
            .Include(u => u.Messages)
            .Include(u => u.Chats)
            .First(u => u.Email == email);
    }
    
    public IEnumerable<User> GetByName(string name)
    {
        return _context.Users
            .Include(u => u.Messages)
            .Include(u => u.Chats)
            .Where(u => u.Name.Contains(name));
    }

    public User Add(User user)
    {
        /*foreach (var chat in user.Chats)
        {
            _context.Chats.Attach(chat);
        }
        
        foreach (var message in user.Messages)
        {
            _context.Messages.Attach(message);
        }*/
        
        _context.Chats.AttachRange(user.Chats);
        _context.Messages.AttachRange(user.Messages);
        
        _context.Users.Add(user);       
        _context.SaveChanges();       
        return user;
    }

    public User Update(User user)
    {
        /*foreach (var chat in user.Chats)
        {
            _context.Chats.Attach(chat);
        }
        
        foreach (var message in user.Messages)
        {
            _context.Messages.Attach(message);
        }*/
        
        _context.Chats.AttachRange(user.Chats);
        _context.Messages.AttachRange(user.Messages);
        
        _context.Users.Update(user);       
        _context.SaveChanges();       
        return user;
    }

    public void Delete(User user)
    {
        
        _context.Messages.RemoveRange(user.Messages);
        
        _context.Users.Remove(user);
        _context.SaveChanges();
        //throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
        var existingUser = GetById(id);      
        
        _context.Messages.RemoveRange(existingUser.Messages);
        _context.Users.Remove(existingUser);       
        _context.SaveChanges();
    }
    
    public bool Exists (int id)
    {
        return _context.Users.Any(u => u.Id == id);
    }
    
    public bool ExistsByEmail (string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }
}