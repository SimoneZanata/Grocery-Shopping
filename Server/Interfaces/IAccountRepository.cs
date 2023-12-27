using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.DTOs;
using Server.Entities;

namespace Server.Interfaces
{
    public interface IAccountRepository
    {
    Task<bool> UserExists(string username); 

    Task<User> GetUserByUsernameAsync(string username);

    Task<bool> SaveAllAsync();
    void AddUser(RegisterDto registerDto);
    bool ValidatePassword(string password, byte[] salt, byte[] hash);
    }

    

    
}