using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotDefteri.Entities
{
    public class MyEntityBase
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Oluşturulma zamanı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez !")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Değiştirilme zamanı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez !")]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Değiştiren"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez !"), 
            StringLength(30, ErrorMessage = "{0} alanı max. {1} karakter alabilir.")]
        public string ModifiedUserName { get; set; }
    }
}
