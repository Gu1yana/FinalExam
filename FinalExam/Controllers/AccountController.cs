using FinalExam.Models;
using FinalExam.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Controllers
{
    public class AccountController
        (UserManager<AppUser> _userManager, 
        SignInManager<AppUser> _signInManager,
        RoleManager<IdentityRole> _roleManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            AppUser user = new();
            user.UserName = vm.Username;
            user.Email = vm.Email;
           var result= await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                    return View();
                }
            }
            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View(vm);
            AppUser? user = null;
            if (vm.UsernameOrEmail.Contains('@'))
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            }
            var result = await _signInManager.PasswordSignInAsync(vm.UsernameOrEmail, vm.Password, true, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "User account locked out");
                    return View();
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(" ", "User in bloked, calling admin please");
                    return View();
                }
                else
                {
                    ModelState.AddModelError(" ", "Username or email is not true");
                }
            }         
            if(ReturnUrl is not null)
                return LocalRedirect(ReturnUrl);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CreatedRoles()
        {
            await _roleManager.CreateAsync(new() { Name = "User" });
            await _roleManager.CreateAsync(new() { Name = "Admin" });
            await _roleManager.CreateAsync(new() { Name = "Moderator" });
            await _roleManager.CreateAsync(new() { Name = "SuperAdmin" });
            return Ok("Rollar yarandi");
        }
        public async Task<IActionResult> SeedDatas()
        {
            if(!await _userManager.Users.AnyAsync(x => x.NormalizedUserName == "ADMIN"))
            {
                var user = new Models.AppUser
                {
                    UserName = "admin123",
                    Email = "admin@gmail.com"
                };
                await _userManager.CreateAsync(user, "admin12345");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            if (!await _userManager.Users.AnyAsync(x => x.NormalizedUserName == "SUPERADMIN"))
            {
                var user = new Models.AppUser
                {
                    UserName = "superadmin123",
                    Email = "superadmin@gmail.com"
                };
                await _userManager.CreateAsync(user, "superadmin12345");
                await _userManager.AddToRoleAsync(user, "SuperAdmin");
            }
            if (!await _userManager.Users.AnyAsync(x => x.NormalizedUserName == "MODERATOR"))
            {
                var user = new Models.AppUser
                {
                    UserName = "moderator123",
                    Email = "moderator@gmail.com"
                };
                await _userManager.CreateAsync(user, "moderator12345");
                await _userManager.AddToRoleAsync(user, "Moderator");
            }
            return Ok("Datalar yarandi");
        }
    }
}
