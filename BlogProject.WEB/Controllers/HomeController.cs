using BlogProject.DAL.Repositories.Interfaces.Concrete;
using BlogProject.Models.Entities.Concrrete;
using BlogProject.Models.Enums;
using BlogProject.WEB.Models;
using BlogProject.WEB.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogProject.WEB.Controllers
{
    public class HomeController : Controller
    {    
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly SignInManager<IdentityUser> _signInManager; 
        private readonly IAppUserRepository _appUserRepository; 

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAppUserRepository appUserRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appUserRepository = appUserRepository;
        }


        // Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dTO)
        {
            if (ModelState.IsValid) 
            {
                IdentityUser identityUser = await _userManager.FindByEmailAsync(dTO.Mail); 
                AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

                if (identityUser != null && appUser.Statu != Statu.Passive) 
                {
                    await _signInManager.SignOutAsync(); 
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(identityUser, dTO.Password, true, true);

                    if (result.Succeeded) 
                    {
                        string role = (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault(); 
                        return RedirectToAction("Index", "AppUser", new { area = role }); 
                    }
                }
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }
    }
}