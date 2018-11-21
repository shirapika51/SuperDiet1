using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperDiet.Models
{
    public class Item
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Calories { get; set; }
        public virtual ICollection<ItemOrder> ItemOrders { get; set; }
    }
}
