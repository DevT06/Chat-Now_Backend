using AutoMapper;
using ChatAppSignalRApi.Models;
using Microsoft.AspNetCore.SignalR;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;

namespace ChatAppSignalRApi.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    
    private readonly IChatService _chatService;
    
    private readonly IUserService _userService;

    private readonly IChatRepository _chatRepository;

    private readonly IUserRepository _userRepository;

    private readonly IMessageRepository _messageRepository;

    public ChatHub(IMessageService messageService, IChatService chatService, IUserService userService, IChatRepository chatRepository, IUserRepository userRepository, IMessageRepository messageRepository)
    {
        _messageService = messageService;
        _userService = userService;
        _chatService = chatService;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
    }
    
    public async Task JoinChat(UserConnection connection)
    {
        await Clients.All
            .SendAsync("ReceiveMessage", "admin", $"{connection.Username} has joined");
    }

    public async Task JoinSpecificChatRoom(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        
        
        await Clients.Group(connection.ChatRoom)
            .SendAsync("JoinSpecificChatRoom", "admin", $"{connection.Username} has joined {connection.ChatRoom} test message:", $"{connection.Username}", $"");

    }
    
    public async Task JoinSpecificChat(string chatName, string username, string email)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
        
        
        await Clients.Group(chatName)
            .SendAsync("JoinSpecificChat", "User:", $"{username} with E-Mail: {email} joined Chat: {chatName}");

    }

    public async Task SendMessage(MessageDto messageDto, string chatName)
    {
        if (_chatRepository.ExistsByName(chatName))
        {
            _messageService.Add(messageDto);
            
            
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageDto, MessageResponseDto>()
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserChatDto { Id = src.UserId }));
            });
        
            var mapper = new Mapper(config);
    
            var dto = mapper.Map<MessageResponseDto>(messageDto);
            
            var user = _userRepository.GetByIdFast(messageDto.UserId);

            dto.Id = user.Messages.Last().Id;
            dto.User.Name = user.Name;
            dto.User.Email = user.Email;
            
            await Clients.Group(chatName)
                .SendAsync("ReceiveSpecificMessage", $"{dto.Id}", $"{dto.Text}", $"{dto.CreatedAt}", $"{dto.Image}", $"{dto.User.Id}", $"{dto.User.Name}", $"{dto.User.Email}");
        }
    }

    public async Task DeleteMessage(int messageId, string chatName)
    {
        if (_chatRepository.ExistsByName(chatName))
        {
            _messageRepository.DeleteById(messageId);

            await Clients.Group(chatName)
                .SendAsync("ReceiveSpecificMessageDeletion", $"{messageId}");
        }
    }

    /*public async Task DeleteChat(int chatId, string chatName)
    {
        if (_chatRepository.ExistsByName(chatName))
        {
            _chatRepository.DeleteById(chatId);

            await Clients.Group(chatName)
                .SendAsync("ReceiveSpecificChatDeletion", $"{chatId}");
        }
    }*/
}