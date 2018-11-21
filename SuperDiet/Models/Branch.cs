using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuperDiet.Models
{
    public class Branch
    {
        public int ID { get; set; }
        public String City { get; set; }
        public String Address { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longtitude { get; set; }
    }
}
