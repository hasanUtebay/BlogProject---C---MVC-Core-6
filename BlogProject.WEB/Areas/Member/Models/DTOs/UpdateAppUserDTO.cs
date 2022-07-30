using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.WEB.Areas.Member.Models.DTOs
{
    public class UpdateAppUserDTO
    {
        public int ID { get; set; }

        public string IdentityID { get; set; }

        public string oldImage { get; set; }

        public string oldPassword { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [MinLength(3, ErrorMessage = "En az 3 karakter yazılmalıdır")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [MinLength(3, ErrorMessage = "En az 3 karakter yazılmalıdır")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [MinLength(3, ErrorMessage = "En az 3 karakter yazılmalıdır")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [MinLength(3, ErrorMessage = "En az 3 karakter yazılmalıdır")]
        [DataType(DataType.Password)] 
        public string Password { get; set; }

        public string? Image { get; set; } 
        
        [NotMapped]
        public IFormFile? ImagePath { get; set; }


        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [DataType(DataType.EmailAddress)] 
        public string Mail { get; set; }

        public string? oldPassword1 { get; set; }
        public string? oldPassword2 { get; set; }
        public string? oldPassword3 { get; set; }
    }
}
