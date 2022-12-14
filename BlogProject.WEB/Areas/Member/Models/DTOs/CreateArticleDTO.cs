using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.WEB.Areas.Member.Models.DTOs
{
    public class CreateArticleDTO
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [MinLength(3, ErrorMessage = "En az 3 karakter giriniz"), MaxLength(100, ErrorMessage = "En fazla 100 karakter giriniz")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [MinLength(3, ErrorMessage = "En az 3 karakter giriniz"), MaxLength(600, ErrorMessage = "En fazla 600 karakter giriniz")]
        public string Content { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [NotMapped]
        public IFormFile ImagePath { get; set; }

        [Required(ErrorMessage ="Bu alan boş bırakılamaz")]
        public int CategoryID { get; set; }
                
        public int AppUserID { get; set; }

        public List<GetCategoryDTO>? Categories { get; set; }

    }
}
