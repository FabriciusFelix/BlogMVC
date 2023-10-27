using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // seed Roles(Userr,Admin,Superadmin)
            var userRoleId = "404e8c5d-b664-46b6-99fc-f72ba32783ac";
            var adminRoleId = "a29f552f-d9e5-4093-b6ce-6bc9894cc533";
            var superAdminRoleId = "aef79042-8fd3-4a52-b8b9-45d059e8fa56";
            var roles = new List<IdentityRole> { 
                new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdminUser
            var superAdminId = "888a6b47-3ad4-41a6-921d-a630b84256c3";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@fxblog.com",
                Email = "superadmin@fxblog.com",
                NormalizedEmail = "superadmin@fxblog.com".ToUpper(),

                NormalizedUserName = "superadmin@fxblog.com".ToUpper(),
                Id = superAdminId,
                ConcurrencyStamp = superAdminId
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "SuperAdmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);
            // Add All roles to SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId ,
                    UserId = superAdminId,

                },

                new IdentityUserRole<string>
                {
                    RoleId =  userRoleId,
                    UserId = superAdminId,

                },
                new IdentityUserRole<string>
                {
                    RoleId =  superAdminRoleId,
                    UserId = superAdminId,

                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
