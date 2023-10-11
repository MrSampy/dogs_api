using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class DogAPIDBContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        public DogAPIDBContext(DbContextOptions<DogAPIDBContext> options, bool ensureDeleted = false) : base(options)
        {
            if (ensureDeleted)
            {
                Database.EnsureDeleted();
            }
            Database.EnsureCreated();
        }

    }
}
