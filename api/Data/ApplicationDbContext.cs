using api.Model;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Crux> Cruxes { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship
            modelBuilder.Entity<Crux>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById);

            // Seed user
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "seed-user-1",
                    Email = "admin@mtbcrux.com",
                    Name = "Admin User"
                }
            );

            // Seed initial data
            modelBuilder.Entity<Crux>().HasData(
                new Crux
                {
                    Id = 1,
                    Lat = "40.0150",
                    Lng = "-105.2705",
                    Level = 5,
                    Name = "The Bastille Crack",
                    Description = "The Bastille Crack is a Eldorado Canyon classic, featuring traditional climbing up a prominent dihedral.",
                    CreatedById = "seed-user-1"
                },
                new Crux { 
                    Id = 2, 
                    Lat = "59.2096", 
                    Lng = "9.6080", 
                    Level = 3, 
                    Name = "Vestveggen", 
                    Description = "Popular route with varied climbing on the west-facing wall",
                    CreatedById = "seed-user-1"
                },
                new Crux { 
                    Id = 3, 
                    Lat = "59.2100", 
                    Lng = "9.6085", 
                    Level = 4, 
                    Name = "Fossekallen", 
                    Description = "Technical face climbing along the waterfall route",
                    CreatedById = "seed-user-1"
                },
                new Crux { 
                    Id = 4, 
                    Lat = "59.2105", 
                    Lng = "9.6090", 
                    Level = 2, 
                    Name = "Granittblokka", 
                    Description = "Easy slab climbing on solid granite",
                    CreatedById = "seed-user-1"
                }
            );
        }
    }
} 