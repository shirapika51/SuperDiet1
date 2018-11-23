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
    public class ItemOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("GetItemOrder")]
        public async Task<IActionResult> GetItemOrder()
        {
            var UserID = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            if (UserID == null)
            {
                return RedirectToAction("Error", "Error");
            }
            var items = from itemOrder in _context.ItemOrder
                        join item in _context.Item on itemOrder.ItemID equals item.ID
                        where itemOrder.OrderID == UserID
                        select new
                        {
                            id = item.ID,
                            name = item.Name,
                            price = item.Price,
                            quantity = itemOrder.Quantity
                        };
            return Ok(items);
        }

        [HttpGet("GetAllItemOrder")]
        public IActionResult GetAllItemOrder()
        {
            var items = from itemOrder in _context.ItemOrder
                        join item in _context.Item on itemOrder.ItemID equals item.ID
                        select new
                        {
                            id = item.ID,
                            name = item.Name,
                            price = item.Price,
                            quantity = itemOrder.Quantity,
                            orderId = itemOrder.OrderID
                        };
            return Ok(items);
        }

        // POST: Api/ItemOrders/RemoveFromCart
        [HttpPost("RemoveFromCart/{ItemId}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int ItemId)
        {
            var UserID = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            if (UserID == null)
            {
                return RedirectToAction("Error", "Error");
            }
            var itemOrder = _context.ItemOrder.Where(m => m.ItemID == ItemId && m.OrderID == UserID).First();
            if (itemOrder == null)
            {
                return RedirectToAction("Error", "Error");
            }
            itemOrder.Quantity--;
            var item = await _context.Item.SingleOrDefaultAsync(p => p.ID == itemOrder.ItemID);
            if (item != null)
            {
                item.Quantity++;
            }
            if (itemOrder.Quantity == 0)
            {
                _context.ItemOrder.Remove(itemOrder);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
