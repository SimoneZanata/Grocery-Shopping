using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Interfaces;

namespace Server.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

    public UsersController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }


        [HttpGet("{userId}/items")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems(int userId)
        {
            var items =await _itemRepository.GetItemsDtoAsync(userId);
            return Ok(items);
        }

        [HttpGet("{userId}/items/{itemId}")]
        public async Task<ActionResult<ItemDto>> GetItemById(int userId, int itemId)
        {
            var item = await _itemRepository.GetItemDtoAsync(itemId,userId);
            if (item == null)
            {
                return NotFound("Item not found");
            }
            return Ok(item);
        }


        [HttpPost("{userId}/items")]
        public async Task<ActionResult> AddItem(int userId, ItemDto item)

        {
            var itemToCheck =await _itemRepository.FindItemByUserIdAndNameAsync(userId,item.Name);

            if (itemToCheck)
            {
                return BadRequest("Item with the same name already exists for the user");
            }

            _itemRepository.AddItem(item,userId);
            await _itemRepository.SaveAllAsync();
            return NoContent();
        }


        [HttpPut("{userId}/items/{itemId}")]
        public async Task<ActionResult> UpdateItem(int userId, int itemId, ItemDto item)
        {

            var itemToUpdate = await _itemRepository.GetItemAsync(itemId,userId);

            if (itemToUpdate == null)
            {
                return NotFound("item not found");
            }

            _itemRepository.UpdateItem(itemToUpdate,item);
            await _itemRepository.SaveAllAsync();
            return NoContent();
        }



        [HttpDelete("{userId}/items/{itemId}")]
        public async Task<ActionResult> DeleteItem(int userId, int itemId)
        {

            var itemToDelete = await _itemRepository.GetItemAsync(itemId,userId);
            if (itemToDelete == null)
            {
                return BadRequest("Item not found");
            }

           _itemRepository.DeleteItem(itemToDelete);
           await _itemRepository.SaveAllAsync();

            return NoContent();
        }

    }
}

