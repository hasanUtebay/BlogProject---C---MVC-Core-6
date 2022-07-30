using AutoMapper;
using BlogProject.DAL.Repositories.Interfaces.Concrete;
using BlogProject.Models.Entities.Concrrete;
using BlogProject.Models.Enums;
using BlogProject.WEB.Models.DTOs;
using BlogProject.WEB.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlogProject.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly IMapper _mapper; 
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UserController(IAppUserRepository appUserRepository, UserManager<IdentityUser> userManager, IMapper mapper, IArticleRepository articleRepository, ICategoryRepository categoryRepository)
        {
            _appUserRepository = appUserRepository; 
            _userManager = userManager; 
            _mapper = mapper;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> Create(CreateUserDTO createUserDTO)
        {
            if (ModelState.IsValid)
            {
             
                var checkUserName = _userManager.Users.Any(a => a.UserName == createUserDTO.UserName);
                var checkMail = _userManager.Users.Any(a => a.Email == createUserDTO.Mail);

                if (!checkMail && !checkUserName) 
                {                   
                    string newId = Guid.NewGuid().ToString();
                    IdentityUser identityUser = new IdentityUser()
                    {
                        Email = createUserDTO.Mail,
                        UserName = createUserDTO.UserName,
                        Id = newId
                    };

                    IdentityResult result = await _userManager.CreateAsync(identityUser, createUserDTO.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(identityUser, "Member");

                        var user = _mapper.Map<AppUser>(createUserDTO);

                        user.IdentityId = newId;

                        if (createUserDTO.ImagePath != null)
                        {
                        
                            using var image = Image.Load(createUserDTO.ImagePath.OpenReadStream());
                            image.Mutate(a => a.Resize(500, 500)); 
                            image.Save($"wwwroot/images/users/{user.UserName}.jpg");
                          
                            user.Image = ($"/images/users/{user.UserName}.jpg");

                            _appUserRepository.Create(user);
                            return RedirectToAction("Login", "Home"); 
                        }
                    }
                }
                else
                {
                  
                    TempData["Message"] = "Bu mail adresi veya kullanıcı adı daha önce alınmıştır.";
                }
            }
            return View(createUserDTO); 
        }

        // Detail ArticleList Of User
        public async Task<IActionResult> Detail(int id)
        {
            AppUser appUser = _appUserRepository.GetDefault(a => a.ID == id);
            IdentityUser identityUser = await _userManager.FindByIdAsync(appUser.IdentityId);

            List<Article> articleList = _articleRepository.GetDefaults(a => a.Statu != Statu.Passive && a.AppUserID == appUser.ID);

            List<Category> userFollowedCategories = _categoryRepository.GetCategoryWithUser(appUser.ID);

            GetProfileVM getProfileVM = new GetProfileVM()
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
    }
}
