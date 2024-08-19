using Shared.DTOs;
using Shared.Entities;

namespace Shared.IServices;

public interface IMessageService
{
    IEnumerable<MessageDto> GetAll();

    MessageDto GetById(int id);

    Message Add(MessageDto messageDto);

    void Update(int id, MessageDto messageDto);
}