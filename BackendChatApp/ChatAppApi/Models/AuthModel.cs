using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatAppApi.Models;

public class AuthModel
{
    [EmailAddress]
    public string email { get; set; }
    
    [PasswordPropertyText]
    public string password { get; set; }
}