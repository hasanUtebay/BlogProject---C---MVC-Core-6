using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.WEB.Models.DTOs
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage ="Bu alan boş bırakılamaz")]
        [MinLength(3,ErrorMessage ="En az 3 karakter yazılmalıdır")]
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

        [Required]
        [NotMapped]
        public IFormFile ImagePath { get; set; }
    
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")] 
        [DataType(DataType.EmailAddress)] 
        public string Mail { get; set; }
    }
}
