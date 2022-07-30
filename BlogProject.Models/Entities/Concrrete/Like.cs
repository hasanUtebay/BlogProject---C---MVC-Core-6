namespace BlogProject.Models.Entities.Concrrete
{

    public class Like 
    {
        // Navigation Property
        
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }
      
        public int ArticleID { get; set; }
        public Article Article { get; set; }
    }
}