using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Server.Data;
using Server.DTOs;
using Server.Entities;
using Server.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public UsersController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x =>
                x.Username == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }



        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }

/*
        [HttpPost("{userId}/add-item")]
        public async Task<ActionResult<UserDto>> AddItemToUser(int userId, ItemDto addItemDto)
        {
            // Verifica se l'utente esiste
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Crea il nuovo elemento
            var newItem = new Item
            {
                Name = addItemDto.Name,
                Quantity = addItemDto.Quantity,
                Price = addItemDto.Price,
                Purchased = true
            };

            // Aggiungi l'elemento alla lista dell'utente
            user.Items.Add(newItem);
            // Salva le modifiche nel database
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{userId}/delete-item/{itemId}")]
        public async Task<ActionResult> DeleteItemFromUser(int userId, int itemId)
        {
            // Trova l'utente
            var user = await _context.Users.
            Include(u => u.Items).
            FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Trova l'elemento da eliminare nella lista dell'utente
            var itemToDelete = user.Items.FirstOrDefault(i => i.Id == itemId);

            if (itemToDelete == null)
            {
                return NotFound("Item not found");
            }

            // Rimuovi l'elemento dalla lista dell'utente
            user.Items.Remove(itemToDelete);
            // Salva le modifiche nel database
            await _context.SaveChangesAsync();

            return Ok();

        }
    */
    }
}

