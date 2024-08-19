using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;

namespace ChatAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    
    private readonly IChatRepository _chatRepository;
    

    public ChatController(IChatRepository chatRepository, IChatService chatService)
    {
        _chatService = chatService;
        _chatRepository = chatRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ChatResponseDto>> Get()
    {
        //var config = new MapperConfiguration(cfg => cfg.CreateMap<Chat, ChatDto>());
        
        /*var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Chat, ChatDto>()
                .ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.Users.Select(u => u.Id)))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        
        var mapper = new Mapper(config);
        var chats = _chatRepository.GetAll().ToList();
        var dtos = chats.Select(chat => mapper.Map<ChatDto>(chat));
        */
        var dtos = _chatService.GetAll();
        
        return Ok(dtos);
    }

    [HttpGet("name/{name}")]
    public ActionResult<ChatResponseDto> Get(string name)
    {
        var dto = _chatService.GetByName(name);

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public ActionResult<ChatResponseDto> Get(int id)
    {
        if (_chatRepository.Exists(id) == false)           
        {           
            return NotFound($"No chat found with id: {id}");           
        } 
        
        /*var chat = _chatRepository.GetById(id);
        
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Chat, ChatDto>()
                .ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.Users.Select(u => u.Id)))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        var mapper = new Mapper(config);
    
        var dto = mapper.Map<ChatDto>(chat);*/

        var dto = _chatService.GetById(id);
    
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<ChatDto> Post([FromBody] ChatDto chatDto)
    {
        /*var config = new MapperConfiguration(cfg => cfg.CreateMap<ChatDto, Chat>());
        var mapper = new Mapper(config);
        var chat = mapper.Map<Chat>(chatDto);*/
        
        /*var chat = new Chat();
        
        chat.Name = chatDto.Name;
        ISet<Message> messages = _messageRepository.GetByIds(chatDto.MessageIds).ToHashSet();
        chat.Messages = messages;
        ISet<User> users = _userRepository.GetByIds(chatDto.UserIds).ToHashSet();
        chat.Users = users;*/
        
        // change later to name     
        /*if (_chatRepository.Exists(chat.Id)) 
            return Conflict("Chat with this id already exists");*/
        if (!ModelState.IsValid)
            return BadRequest(chatDto);
        
        if (_chatRepository.ExistsByName(chatDto.Name))
        {
            return Conflict("Chat already exists");
        }
        
        string[] parts = chatDto.Name.Split('_');
        string[] values = parts[0].Split('&');
        Array.Reverse(values);
        string newFirstPart = string.Join("&", values);
        string[] userValues = parts[1].Split(',');
        Array.Reverse(userValues);
        string newSecondPart = string.Join(",", userValues);
        string newString = $"{newFirstPart}_{newSecondPart}";

        if (_chatRepository.ExistsByName(newString))
        {
            return Conflict("Chat already exists");
        }

        var chat = _chatService.Add(chatDto);
   

        return CreatedAtAction(nameof(Get), new { id = chat.Id }, chatDto);
    }

    [HttpPut("{id}")]
    public ActionResult<ChatDto> Put(int id, [FromBody] ChatDto chatDto)
    {
        //var existingChat = _chatRepository.GetById(id);
        
        if (!ModelState.IsValid)
            return BadRequest(chatDto);
    
        if (_chatRepository.Exists(id))
        {

            /*if (!string.IsNullOrEmpty(chatDto.Name) && !string.IsNullOrWhiteSpace(chatDto.Name))
            {
                existingChat.Name = chatDto.Name;
            }
            if ((chatDto.UserIds).Count >= 2)
            {
                ISet<User> users = _userRepository.GetByIds(chatDto.UserIds).ToHashSet();
                existingChat.Users = users;
            }
            
            ISet<Message> messages = _messageRepository.GetByIds(chatDto.UserIds).ToHashSet();
            existingChat.Messages = messages;
            */
            

            _chatService.Update(id, chatDto);
        }
        else
        {
            return NotFound("Chat with this id was not found");
        }

        return Ok("Chat updated");
    }

    [HttpDelete("{id}")]
    public ActionResult<Chat> Delete(int id)
    {
        if (!_chatRepository.Exists(id)) 
            return NotFound();
    
        _chatRepository.DeleteById(id);
        return new NoContentResult();
    }
}