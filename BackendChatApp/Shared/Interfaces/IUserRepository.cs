using Shared.Entities;

namespace Shared.Interfaces;

public interface IUserRepository
{

    User GetByIdFast(int id);
    User? GetById(int id);

    IEnumerable<User> GetByIds(IEnumerable<int> ids);
    
    IEnumerable<User> GetAll();
    
    User? GetByEmail(string email);

    IEnumerable<User>? GetByName(string name);
    
    User Add(User user);
    
    User Update(User user);
    
    void Delete(User user);

    void DeleteById(int id);
    
    bool Exists(int id);

    bool ExistsByEmail(string email);
}