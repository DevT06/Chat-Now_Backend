using Shared.Entities;

namespace Shared.Interfaces;

public interface IChatRepository
{
    Chat? GetById(int id);
    
    IEnumerable<Chat> GetAll();

    Chat? GetByName(string name);
    
    Chat Add(Chat chat);
    
    Chat Update(Chat chat);

    void Delete(Chat chat);
    
    void DeleteById(int id);
    
    bool Exists(int id);

    bool ExistsByName(string name);
}