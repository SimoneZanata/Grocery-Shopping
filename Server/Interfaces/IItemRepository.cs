using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.DTOs;
using Server.Entities;

namespace Server.Interfaces
{
    public interface IItemRepository
    {

        Task<IEnumerable<ItemDto>> GetItemsDtoAsync(int userId);
        Task<ItemDto> GetItemDtoAsync(int id, int userId);
        Task<Item> GetItemAsync(int itemId, int userId);
        Task<bool>FindItemByUserIdAndNameAsync(int userId, string name);
        void AddItem(ItemDto item,int userId);        
        void UpdateItem(Item itemFromDb, ItemDto itemFromUser);
        void DeleteItem(Item item);
        Task<bool> SaveAllAsync();

        


    }
}