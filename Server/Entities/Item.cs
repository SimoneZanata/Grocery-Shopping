using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public bool Purchased { get; set; }
    }
}