using ChatAppApi.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Entities;
using Shared.Interfaces;
using Shared.IServices;
using Shared.PasswordService;

namespace ChatAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthUserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    private readonly IUserService _userService;
    

    public AuthUserController(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    [HttpPost]
    public ActionResult<int> Post([FromBody] AuthModel authModel)
    {
        if (!_userRepository.ExistsByEmail(authModel.email))
        {
            return NotFound("User doesnt exist");
        }

        var user = _userRepository.GetByEmail(authModel.email);
        
        if (!PasswordHasher.VerifyPassword(authModel.password, user.Password))
        {
            return Unauthorized("Wrong Password");
        }

        return Ok(user.Id);
    }
}