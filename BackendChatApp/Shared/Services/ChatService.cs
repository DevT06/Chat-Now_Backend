using AutoMapper;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;

namespace Shared.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    
    private readonly IUserRepository _userRepository;
    
    private readonly IMessageRepository _messageRepository;

    public ChatService(IChatRepository chatRepository, IUserRepository userRepository, IMessageRepository messageRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
    }

    public IEnumerable<ChatResponseDto> GetAll()
    {
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Chat, ChatResponseDto>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => new UserChatDto { Id = u.Id, Name = u.Name, Email = u.Email})))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages.Select(m => new MessageResponseDto { Id = m.Id, Text = m.Text, Image = m.Image, CreatedAt = m.CreatedAt, /*ChatId = m.Chat.Id,*/ User = new UserChatDto { Id = m.User.Id, Name = m.User.Name, Email = m.User.Email}})));
        });
        
        var mapper = new Mapper(config);
        var chats = _chatRepository.GetAll().ToList();
        var dtos = chats.Select(chat => mapper.Map<ChatResponseDto>(chat));

        return dtos;
    }

    public ChatResponseDto GetById(int id)
    {
        var chat = _chatRepository.GetById(id);
        
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Chat, ChatResponseDto>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => new UserChatDto { Id = u.Id, Name = u.Name, Email = u.Email})))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages.Select(m => new MessageResponseDto { Id = m.Id, Text = m.Text, Image = m.Image, CreatedAt = m.CreatedAt, /*ChatId = m.Chat.Id,*/ User = new UserChatDto { Id = m.User.Id, Name = m.User.Name, Email = m.User.Email}})));
        });
        var mapper = new Mapper(config);
    
        var dto = mapper.Map<ChatResponseDto>(chat);

        return dto;
    }

    public ChatResponseDto GetByName(string name)
    {
        var chat = _chatRepository.GetByName(name);
        
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Chat, ChatResponseDto>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => new UserChatDto { Id = u.Id, Name = u.Name, Email = u.Email})))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages.Select(m => new MessageResponseDto { Id = m.Id, Text = m.Text, Image = m.Image, CreatedAt = m.CreatedAt, /*ChatId = m.Chat.Id,*/ User = new UserChatDto { Id = m.User.Id, Name = m.User.Name, Email = m.User.Email}})));
        });
        var mapper = new Mapper(config);
    
        var dto = mapper.Map<ChatResponseDto>(chat);

        return dto;
    }

    public Chat Add(ChatDto chatDto)
    {
        var chat = new Chat();
        
        chat.Name = chatDto.Name;
        ISet<Message> messages = _messageRepository.GetByIds(chatDto.MessageIds).ToHashSet();
        chat.Messages = messages;
        ISet<User> users = _userRepository.GetByIds(chatDto.UserIds).ToHashSet();
        chat.Users = users;
        
        _chatRepository.Add(chat);

        return chat;
    }

    public void Update(int id, ChatDto chatDto)
    {
        var existingChat = _chatRepository.GetById(id);
        
        if (!string.IsNullOrEmpty(chatDto.Name) && !string.IsNullOrWhiteSpace(chatDto.Name))
        {
            existingChat.Name = chatDto.Name;
        }
        if ((chatDto.UserIds).Count >= 2)
        {
            ISet<User> users = _userRepository.GetByIds(chatDto.UserIds).ToHashSet();
            existingChat.Users = users;
        }
            
        //ISet<Message> messages = _messageRepository.GetByIds(chatDto.UserIds).ToHashSet();
        //existingChat.Messages = messages;
            

        _chatRepository.Update(existingChat);
    }
    
}