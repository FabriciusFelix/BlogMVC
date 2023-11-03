using Blog.Web.Data;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _manager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> manager , 
            SignInManager<IdentityUser> signInManager)
        {    
            _manager = manager;
            _signInManager = signInManager;
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
            List<string> roles = new List<string>();
            foreach(var erro in identityResult.Errors)
            {
                roles.Add(erro.Description);
            }
            ViewBag.Erros = roles;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var signInResult = await _signInManager
                .PasswordSignInAsync(loginViewModel.Username,
                                        loginViewModel.Password
                                        , false, false);

            if (signInResult.Succeeded && signInResult != null)
            {
                return RedirectToAction("Index","Home");
            }
            //Show Errors
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
               
            return RedirectToAction("Index", "Home");
           
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
