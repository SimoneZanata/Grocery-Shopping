using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Server.Data;
using Server.DTOs;
using Server.Entities;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {

            _context = context;
        }




        [HttpGet("{userId}/items")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsForUser(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Items)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var itemDtos = user.Items.Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Price = item.Price,
                Purchased = item.Purchased

            });

            return Ok(itemDtos);

        }

        [HttpPost("{userId}/items/")]
        public async Task<ActionResult<UserDto>> AddItemToUser(int userId, Item addItem)
        {
            // Verifica se l'utente esiste
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Check if an item with the same name already exists in the user's list
            if (user.Items.Any(item => item.Name == addItem.Name))
            {
                return BadRequest("Item with the same name already exists for the user");
            }

            // Crea il nuovo elemento
            var newItem = new Item
            {
                Name = addItem.Name,
                Quantity = addItem.Quantity,
                Price = addItem.Price,
                Purchased = false,
                UserId = user.Id
            };

            // Aggiungi l'elemento alla lista dell'utente
            user.Items.Add(newItem);

            // Salva le modifiche nel database
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{userId}/items/{itemId}")]
        public async Task<ActionResult> UpdateItemForUser(int userId, int itemId, Item updatedItem)
        {
            var user = await _context.Users
                .Include(u => u.Items)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var itemToUpdate = user.Items.FirstOrDefault(i => i.Id == itemId);

            if (itemToUpdate == null)
            {
                return NotFound("Item not found");
            }

            // Update item properties
            itemToUpdate.Name = updatedItem.Name;
            itemToUpdate.Quantity = updatedItem.Quantity;
            itemToUpdate.Price = updatedItem.Price;
            itemToUpdate.Purchased = updatedItem.Purchased;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{userId}/items/{itemId}")]
        public async Task<ActionResult> DeleteItemFromUser(int userId, int itemId)
        {
            var user = await _context.Users.
            Include(u => u.Items).
            FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

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
    }
}

