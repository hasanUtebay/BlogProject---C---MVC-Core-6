using BlogProject.Models.Entities.Abstract;

namespace BlogProject.Models.Entities.Concrrete
{
    public class Category : BaseEntity 
    {
        
        public Category()
        {
            Articles = new List<Article>();
            UserFollowedCategories = new List<UserFollowedCategory>();
        }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation Property

        public List<Article> Articles { get; set; }

       
        public List<UserFollowedCategory> UserFollowedCategories { get; set; }
    }
}