using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Entities;

namespace Server.Controllers
{
    [Authorize]
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

        [HttpGet("{userId}/items/{itemId}")]
        public async Task<ActionResult<ItemDto>> GetItemForUser(int userId, int itemId)
        {
            var user = await _context.Users
                .Include(u => u.Items)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var item = user.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return NotFound("Item not found");
            }

            var itemDto = new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Price = item.Price,
                Purchased = item.Purchased
            };

            return Ok(itemDto);
        }

        [HttpPost("{userId}/items/")]
        public async Task<ActionResult> AddItemToUser(int userId, ItemDto addItem)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.Items.Any(item => item.Name == addItem.Name))
            {
                return BadRequest("Item with the same name already exists for the user");
            }

            var newItem = new Item
            {
                Name = addItem.Name,
                Quantity = addItem.Quantity,
                Price = addItem.Price,
                Purchased = false

            };

            user.Items.Add(newItem);
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

            itemToUpdate.Name = updatedItem.Name;
            itemToUpdate.Quantity = updatedItem.Quantity;
            itemToUpdate.Price = updatedItem.Price;
            itemToUpdate.Purchased = updatedItem.Purchased;

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

            user.Items.Remove(itemToDelete);
            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}

