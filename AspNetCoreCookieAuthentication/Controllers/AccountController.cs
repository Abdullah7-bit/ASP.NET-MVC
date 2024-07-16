using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AspNetCoreCookieAuthentication.Data;
using AspNetCoreCookieAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AspNetCoreCookieAuthentication.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(DbContext context)
        {
            
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
                    ModelState.AddModelError(string.Empty, model.Email + " Already exists");
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
    
        //RegisterConfirmation
        public IActionResult RegisterConfirmation(RegisterConfirmationModel model,string email)
        {
            if(email == null)
            {
                return RedirectToAction("/Index");
            }
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            if(user == null)
            {
                return NotFound($"Unable to find user with email {email}");
            }
            model.Email = email;

            return View(model);
        }
    
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(f => f.Email == model.Email && f.Password == model.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                    return View(model);
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserDefined", "whatever"),
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = true });

                return LocalRedirect(returnUrl);
            }

            return View(model);
        }

        [AllowAnonymous] 
        public IActionResult Logout()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home"); // Replace "Index" and "Home" with your actual home action and controller
            }
        }
    }
}
