using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Entities;


namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetUsers()
        {
            var items = await _context.Items.ToListAsync();

            return items;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Item>> GetUser(int id)
        {
            return await _context.Items.FindAsync(id);
        }



        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(ItemDto itemDto)
        {
            if (await _context.Items.AnyAsync(i => i.Name == itemDto.Name))
            {
                return BadRequest("Item already exists.");
            }

            var newItem = new Item
            {
                Name = itemDto.Name,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
                Purchased = false
            };

            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();

            return Ok(newItem);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Item>> UpdateItem(int id, ItemDto itemDto)
        {
            // Verifica se esiste già un elemento con lo stesso nome diverso dall'elemento che si sta aggiornando
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Name == itemDto.Name && i.Id != id);
            if (existingItem != null)
            {
                return BadRequest("Item with the same name already exists.");
            }

            var itemToUpdate = await _context.Items.FindAsync(id);

            itemToUpdate.Name = itemDto.Name;
            itemToUpdate.Quantity = itemDto.Quantity;
            itemToUpdate.Price = itemDto.Price;

            await _context.SaveChangesAsync();


            return Ok(itemToUpdate);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}




