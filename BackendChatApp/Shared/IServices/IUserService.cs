using Shared.DTOs;
using Shared.Entities;

namespace Shared.IServices;

public interface IUserService
{
    IEnumerable<UserResponseDto> GetAll();

    UserResponseDto GetById(int id);

    IEnumerable<UserResponseDto> GetByName(string name);
    
    UserResponseDto GetByEmail(string email);

    User Add(UserRequestDto userRequestDto);

    void Update(int id, UserRequestDto userRequestDto);

    //maybe implement delete
}