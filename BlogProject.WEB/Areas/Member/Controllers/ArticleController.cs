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
using System.IO;


namespace BlogProject.WEB.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = "Member")]
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILikeRepository _likeRepository;
        private readonly ICommentRepository _commentRepository;

        public ArticleController(IArticleRepository articleRepository, IMapper mapper, UserManager<IdentityUser> userManager, IAppUserRepository appUserRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment, ILikeRepository likeRepository, ICommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _likeRepository = likeRepository;
            _commentRepository = commentRepository;
        }


        // Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);
            AppUser user = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            CreateArticleDTO dto = new CreateArticleDTO()
            {
                Categories = _categoryRepository.GetByDefaults
                (
                    selector: a => new GetCategoryDTO
                    {
                        ID = a.ID,
                        Name = a.Name
                    },
                     expression: a => a.Statu != Statu.Passive
                ),

                AppUserID = user.ID
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult Create(CreateArticleDTO createArticleDTO)
        {
            if (ModelState.IsValid)
            {
                Article article = _mapper.Map<Article>(createArticleDTO);

                if (article.ImagePath != null)
                {
                    using var image = Image.Load(createArticleDTO.ImagePath.OpenReadStream());
                    image.Mutate(a => a.Resize(1000, 1000));

                    Guid guid = Guid.NewGuid();

                    image.Save($"wwwroot/images/articles/{guid}.jpg");

                    article.Image = ($"/images/articles/{guid}.jpg");

                    _articleRepository.Create(article);
                }
                return RedirectToAction("List");
            }

            createArticleDTO.Categories = _categoryRepository.GetByDefaults
                (
                    selector: a => new GetCategoryDTO
                    {
                        ID = a.ID,
                        Name = a.Name
                    },
                     expression: a => a.Statu != Statu.Passive
                );

            return View(createArticleDTO);
        }

        // List
        public async Task<IActionResult> List()
        {

            IdentityUser identityUser = await _userManager.GetUserAsync(User);
            AppUser user = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            var articleList = _articleRepository.GetByDefaults
                (

                selector: a => new GetArticleVM()
                {
                    ArticleID = a.ID,
                    CategoryName = a.Category.Name,
                    Title = a.Title,
                    Content = a.Content,
                    Image = a.Image,
                    UserFullName = a.AppUser.FullName
                },

                expression: a => a.Statu != Statu.Passive && a.AppUserID == user.ID,

                include: a => a.Include(a => a.AppUser).Include(a => a.Category),

                orderby: a => a.OrderByDescending(a => a.CreateDate)
                );

            return View(articleList);
        }

        // Update
        public IActionResult Update(int id)
        {
            Article article = _articleRepository.GetDefault(a => a.ID == id);

            UpdateArticleDTO updateArticleDTO = _mapper.Map<UpdateArticleDTO>(article);

            updateArticleDTO.Categories = _categoryRepository.GetByDefaults
                (
                    selector: a => new GetCategoryDTO
                    {
                        ID = a.ID,
                        Name = a.Name
                    },
                     expression: a => a.Statu != Statu.Passive
                );
            return View(updateArticleDTO);
        }

        [HttpPost]
        public IActionResult Update(UpdateArticleDTO dto)
        {
            if (ModelState.IsValid)
            {
                Article article = _mapper.Map<Article>(dto);


                if (article.ImagePath != null)
                {
                    string imageName = dto.Image.Trim().Substring(17);

                    string deletedImage = Path.Combine(_webHostEnvironment.WebRootPath, "images", "articles", $"{imageName}");

                    if (System.IO.File.Exists(deletedImage))
                    {
                        System.IO.File.Delete(deletedImage);
                    }

                    using var image = Image.Load(dto.ImagePath.OpenReadStream());
                    image.Mutate(a => a.Resize(1000, 1000));

                    Guid guid = Guid.NewGuid();

                    image.Save($"wwwroot/images/articles/{guid}.jpg");

                    article.Image = ($"/images/articles/{guid}.jpg");

                }

                _articleRepository.Update(article);
                return RedirectToAction("List");
            }

            dto.Categories = _categoryRepository.GetByDefaults
                (
                    selector: a => new GetCategoryDTO
                    {
                        ID = a.ID,
                        Name = a.Name
                    },
                     expression: a => a.Statu != Statu.Passive
                );

            return View(dto);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            Article article = _articleRepository.GetDefault(a => a.ID == id);
            _articleRepository.Delete(article);
            return RedirectToAction("List");
        }

        // Detail
        public async Task<IActionResult> Detail(int id)
        {
            ArticleDetailVM articleDetailVM = new ArticleDetailVM()
            {
                Article = _articleRepository.GetDefault(a => a.ID == id),

            };

            articleDetailVM.Article.Category = _categoryRepository.GetDefault(a => a.ID == articleDetailVM.Article.CategoryID);

            articleDetailVM.Article.AppUser = _appUserRepository.GetDefault(a => a.ID == articleDetailVM.Article.AppUserID);

            articleDetailVM.userFollowedCategories = _categoryRepository.GetCategoryWithUser(articleDetailVM.Article.AppUser.ID);

            articleDetailVM.Mail = _userManager.Users.FirstOrDefault(a => a.Id == articleDetailVM.Article.AppUser.IdentityId).Email;

            articleDetailVM.Article.Likes = _likeRepository.GetLikes(a => a.ArticleID == articleDetailVM.Article.ID);

            articleDetailVM.Article.Comments = _commentRepository.GetDefaults(a => a.ArticleID == articleDetailVM.Article.ID && a.Statu != Statu.Passive);

            foreach (var item in articleDetailVM.Article.Comments)
            {
                item.AppUser = _appUserRepository.GetDefault(a => a.ID == item.AppUserID);
            }

            IdentityUser identityUser = await _userManager.GetUserAsync(User);
            articleDetailVM.ActiveAppUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            articleDetailVM.ActiveArticleID = id;

            _articleRepository.Read(articleDetailVM.Article);

            return View(articleDetailVM);
        }

        // Like
        public async Task<IActionResult> Like(int id)
        {
            Article article = _articleRepository.GetDefault(a => a.ID == id);

            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);


            Like like = new Like()
            {
                AppUser = appUser,
                AppUserID = appUser.ID,
                Article = article,
                ArticleID = article.ID
            };

            _likeRepository.Create(like);

            return RedirectToAction("Detail", new
            {
                id = id
            });
        }

        // Unlike
        public async Task<IActionResult> Unlike(int id)
        {
            Article article = _articleRepository.GetDefault(a => a.ID == id);

            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            AppUser appUser = _appUserRepository.GetDefault(a => a.IdentityId == identityUser.Id);

            Like like = _likeRepository.GetDefault(a => a.ArticleID == article.ID && a.AppUserID == appUser.ID);

            _likeRepository.Delete(like);
            return RedirectToAction("Detail", new { id = id });
        }

        // ListArticle Of Category
        public IActionResult ListWithFilter(int id)
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

                    expression: a => a.Statu != Statu.Passive && a.CategoryID == id,

                    include: a => a.Include(a => a.AppUser).Include(a => a.Category),

                    orderby: a => a.OrderByDescending(a => a.CreateDate)
                );

            ViewBag.AllCategory = _categoryRepository.GetDefaults(a => a.Statu != Statu.Passive);
            return View(articles);
        }

        // ListWithFilters
        public IActionResult ListWithFilters(List<int> categories)
        {
            if (categories.Count == 0)
            {
                return RedirectToAction("Index", "AppUser");
            }
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

            var newArticleList = articles.Where(article => categories.Contains(article.CategoryID)).ToList();

            ViewBag.AllCategory = _categoryRepository.GetDefaults(a => a.Statu != Statu.Passive);

            return View(newArticleList);

        }

    }
}
