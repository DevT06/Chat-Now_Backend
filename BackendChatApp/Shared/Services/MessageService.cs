using AutoMapper;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;

namespace Shared.Services;

public class MessageService : IMessageService
{
    private readonly IChatRepository _chatRepository;
    
    private readonly IUserRepository _userRepository;
    
    private readonly IMessageRepository _messageRepository;

    public MessageService(IChatRepository chatRepository, IUserRepository userRepository, IMessageRepository messageRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
    }

    public IEnumerable<MessageDto> GetAll()
    {
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.Chat.Id));
        });

        var mapper = new Mapper(config);

        var messages = _messageRepository.GetAll().ToList();
        var dtos = messages.Select(message => mapper.Map<MessageDto>(message));

        return dtos;
    }

    public MessageDto GetById(int id)
    {
        var message =  _messageRepository.GetById(id);
        
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.Chat.Id));
        });
        
        var mapper = new Mapper(config);
    
        var dto = mapper.Map<MessageDto>(message);

        return dto;
    }

    public Message Add(MessageDto messageDto)
    {
        var message = new Message();

        message.Text = messageDto.Text;
        var user = _userRepository.GetById(messageDto.UserId);
        message.User = user;
        var chat = _chatRepository.GetById(messageDto.ChatId);
        message.Chat = chat;

        message.CreatedAt = messageDto.CreatedAt;
        message.Image = messageDto.Image;

        _messageRepository.Add(message);

        return message;
    }

    public void Update(int id, MessageDto messageDto)
    {
        var existingMessage = _messageRepository.GetById(id);
        
        if (!string.IsNullOrEmpty(messageDto.Text) && !string.IsNullOrWhiteSpace(messageDto.Text))
        {
            existingMessage.Text = messageDto.Text;
        }

        existingMessage.Image = messageDto.Image;

        existingMessage.CreatedAt = messageDto.CreatedAt;

        _messageRepository.Update(existingMessage);
    }
    
}