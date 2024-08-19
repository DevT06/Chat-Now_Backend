using AutoMapper;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;
using Shared.PasswordService;

namespace Shared.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public IEnumerable<UserResponseDto> GetAll()
    {
        var config = new MapperConfiguration(cfg => 
        {
            //implement later UserIds = c.Users.Select(u => u.Id)}
            cfg.CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Chats, opt => opt.MapFrom(src => src.Chats.Select(c => new ChatUserDto { Id = c.Id, Name = c.Name })))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        
        var mapper = new Mapper(config);
        var users = _userRepository.GetAll().ToList();
        var dtos = users.Select(user => mapper.Map<UserResponseDto>(user));
        return dtos;
    }

    public UserResponseDto GetById(int id)
    {
        var config = new MapperConfiguration(cfg => 
        {
            //implement later UserIds = c.Users.Select(u => u.Id)}
            cfg.CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Chats, opt => opt.MapFrom(src => src.Chats.Select(c => new ChatUserDto { Id = c.Id, Name = c.Name })))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        
        var mapper = new Mapper(config);
        
        var user =  _userRepository.GetById(id); 
        var dto = mapper.Map<UserResponseDto>(user);

        return dto;
    }

    public IEnumerable<UserResponseDto> GetByName(string name)
    {
        var users = _userRepository.GetByName(name);
        
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Chats, opt => opt.MapFrom(src => src.Chats.Select(c => new ChatUserDto { Id = c.Id, Name = c.Name })))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        var mapper = new Mapper(config);
    
        var dtos = users.Select(user => mapper.Map<UserResponseDto>(user));

        return dtos;
    }
    
    public UserResponseDto GetByEmail(string email)
    {
        var user = _userRepository.GetByEmail(email);
        
        var config = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Chats, opt => opt.MapFrom(src => src.Chats.Select(c => new ChatUserDto { Id = c.Id, Name = c.Name })))
                .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(m => m.Id)));
        });
        var mapper = new Mapper(config);
    
        var dto = mapper.Map<UserResponseDto>(user);

        return dto;
    }

    public User Add(UserRequestDto userRequestDto)
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRequestDto, User>());
        var mapper = new Mapper(config);
        var user = mapper.Map<User>(userRequestDto);

        user.Password = PasswordHasher.HashPassword(userRequestDto.Password);
        
        _userRepository.Add(user);

        return user;
    }

    public void Update(int id, UserRequestDto userRequestDto)
    {
        var existingUser = _userRepository.GetById(id);
        
        if (userRequestDto.Email.Contains('@'))
        {
            existingUser.Email = userRequestDto.Email;
        }
        if (!string.IsNullOrEmpty(userRequestDto.Name) && !string.IsNullOrWhiteSpace(userRequestDto.Name))
        {
            existingUser.Name = userRequestDto.Name;
        }
        if (!string.IsNullOrEmpty(userRequestDto.Password) && !string.IsNullOrWhiteSpace(userRequestDto.Password))
        {
            existingUser.Password = PasswordHasher.HashPassword(userRequestDto.Password);
        }
        
        _userRepository.Update(existingUser);
    }
    
}