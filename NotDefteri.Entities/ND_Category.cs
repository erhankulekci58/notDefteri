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
    [Table("ND_Categories")]
    public class ND_Category : MyEntityBase
    {
        [DisplayName("Kategori"),
            Required(ErrorMessage = "{0} alanı boş geçilemez !"),
            StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter alabilir.")]
        public string Title { get; set; }

        [DisplayName("Açıklama"), 
            StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter alabilir.")]
        public string Description { get; set; }

        public virtual List<ND_Note> Notes { get; set; }

        public ND_Category()
        {
            Notes = new List<ND_Note>();
        }
    }
}
