using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperDiet.Models
{
    public class ItemOrder
    {
        public int ItemID { get; set; }
        public Item Item { get; set; }
        public string OrderID { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
