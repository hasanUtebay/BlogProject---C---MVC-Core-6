using BlogProject.Models.Entities.Concrrete;

namespace BlogProject.WEB.Models.VMs
{
    public class GetArticleWithUserVM
    {        
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public int? ReadingTime { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; } 
        public string UserFullName { get; set; }
        public string UserImage { get; set; }
        public string Image { get; set; }
        public int ArticleId { get; set; } 
        public int ReadCounter { get; set; }
        
    }
}
