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
            var countitems = (from itemOrder in _context.ItemOrder
                              select new
                              {
                                  name = itemOrder.Item.Name,
                                  count = itemOrder.Quantity
                              }).GroupBy(i => i.name).Count();
            return Ok(countitems);
        }
    }
}