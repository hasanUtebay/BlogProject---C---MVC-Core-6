using BlogProject.Models.Entities.Abstract;

namespace BlogProject.Models.Entities.Concrrete
{
    public class Comment : BaseEntity 
    {
        public string Text { get; set; }

        // Navigation Property
       
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }
               
        public int ArticleID { get; set; }
        public Article Article { get; set; }

    }
}