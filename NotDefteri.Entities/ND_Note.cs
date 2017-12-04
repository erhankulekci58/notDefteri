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
    [Table("ND_Notes")]
    public class ND_Note : MyEntityBase
    {
        [DisplayName("Başlık"),
            Required(ErrorMessage = "{0} alanı boş geçilemez !"),
            StringLength(60, ErrorMessage = "{0} alanı max. {1} karakter alabilir.")]
        public string Title { get; set; }

        [DisplayName("İçerik"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez !"), 
            StringLength(2000, ErrorMessage = "{0} alanı max. {1} karakter alabilir.")]
        public string Text { get; set; }

        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }

        [DisplayName("Beğeni")]
        public int LikeCount { get; set; }

        [DisplayName("Kategori")]
        public int CategoryId { get; set; }

        public virtual ND_User Owner { get; set; }
        public virtual ND_Category Category { get; set; }
        public virtual List<ND_Comment> Comments { get; set; }
        public virtual List<ND_Liked> Likes { get; set; }

        public ND_Note()
        {
            Comments = new List<ND_Comment>();
            Likes = new List<ND_Liked>();
        }
    }
}
