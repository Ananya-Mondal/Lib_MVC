using Library.Data;
using Libray.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace Library.Controllers
{

    public class AccountController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;
        //private readonly IHttpContextAccessor context;

        public AccountController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }
        //public AccountController(IHttpContextAccessor httpContext)
        //{
        //    this.context = httpContext;
        //}
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") == "True")
            {
                var users = await libraryDbContext.Users.ToListAsync();
                return View(users);

            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User u)
        {

            var usr = libraryDbContext.Users.Where(m => m.Email == u.Email && m.Password == u.Password && m.IsActive == true).FirstOrDefault();

            if (usr == null)
            {
                ModelState.AddModelError("Error", "Invalid credential");
            }
            else
            {
                HttpContext.Session.SetInt32("UserId", usr.Id);
                HttpContext.Session.SetString("IsAdmin", usr.IsAdmin.ToString());
                HttpContext.Session.SetString("UserName", usr.Name);
                return RedirectToAction("Index", "Home", null);
            }

            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(User u)
        {
            if (await libraryDbContext.Users.Where(m => m.Email == u.Email).CountAsync() != 0)
            {
                ModelState.AddModelError("Error", "User is already exists ");
            }
            else
            {
                if (u.Dob >= System.DateTime.Now.Date)
                {
                    ModelState.AddModelError("Error", "Date of birth incorrect");

                }

                u.IsAdmin = false;
                u.IsActive = true;
                u.CreatedOn = System.DateTime.Now.Date;
                if (ModelState.IsValid)
                {
                    await libraryDbContext.Users.AddAsync(u);
                    await libraryDbContext.SaveChangesAsync();
                    ViewData["Success"] = "Sign up Successful now login.";
                }

            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") > 0)
            {
                var u = await libraryDbContext.Users.FindAsync(id);
                return View(u);
            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(User u)
        {
            if (HttpContext.Session.GetInt32("UserId") > 0)
            {
                var count = await libraryDbContext.Users.Where(m => m.Email == u.Email && m.Id != u.Id).CountAsync();

                if (count >= 1)
                {
                    ModelState.AddModelError("Error", "User is already exists ");
                }
                else
                {
                    if (u.Dob >= System.DateTime.Now.Date)
                    {
                        ModelState.AddModelError("Error", "Date of birth incorrect");
                        return View(u);
                    }
                    var user = await libraryDbContext.Users.FindAsync(u.Id);
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
                        if (u.Password != null)
                        {
                            user.Password = u.Password;
                        }
                        else
                        {
                            u.Password = user.Password;
                        }
                        user.Gender = u.Gender;
                        if (HttpContext.Session.GetString("IsAdmin") == "True")
                        {
                            user.IsAdmin = u.IsAdmin;
                            user.IsActive = u.IsActive;

                        }
                        else
                        {
                            user.IsAdmin = false;
                            user.IsActive = true;

                        }

                        //user.CreatedOn = u.CreatedOn;

                    }


                    //if (ModelState.IsValid)
                    //{

                    await libraryDbContext.SaveChangesAsync();
                    ViewData["Success"] = "Profile is saved.";
                    //}

                }
                return View(u);
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
                var users = await libraryDbContext.Users.FindAsync(id);
                return View(users);

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

                var users = await libraryDbContext.Users.FindAsync(id);
                if (users != null)
                {
                    libraryDbContext.Users.Remove(users);
                    await libraryDbContext.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Account");
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
                

                var users = await libraryDbContext.Users.Where(m => m.Name.Contains(search) || search == null).ToListAsync();
                return View(users);
                //return RedirectToAction("Index", "Account", users);

            }
            else
            {
                return RedirectToAction("Login", "Account", null);
            }

        }
    }
}
