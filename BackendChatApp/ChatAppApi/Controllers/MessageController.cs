using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.Types;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;

namespace ChatAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    
    private readonly IMessageRepository _messageRepository;
    
    private readonly IUserRepository _userRepository;

    private readonly IChatRepository _chatRepository;

    public MessageController(IMessageService messageService, IMessageRepository messageRepository, IUserRepository userRepository, IChatRepository chatRepository)
    {
        _messageService = messageService;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }

    //deactivate later
    [HttpGet]
    public ActionResult<IEnumerable<MessageDto>> Get()
    {
        /*var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.Chat.Id));
        });

        var mapper = new Mapper(config);

        var messages = _messageRepository.GetAll().ToList();
        var dtos = messages.Select(message => mapper.Map<MessageDto>(message));
        */

        var dtos = _messageService.GetAll();
        
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public ActionResult<MessageDto> Get(int id)
    {
        //var message =  _messageRepository.GetById(id);
        
        if (_messageRepository.Exists(id) == false)           
        {           
            return NotFound($"No message found with id: {id}");           
        } 
        
        /*var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.Chat.Id));
        });
        
        var mapper = new Mapper(config);
    
        var dto = mapper.Map<MessageDto>(message);*/

        var dto = _messageService.GetById(id);
        
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<MessageDto> Post([FromBody] MessageDto messageDto)
    {
        /*var message = new Message();

        message.Text = messageDto.Text;
        var user = _userRepository.GetById(messageDto.UserId);
        message.User = user;
        var chat = _chatRepository.GetById(messageDto.ChatId);
        message.Chat = chat;

        message.CreatedAt = messageDto.CreatedAt;
        message.Image = messageDto.Image;*/

        /*if (_messageRepository.Exists(message.Id)) 
            return Conflict("Message with this id already exists");*/
    
        if (!ModelState.IsValid)
            return BadRequest(messageDto);

        var message = _messageService.Add(messageDto);

        return CreatedAtAction(nameof(Get), new { id = message.Id }, messageDto);
    }

    [HttpPut("{id}")]
    public ActionResult<MessageDto> Put(int id, [FromBody] MessageDto messageDto)
    {
        //var existingMessage = _messageRepository.GetById(id);
        
        if (!ModelState.IsValid)
            return BadRequest(messageDto);

        if (_messageRepository.Exists(id))
        {
            /*if (!string.IsNullOrEmpty(messageDto.Text) && !string.IsNullOrWhiteSpace(messageDto.Text))
            {
                existingMessage.Text = messageDto.Text;
            }

            existingMessage.Image = messageDto.Image;

            existingMessage.CreatedAt = messageDto.CreatedAt;

            _messageRepository.Update(existingMessage);*/
            
            _messageService.Update(id, messageDto);
        }
        else
        {
            return NotFound("Message with this id was not found");
        }

        return Ok("Message updated");
    }

    [HttpDelete("{id}")]
    public ActionResult<Message> Delete(int id)
    {
        var message = _messageRepository.GetById(id);
        if (message == null) 
            return NotFound();
    
        _messageRepository.DeleteById(message.Id);
        return new NoContentResult();
    }
}