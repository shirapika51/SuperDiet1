using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperDiet.Data;
using SuperDiet.Models;

namespace SuperDiet.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminPanelController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminPanel
        public IActionResult Index()
        {
            if(!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            return View();
        }

        public async Task<IActionResult> ItemsView()
        {
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            return View(await _context.Item.ToListAsync());
        }


        public async Task<IActionResult> UsersView()
        {
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            return View(await _context.Users.ToListAsync());
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("SearchUser/{email}/{fname}/{lname}")]
        public IActionResult Search([FromRoute] string email, [FromRoute] string fname, [FromRoute] string lname)
        {
            var users = _context.Users;
            if (email == "empty")
            {
                if (fname == "empty")
                {
                    if (lname == "empty")
                    {
                        return Ok(users.ToList());
                    }
                    else
                    {
                        return Ok(users.Where(u => u.LastName.Contains(lname)).ToList());
                    }
                }
                else
                {
                    if (lname == "empty")
                    {
                        return Ok(users.Where(u => u.FirstName.Contains(fname)).ToList());
                    }
                    else
                    {
                        var byfname = users.Where(u => u.FirstName.Contains(fname)).ToList();
                        return Ok(byfname.Where(u => u.LastName.Contains(lname)).ToList());
                    }
                }
            }
            else
            {
                if (fname == "empty")
                {
                    if (lname == "empty")
                    {
                        return Ok(users.Where(u=> u.Email.Equals(email)).ToList());
                    }
                    else
                    {
                        var byemail = users.Where(u=>u.Email.Equals(email)).ToList();
                        return Ok(byemail.Where(u => u.LastName.Contains(lname)).ToList());
                    }
                }
                else
                {
                    if (lname == "empty")
                    {
                        var byemail = users.Where(u => u.Email.Equals(email)).ToList();
                        return Ok(byemail.Where(u => u.FirstName.Contains(fname)).ToList());
                    }
                    else
                    {
                        var byemail = users.Where(u => u.Email.Equals(email)).ToList();
                        var byfname = byemail.Where(u => u.FirstName.Contains(fname)).ToList();
                        return Ok(byfname.Where(u => u.LastName.Contains(lname)).ToList());
                    }
                }
            }
        }

        public async Task<IActionResult> BranchesView()
        {
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            return View(await _context.Branch.ToListAsync());
        }

        // GET: AdminPanel/Create
        public IActionResult CreateBranch()
        {
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            return View();
        }

        // POST: AdminPanel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBranch([Bind("ID,City,Address,Latitude,Longtitude")] Branch item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BranchesView));
            }
            return View(item);
        }

        public async Task<IActionResult> EditBranch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            var item = await _context.Branch.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: AdminPanel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBranch(int id, [Bind("ID,City,Address,Latitude,Longtitude")] Branch branch)
        {
            if (id != branch.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BranchesView));
            }
            return View(branch);
        }

        // GET: AdminPanel/Delete/5
        public async Task<IActionResult> DeleteBranch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            var item = await _context.Branch
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: AdminPanel/Delete/5
        [HttpPost, ActionName("DeleteBranch")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var item = await _context.Branch.FindAsync(id);
            _context.Branch.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BranchesView));
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.ID == id);
        }

        // GET: AdminPanel/Details/5
        public async Task<IActionResult> DetailsItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        //// GET: Users/Details/5
        //public async Task<IActionResult> DetailsUser(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    if (!User.IsInRole("Admin"))
        //    {
        //        if (!User.IsInRole("Costumer"))
        //            return Unauthorized();
        //        return StatusCode(403);
        //    }
        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        // GET: Users/Edit/5
        //public async Task<IActionResult> EditUser(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditUser(int id, [Bind("ID,UserName,Password,IsAdmin,FirstName,LastName")] User user)
        //{
        //    if (id != user.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(UsersView));
        //    }
        //    return View(user);
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.User.Any(e => e.ID == id);
        //}

        // GET: Users/Delete/5
        //public async Task<IActionResult> DeleteUser(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        // POST: Users/Delete/5
        //[HttpPost, ActionName("DeleteUser")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmedUser(int id)
        //{
        //    var user = await _context.User.FindAsync(id);
        //    _context.User.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(UsersView));
        //}

        // GET: AdminPanel/Create
        public IActionResult CreateItem()
        {
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            return View();
        }

        // POST: AdminPanel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem([Bind("ID,CategoryID,Name,Price,Quantity,Calories")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ItemsView));
            }
            return View(item);
        }

        // GET: AdminPanel/Edit/5
        public async Task<IActionResult> EditItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: AdminPanel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, [Bind("ID,CategoryID,Name,Price,Quantity,Calories")] Item item)
        {
            if (id != item.ID)
            {
                return RedirectToAction(nameof(ErrorView));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
                    {
                        return RedirectToAction("ErrorView", new { error = "item not found" });
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ItemsView));
            }
            return View(item);
        }

        // GET: AdminPanel/Delete/5
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin"))
            {
                if (!User.IsInRole("Costumer"))
                    return Unauthorized();
                return StatusCode(403);
            }
            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: AdminPanel/Delete/5
        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ItemsView));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ID == id);
        }

        public IActionResult ErrorView(string error)
        {
            ViewBag.Massage = error;
            return View(error);
        }
    }
}
