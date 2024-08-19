using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;

namespace ChatAppApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    private readonly IUserService _userService;

    /*public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }*/

    public UserController(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }
    
    

    //deactivate later
    [HttpGet]
    public ActionResult<IEnumerable<UserResponseDto>> Get()
    {
        
        /*var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.ChatIds, opt => opt.MapFrom(src => src.Chats.Select(u => u.Id)))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        
        var mapper = new Mapper(config);
        var users = _userRepository.GetAll().ToList();
        var dtos = users.Select(user => mapper.Map<UserResponseDto>(user));
        */
        var dtos = _userService.GetAll();
            
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public ActionResult<UserResponseDto> Get(int id)
    {
        if (_userRepository.Exists(id) == false)           
        {           
            return NotFound($"No user found with id: {id}");           
        } 
        
        /*var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.ChatIds, opt => opt.MapFrom(src => src.Chats.Select(u => u.Id)))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        
        var mapper = new Mapper(config);
        
        var user =  _userRepository.GetById(id);     */
        var dto = _userService.GetById(id);
        
        /*if (user == null)           
        {           
            return NotFound($"No user found with id: {id}");           
        }   */
        
        //var dto = mapper.Map<UserResponseDto>(user);
        return Ok(dto);
    }

    [HttpGet("name/{name}")]
    public ActionResult<UserResponseDto> GetByName(string name)
    {
        var dtos = _userService.GetByName(name);

        return Ok(dtos);
    }
    
    [HttpGet("email/{email}")]
    public ActionResult<UserResponseDto> GetByEmail(string email)
    {
        if (!_userRepository.ExistsByEmail(email))
        {
            return NotFound($"No user found with E-mail: {email}");     
        }
        
        var dto = _userService.GetByEmail(email);
        

        return Ok(dto);
    }
    
    [HttpPost]
    public ActionResult<UserRequestDto> Post([FromBody] UserRequestDto userRequestDto)
    {
        /*var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRequestDto, User>());
        var mapper = new Mapper(config);
        var user = mapper.Map<User>(userRequestDto);*/
        
        if (!ModelState.IsValid)
            return BadRequest(userRequestDto);
        
        if (_userRepository.ExistsByEmail(userRequestDto.Email)) 
            return Conflict("User with this Email already exists");

        var user = _userService.Add(userRequestDto);


        return CreatedAtAction(nameof(Get), new { id = user.Id }, userRequestDto);
    }

    [HttpPut("{id}")]
    public ActionResult<UserRequestDto> Put(int id, [FromBody] UserRequestDto userRequestDto)
    {
        
        /*var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRequestDto, UserResponseDto>());
        var mapper = new Mapper(config);
        var newUserDto = mapper.Map<UserResponseDto>(userRequestDto);*/
        
        if (!ModelState.IsValid)
            return BadRequest(userRequestDto);
        
        //var existingUser = _userRepository.GetById(id);

        var checkMail = _userRepository.GetById(id).Email;

        if (userRequestDto.Email != checkMail)
        {
            if (_userRepository.ExistsByEmail(userRequestDto.Email))
            {
                return Conflict("User with this Email already exists");
            }
        }


        if (_userRepository.Exists(id))
        {

            /*if (userRequestDto.Email.Contains('@'))
            {
                existingUser.Email = userRequestDto.Email;
            }
            if (!string.IsNullOrEmpty(userRequestDto.Name) && !string.IsNullOrWhiteSpace(userRequestDto.Name))
            {
                existingUser.Name = userRequestDto.Name;
            }
            if (!string.IsNullOrEmpty(userRequestDto.Password) && !string.IsNullOrWhiteSpace(userRequestDto.Password))
            {
                existingUser.Password = userRequestDto.Password;
            }*/
            

            _userService.Update(id, userRequestDto);
        }
        else
        {
            return NotFound("User with this id was not found");
        }

        return Ok("User updated");
    }

    [HttpDelete("{id}")]
    public ActionResult<User> Delete(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null) 
            return NotFound();
    
        _userRepository.DeleteById(user.Id);
        return new NoContentResult();
    }
}