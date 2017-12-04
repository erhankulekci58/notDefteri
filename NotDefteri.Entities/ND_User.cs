using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace NotDefteri.Entities
{
    [Table("ND_Users")]
    public class ND_User : MyEntityBase
    {
        [DisplayName("İsim"),
            StringLength(25,ErrorMessage ="{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Soyadı"), 
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı adı"), 
            Required(ErrorMessage = "{0} girilmelidir !"),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-mail"), 
            Required(ErrorMessage = "{0} girilmelidir !"), 
            StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [DisplayName("Şifre"), 
            Required(ErrorMessage = "{0} girilmelidir !"), 
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [StringLength(30), ScaffoldColumn(false)]
        public string ProfileImageFilename { get; set; }

        [DisplayName("Aktivasyon")]
        public bool IsActive { get; set; }

        [DisplayName("Admin")]
        public bool IsAdmin { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid ActivatedGuid { get; set; }
        
        public virtual List<ND_Note> Notes { get; set; }
        public virtual List<ND_Comment> Comments { get; set; }
        public virtual List<ND_Liked> Likes { get; set; }
    }
}
