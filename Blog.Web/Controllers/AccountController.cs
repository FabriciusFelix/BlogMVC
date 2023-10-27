using Blog.Web.Data;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _manager;

        public AccountController(UserManager<IdentityUser> manager)
        {
            _manager = manager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };
            var identityResult = await _manager.CreateAsync(identityUser,registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                //assign this user the "User" role
                var roleIdentityResult = await _manager.AddToRoleAsync(identityUser, "User");
                if (roleIdentityResult.Succeeded)
                {
                    return RedirectToAction("Register");
                }
            }
            //Show error notification
            return View();
        }
    }
}
