using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotDefteri.Entities
{
    [Table("ND_Comments")]
    public class ND_Comment : MyEntityBase
    {
        [Required,StringLength(300)]
        public string Text { get; set; }

        public virtual ND_Note Note { get; set; }
        public virtual ND_User Owner { get; set; }
    }
}
