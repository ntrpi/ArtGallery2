using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ArtGalleryDbContext : IdentityDbContext<ApplicationUser>
    {
        public ArtGalleryDbContext()
            : base("ArtGallery2", throwIfV1Schema: false)
        {
        }

        public static ArtGalleryDbContext Create()
        {
            return new ArtGalleryDbContext();
        }

        // Set up Inventory tables.
        public DbSet<Piece> pieces {
            get; set;
        }
        public DbSet<Form> forms {
            get; set;
        }

        public DbSet<Image> images {
            get; set;
        }

        public DbSet<Media> medias {
            get; set;
        }

        public DbSet<Technique> techniques {
            get; set;
        }

        public DbSet<Status> statuses {
            get; set;
        }
    }
}