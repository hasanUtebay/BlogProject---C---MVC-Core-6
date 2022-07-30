using AutoMapper;
using BlogProject.DAL.Repositories.Interfaces.Concrete;
using BlogProject.Models.Entities.Concrrete;
using BlogProject.Models.Enums;
using BlogProject.WEB.Areas.Member.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.WEB.Areas.Member.Controllers
{
    [Area("Member")] 
    [Authorize(Roles = "Member")] 
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IUserFollowedCategoryRepository _userFollowedCategoryRepository;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, UserManager<IdentityUser> userManager, IAppUserRepository appUserRepository, IUserFollowedCategoryRepository userFollowedCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _userFollowedCategoryRepository = userFollowedCategoryRepository;
        }

        // Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDTO createCategoryDTO)
        {
            if (ModelState.IsValid)
            {
                Category category = _mapper.Map<Category>(createCategoryDTO);
                _categoryRepository.Create(category);
                return RedirectToAction("List");
            }
            return View(createCategoryDTO);
        }

        // List
        public async Task<IActionResult> List()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            List<UserFollowedCategory> userFollowedCategories = _userFollowedCategoryRepository.GetFollowedCategories(a => a.AppUserID == appUser.ID);

            ViewBag.list = userFollowedCategories;

            List<Category> model = _categoryRepository.GetDefaults(a => a.Statu != Statu.Passive);

            return View(model);
        }
         
        // Follow
        public async Task<IActionResult> Follow(int id)
        {
            Category category = _categoryRepository.GetDefault(a => a.ID == id);

            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            category.UserFollowedCategories.Add(new UserFollowedCategory
            {
                Category = category,
                CategoryID = category.ID,
                AppUser = appUser,
                AppUserID = appUser.ID
            });

            _categoryRepository.Update(category);
            return RedirectToAction("List");
        }

        // UnFollow
        public async Task<IActionResult> UnFollow(int id)
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            Category category = _categoryRepository.GetDefault(a => a.ID == id);

            UserFollowedCategory userFollowedCategory = _userFollowedCategoryRepository.GetDefault(a => a.CategoryID == category.ID && a.AppUserID == appUser.ID);

            _userFollowedCategoryRepository.Delete(userFollowedCategory);

            return RedirectToAction("List");
        }

    }
}
