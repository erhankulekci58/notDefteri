using NotDefteri.DataAccessLayer.EntityFreamworks;
using NotDefteri.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotDefteri.DataAccessLayer.EntityFreamwork
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ND_User> Users { get; set; }
        public DbSet<ND_Note> Notes { get; set; }
        public DbSet<ND_Comment> Comments { get; set; }
        public DbSet<ND_Category> Categories { get; set; }
        public DbSet<ND_Liked> Likes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
    }

}
