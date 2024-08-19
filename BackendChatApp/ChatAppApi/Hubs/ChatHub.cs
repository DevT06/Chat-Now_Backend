using AutoMapper;
using ChatAppApi.Models;
using Microsoft.AspNetCore.SignalR;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;

namespace ChatAppApi.Hubs;

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
        /*var dto = new MessageDto();
        dto.Text = "Testlol xD";
        dto.CreatedAt = DateTime.Now;
        dto.Image = [];
        dto.UserId = 1;
        dto.ChatId = 2;*/
        
        //var message = _messageService.Add(dto);
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        
        
        await Clients.Group(connection.ChatRoom)
            .SendAsync("JoinSpecificChatRoom", "admin", $"{connection.Username} has joined {connection.ChatRoom} test message:", $"{connection.Username}", $"");

    }
    
    public async Task JoinSpecificChat(string chatName, string username, string email)
    {
        /*var dto = new MessageDto();
        dto.Text = "Testlol xD";
        dto.CreatedAt = DateTime.Now;
        dto.Image = [];
        dto.UserId = 1;
        dto.ChatId = 2;*/
        
        //var message = _messageService.Add(dto);
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
            
            //var newMessageId = _userRepository.GetByIdFast(messageDto.UserId).Messages.LastOrDefault().Id;
            var user = _userRepository.GetByIdFast(messageDto.UserId);

            dto.Id = user.Messages.Last().Id;
            dto.User.Name = user.Name;
            dto.User.Email = user.Email;

            //Console.WriteLine(dto.CreatedAt);
            
            //Console.WriteLine(newMessageId);
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
}