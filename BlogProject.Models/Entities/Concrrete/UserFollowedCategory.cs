namespace BlogProject.Models.Entities.Concrrete
{
    public class UserFollowedCategory 
    {
        // Navigation Property
   
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }

 
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}