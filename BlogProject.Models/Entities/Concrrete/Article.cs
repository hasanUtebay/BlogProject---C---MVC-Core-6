using BlogProject.Models.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.Models.Entities.Concrrete
{
    public class Article : BaseEntity
    {

        public Article()
        {
            Likes = new List<Like>();
            Comments = new List<Comment>();
        }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImagePath { get; set; }


        public int ReadCounter { get; set; }

        [NotMapped]
        public int? ReadingTime => Content.Length / 50;

        // Navigation Property.
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        // Navigation Property.
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }

        // Navigation Property.
        public List<Like> Likes { get; set; }

        // Navigation Property.
        public List<Comment> Comments { get; set; }


    }
}