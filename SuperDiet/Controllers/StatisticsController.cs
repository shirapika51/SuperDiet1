using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperDiet.Models;
using SuperDiet.Data;
using Microsoft.AspNetCore.Identity;

namespace SuperDiet.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Statistics
        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            return View();
        }

        [HttpGet("GetStatistics")]
        public IActionResult GetStatistics()
        {
            var countitems = from itemOrder in _context.ItemOrder
                          join item in _context.Item
                          on itemOrder.ItemID equals item.ID
                          group itemOrder by item.Name into depGroup
                          select new { name = depGroup.Key, count = depGroup.Sum(x => x.Quantity) };
            //var countitems = (from itemOrder in _context.ItemOrder
            //                  group itemOrder.Item.Name by itemOrder.Item.Name into devgroup
            //                  select new
            //                  {
            //                      name = devgroup.Key,
            //                      count = devgroup.Sum()
            //                  });
            return Ok(countitems);
        }
    }
}