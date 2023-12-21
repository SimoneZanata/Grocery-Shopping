using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Entities;

namespace Server.DTOs
{
    public class ItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}