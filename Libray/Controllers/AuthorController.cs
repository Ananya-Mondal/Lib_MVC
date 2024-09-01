using Library.Data;
using Library.Models;
using Libray.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;
        public AuthorController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {
                var authors = await libraryDbContext.Authors.ToListAsync();
                return View(authors);

            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {
                if (await libraryDbContext.Authors.Where(m => m.Email == author.Email).CountAsync() != 0)
                {
                    ModelState.AddModelError("Error", "User is already exists ");
                }
                if (author.Dob >= System.DateTime.Now.Date)
                {
                    ModelState.AddModelError("Error", "Date of birth incorrect");

                }
                author.IsActive = true;
                author.CreatedOn = System.DateTime.Now.Date;
               
                if (ModelState.IsValid)
                {
                    await libraryDbContext.Authors.AddAsync(author);
                    await libraryDbContext.SaveChangesAsync();
                    ViewData["Success"] = "Author is saved.";
                }
                return View();

            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {
                var authors = await libraryDbContext.Authors.FindAsync(id);
                return View(authors);
                
            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }
                
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {

                var authors = await libraryDbContext.Authors.FindAsync(id);
                if (authors != null)
                {
                    libraryDbContext.Authors.Remove(authors);
                    await libraryDbContext.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Author");
            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }


        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {
                var author = await libraryDbContext.Authors.FindAsync(id);
                return View(author);
            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Author u)
        {
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {
                if (u.Dob >= System.DateTime.Now.Date)
                {
                    ModelState.AddModelError("Error", "Date of birth incorrect");
                    return View(u);
                }

                var count = await libraryDbContext.Authors.Where(m => m.Email == u.Email && m.Id != u.Id).CountAsync();

                if (count >= 1)
                {
                    ModelState.AddModelError("Error", "Author is already exists ");
                }
                else
                {
                    
                    var user = await libraryDbContext.Authors.FindAsync(u.Id);
                    if (user != null)
                    {
                        user.Name = u.Name;
                        user.Email = u.Email;
                        user.Phone = u.Phone;
                        user.Address1 = u.Address1;
                        user.Address2 = u.Address2;
                        user.City = u.City;
                        user.State = u.State;
                        user.PinCode = u.PinCode;
                        user.Gender = u.Gender;
                        user.IsActive = u.IsActive;

                    }


                    if (ModelState.IsValid)
                    {

                    await libraryDbContext.SaveChangesAsync();
                    ViewData["Success"] = "Author is saved.";
                    }

                }
                return View(u);
            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Index(string search)
        {
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {


                var users = await libraryDbContext.Authors.Where(m => m.Name.Contains(search) || search == null).ToListAsync();
                return View(users);
               

            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }

        }

    }
}
