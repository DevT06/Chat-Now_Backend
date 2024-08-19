using Shared.Entities;

namespace Shared.Interfaces;

public interface IMessageRepository
{
    Message? GetById(int id);
    
    IEnumerable<Message> GetByIds(IEnumerable<int> ids);
    
    IEnumerable<Message> GetAll();
    
    Message Add(Message message);
    
    Message Update(Message message);

    void Delete(Message message);
    
    void DeleteById(int id);
    
    bool Exists(int id);
}