using Shared.DTOs;
using Shared.Entities;

namespace Shared.IServices;

public interface IChatService
{
    IEnumerable<ChatResponseDto> GetAll();

    ChatResponseDto GetById(int id);

    ChatResponseDto GetByName(string name);

    Chat Add(ChatDto chatDto);

    void Update(int id, ChatDto chatDto);
}