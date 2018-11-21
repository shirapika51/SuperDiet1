using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperDiet.Data;
using SuperDiet.Models;

namespace SuperDiet.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            return View(await _context.Item.ToListAsync());
        }

        [HttpGet("GetItem")]
        public async Task<IActionResult> GetItem()
        {
            return Ok(await _context.Item.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .SingleOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost("AddToCart/{ItemId}")]
        public async Task<IActionResult> AddToCart([FromRoute] int ItemId)
        {
            var UserID = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            if (UserID == null)
            {
                return Unauthorized();
            }
            var item = await _context.Item.SingleOrDefaultAsync(m => m.ID == ItemId);
            if (item == null || item.Quantity == 0)
            {
                return NotFound();
            }
            var itemOrder = await _context.ItemOrder.SingleOrDefaultAsync(m => m.OrderID == UserID && m.ItemID == ItemId);
            var Orderuser = await _context.Order.SingleOrDefaultAsync(m => m.ID == UserID);
            if (itemOrder == null)
            {
                itemOrder = new ItemOrder
                {
                    OrderID = UserID,
                    Order = Orderuser,
                    ItemID = item.ID,
                    Quantity = 1
                };
                _context.ItemOrder.Add(itemOrder);
            }
            else
            {
                itemOrder.Quantity += 1;
            }
            item.Quantity--;
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> Cart(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Order.FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpGet("Search/{name}/{price}/{calories}")]
        public IActionResult Search([FromRoute] string name, [FromRoute] int price, [FromRoute] int calories)
        {
            var items = _context.Item;
            var filterprice = items.Where(i => i.Price <= price).ToList();
            var filtercalories = filterprice.Where(i => i.Calories <= calories).ToList();
            if (name != "empty")
            {
                var filtername = filtercalories.Where(i => i.Name.Contains(name)).ToList();
                return Ok(filtername);
            }
            return Ok(filtercalories);
        }
    }
}
