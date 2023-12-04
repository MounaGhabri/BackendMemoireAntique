using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Projet.Models;
using Projet.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Projet.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return RedirectToAction("About", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var result = _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {

            var user = new User { UserName = model.Email, Email = model.Email };
            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                _signInManager.SignInAsync(user, isPersistent: false).Wait();
                await _userManager.AddToRoleAsync(user, "Admin");
                return RedirectToAction("Index", "Product");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            
                try
                {
                    var existingUser = _userManager.FindByIdAsync(id).Result;
                    existingUser.UserName = user.UserName;
                    existingUser.Email = user.Email;

                    var result = _userManager.UpdateAsync(existingUser).Result;
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            

            return View(user);
        }

        private bool UserExists(string id)
                {
                    return _userManager.FindByIdAsync(id).Result != null;
                }

        public IActionResult Delete(string id)
        {
            if (UserExists(id)){ 
            var user = _userManager.FindByIdAsync(id).Result;
            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            
}
            return View(user);}
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
