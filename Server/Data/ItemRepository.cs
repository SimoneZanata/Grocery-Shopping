using Microsoft.EntityFrameworkCore;
using Server.DTOs;
using Server.Entities;
using Server.Interfaces;

namespace Server.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _context;
        public ItemRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<ItemDto>> GetItemsDtoAsync(int userId)
        {
            return await _context.Items
              .Where(i => i.UserId == userId)
              .Select(i => new ItemDto
              {
                  Quantity = i.Quantity,
                  Id = i.Id,
                  Name = i.Name,
                  Price = i.Price,
                  Purchased=i.Purchased
              })
              .ToListAsync();
        }

        public async Task<ItemDto> GetItemDtoAsync(int id, int userId)
        {
            return await _context.Items
               .Where(i => i.UserId == userId && i.Id == id)
               .Select(i => new ItemDto
               {
                   Quantity = i.Quantity,
                   Id = i.Id,
                   Name = i.Name,
                   Price = i.Price,
                   Purchased=i.Purchased
               })
               .SingleOrDefaultAsync();
        }

        public async Task<Item> GetItemAsync(int itemId, int userId)
        {
            return await _context.Items
            .Where(i => i.UserId == userId && i.Id == itemId)
            .SingleOrDefaultAsync();
        }

        public async Task<bool> FindItemByUserIdAndNameAsync(int userId, string name)
        {
            return await _context.Items.
            AnyAsync(i => i.UserId == userId && i.Name == name);
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        public void AddItem(ItemDto item,int userId)
        {
             var itemToAdd=new Item(item.Name,item.Quantity,item.Price,userId);
            _context.Items.Add(itemToAdd);

        }
    
       public void UpdateItem(Item itemFromDb, ItemDto itemFromUser)
       {
           itemFromDb.Name = itemFromUser.Name;
           itemFromDb.Quantity = itemFromUser.Quantity;
           itemFromDb.Price = itemFromUser.Price;
           itemFromDb.Purchased = itemFromUser.Purchased;

           _context.Items.Update(itemFromDb);
           
       }

       public void DeleteItem(Item item)
       {
           _context.Items.Remove(item);
       }


    }
}