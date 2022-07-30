using BlogProject.Models.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.Entities.Concrrete
{
    public class AppUser : BaseEntity
    {

        public AppUser()
        {
            Articles = new List<Article>();
            Comments = new List<Comment>();
            Likes = new List<Like>();
            UserFollowedCategories = new List<UserFollowedCategory>();
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        private string _oldPassword1 = null;

        public string? OldPassword1
        {
            get { return _oldPassword1; }
            set { _oldPassword1 = value; }
        }

        private string? _oldPassword2 = null;

        public string? OldPassword2
        {
            get { return _oldPassword2; }
            set { _oldPassword2 = value; }
        }

        private string? _oldPassword3 = null;

        public string? OldPassword3
        {
            get { return _oldPassword3; }
            set { _oldPassword3 = value; }
        }


        public string IdentityId { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImagePath { get; set; }

        // Navigation Property.
        public List<Article> Articles { get; set; }

        // Navigation Property. 
        public List<Comment> Comments { get; set; }

        // Navigation Property.
        public List<Like> Likes { get; set; }

        // Navigation Property. 
        public List<UserFollowedCategory> UserFollowedCategories { get; set; }
    }
}
