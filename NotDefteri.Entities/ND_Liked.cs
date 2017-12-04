using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotDefteri.Entities
{
    [Table("ND_Liked")]
    public class ND_Liked
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public ND_Note Note { get; set; }
        public ND_User LikedUser { get; set; }
    }
}
