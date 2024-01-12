using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AspNetCoreCookieAuthentication.Data;
using AspNetCoreCookieAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreCookieAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager,DataContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        //public IActionResult Register()
        //{
        //    return View();
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model,string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(f => f.Email == model.Email).FirstOrDefault();
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, model.Email + " Alrready exists");
                }
                else
                {
                    user = new User { Email = model.Email, Password = model.Password };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("RegisterConfirmation", new { email = model.Email });
                }

            }

            return View(model);
        }
    }
}
