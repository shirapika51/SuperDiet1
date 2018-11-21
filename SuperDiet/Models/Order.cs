using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuperDiet.Models
{
    public class Order
    {
        [Key]
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<ItemOrder> ItemOrders { get; set; }
    }
}
