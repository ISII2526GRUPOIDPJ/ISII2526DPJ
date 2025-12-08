using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AppForSEII2526.API.Data
{
    public static class SeedData
    {
        public static void Initialize(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            dbContext.Database.EnsureCreated();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                };

                roleManager.CreateAsync(role).Wait();
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
                SeedTestUser(userManager);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creando usuario seed");
            }
        }

        public static void SeedTestUser(UserManager<ApplicationUser> userManager)
        {
            var existing = userManager.FindByNameAsync("test@uclm.es").Result;

            if (existing == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "test@uclm.es",
                    NormalizedUserName = "TEST@UCLM.ES",
                    Email = "test@uclm.es",
                    NormalizedEmail = "TEST@UCLM.ES",
                    EmailConfirmed = true,

                    // Columnas NOT NULL:
                    Name = "Test",
                    Surname = "User",
                    PhoneNumberConfirmed = false,    
                    TwoFactorEnabled = false,        
                    LockoutEnabled = true,           
                    AccessFailedCount = 0,           

                    // Columnas opcionales:
                    PhoneNumber = null,              
                    SecurityStamp = Guid.NewGuid().ToString(), 
                    ConcurrencyStamp = Guid.NewGuid().ToString() 
                };

                var result = userManager.CreateAsync(user, "Test123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
                else
                {
                    throw new Exception(
                        "Errores al crear usuario seed: "
                        + string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }
            }
        }
    }
}