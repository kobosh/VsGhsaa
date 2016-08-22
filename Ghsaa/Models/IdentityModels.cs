using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ghsaa.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            

            return userIdentity;
        }

        public virtual MyUserProfile userProfile { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
       public DbSet<MyUserProfile> userPrfileInfos { get; set; }

        public DbSet<MyEvent> MyEvents { get; set; }
       public DbSet<Attendee> Attendees { get; set; }
     //  public DbSet<Video> Videos { get; set; }
       public DbSet<Message> Messages { get; set; }
       public DbSet<Reply> Replies { get; set; }
       //public static ApplicationDbContext Create()
       //{
       //    return new ApplicationDbContext();
       //}

    }
}