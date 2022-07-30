using AutoMapper;
using BlogProject.DAL.Repositories.Interfaces.Concrete;
using BlogProject.Models.Entities.Concrrete;
using BlogProject.Models.Enums;
using BlogProject.WEB.Areas.Member.Models.DTOs;
using BlogProject.WEB.Areas.Member.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlogProject.WEB.Areas.Member.Controllers
{
    [Area("Member")] 
    [Authorize(Roles ="Member")] 
    public class AppUserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AppUserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAppUserRepository appUserRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IArticleRepository articleRepository, ICategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appUserRepository = appUserRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        // Index page after LOGIN
        public async Task<IActionResult> Index()
        {
            List<GetArticleWithUserVM> articles = _articleRepository.GetByDefaults
             (
                  selector: a => new GetArticleWithUserVM()
                  {
                      Title = a.Title,
                      Content = a.Content,
                      UserId = a.AppUser.ID, 
                      UserFullName = a.AppUser.FullName, 
                      UserImage = a.AppUser.Image,
                      ArticleId = a.ID,
                      Image = a.Image,
                      CreatedDate = a.CreateDate,
                      CategoryName = a.Category.Name, 
                      CategoryID = a.CategoryID,
                      ReadingTime = a.ReadingTime,
                      CreateDate = a.CreateDate,
                      ReadCounter = a.ReadCounter
                  },
                     
                      expression: a => a.Statu != Statu.Passive,
                
                      include: a => a.Include(a => a.AppUser).Include(a => a.Category),
                   
                      orderby: a => a.OrderByDescending(a => a.CreateDate)
               );
          
            ViewBag.AllCategory = _categoryRepository.GetDefaults(a => a.Statu != Statu.Passive);
            return View(articles.Take(10).ToList());
        }

        // Logout
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        // Update
        public async Task<IActionResult> Update()
        {           
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            AppUser user = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            UpdateAppUserDTO updateAppUserDTO = _mapper.Map<UpdateAppUserDTO>(user);

            updateAppUserDTO.Mail = identityUser.Email;
            updateAppUserDTO.oldImage = user.Image;
            updateAppUserDTO.oldPassword = user.Password;

            updateAppUserDTO.oldPassword1 = user.OldPassword1;
            updateAppUserDTO.oldPassword2 = user.OldPassword2;
            updateAppUserDTO.oldPassword3 = user.OldPassword3;

            return View(updateAppUserDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAppUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                string oldPassword = dto.oldPassword;
                string oldImage = dto.oldImage;

                AppUser appUser = _mapper.Map<AppUser>(dto);

                List<string> last3Passwords = new List<string> { appUser.OldPassword1, appUser.OldPassword2, appUser.OldPassword3 };

                bool passwordResult = last3Passwords.Exists(a => a == dto.Password);

                IdentityUser identityUser = await _userManager.FindByIdAsync(dto.IdentityID);

                if (identityUser != null)
                {
                    if (passwordResult)
                    {
                        TempData["Message"] = "Girdiğiniz şifre önceki 3 şifrenizden farklı olmalıdır";
                        return View(dto);
                    }

                    identityUser.Email = dto.Mail;
                    identityUser.UserName = appUser.UserName;

                    await _userManager.ChangePasswordAsync(identityUser, oldPassword, appUser.Password);
                    IdentityResult result = await _userManager.UpdateAsync(identityUser);

                    if (result.Succeeded)
                    {
                        if (appUser.ImagePath != null)
                        {
                            string imageName = oldImage + ".jpg"; 

                            string deletedImage = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users", $"{imageName}"); 

                            if (System.IO.File.Exists(deletedImage))
                            {
                                System.IO.File.Delete(deletedImage); 
                            }

                            using var image = Image.Load(dto.ImagePath.OpenReadStream());
                            image.Mutate(a => a.Resize(1000, 1000)); 

                            image.Save($"wwwroot/images/users/{appUser.UserName}.jpg");
                                                      
                            appUser.Image = ($"/images/users/{appUser.UserName}.jpg");

                        }
                        appUser.OldPassword3 = appUser.OldPassword2;
                        appUser.OldPassword2 = appUser.OldPassword1;
                        appUser.OldPassword1 = appUser.Password;

                        _appUserRepository.Update(appUser);
                    }
                }

                return RedirectToAction("Index");
            }
            return View(dto);
        }

        // Detail
        public async Task<IActionResult> Detail(int id)
        {

            AppUser appUser = _appUserRepository.GetDefault(a => a.ID == id);
            IdentityUser identityUser = await _userManager.FindByIdAsync(appUser.IdentityId);

            List<Article> articleList = _articleRepository.GetDefaults(a => a.Statu != Statu.Passive && a.AppUserID == appUser.ID);

            List<Category> userFollowedCategories = _categoryRepository.GetCategoryWithUser(appUser.ID);

            GetAppUserProfileVM getProfileVM = new GetAppUserProfileVM()
            {
                FullName = appUser.FullName,
                Image = appUser.Image,
                Mail = identityUser.Email,
                Articles = articleList,
                Categories = userFollowedCategories,
            };

            foreach (var item in getProfileVM.Articles)
            {
                item.Category = _categoryRepository.GetDefault(a => a.Statu != Statu.Passive && a.ID == item.CategoryID);
            }


            return View(getProfileVM);

        }

        // Delete
        public async Task<IActionResult> Delete()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);
            AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);
            _appUserRepository.Delete(appUser);
            return Redirect("~/");
        }

        // User Self Detail Page
        public async Task<IActionResult> ActivatedUserDetail()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);
            AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);


            List<Article> articleList = _articleRepository.GetDefaults(a => a.Statu != Statu.Passive && a.AppUserID == appUser.ID);

            List<Category> userFollowedCategories = _categoryRepository.GetCategoryWithUser(appUser.ID);

            GetAppUserProfileVM getProfileVM = new GetAppUserProfileVM()
            {
                FullName = appUser.FullName,
                Image = appUser.Image,
                Mail = identityUser.Email,
                Articles = articleList,
                Categories = userFollowedCategories,
            };

            foreach (var item in getProfileVM.Articles)
            {
                item.Category = _categoryRepository.GetDefault(a => a.Statu != Statu.Passive && a.ID == item.CategoryID);
            }

            return View(getProfileVM);
        }

        // About
        public IActionResult About()
        {
            return View();
        }
    }
}
