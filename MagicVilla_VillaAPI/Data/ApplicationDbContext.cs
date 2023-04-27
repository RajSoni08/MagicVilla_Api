using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
        
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public  DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = " This is sample villa",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 1100,
                    Sqft = 100,
                    Amenity = "",
                    CreatedDate = DateTime.Now

                },
                new Villa()
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Details = " This is sample  Pool villa",
                    ImageUrl = "",
                    Occupancy = 88,
                    Rate = 500,
                    Sqft = 555,
                    Amenity = "",
                    CreatedDate = DateTime.Now

                }, new Villa()
                {
                    Id = 3,
                    Name = "Luxury Pool Villa",
                    Details = " This is Luxury Pool villa",
                    ImageUrl = "",
                    Occupancy = 8,
                    Rate = 5000,
                    Sqft = 444,
                    Amenity = "",
                    CreatedDate = DateTime.Now

                });
        }
    }
}
