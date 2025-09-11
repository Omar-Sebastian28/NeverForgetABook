using Biblioteca.Infraestructura.Identity.Entities;
using Biblioteca.Infraestructura.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infraestructura.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task CreateDefaultAdmin(UserManager<AppUser> userManager)
        {
            AppUser user = new AppUser()
            {
                Nombre = "Sebastian",
                Apellido = "Joaquín M.",
                UserName = "admin22",
                Email = "omarsebastian@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "1234567890",
            };

            if (await userManager.Users.AllAsync(u => u.UserName != user.UserName || u.Id != user.Id)) 
            {            
                var result = await userManager.FindByEmailAsync(user.Email);
                if (result != null) 
                {
                    var cretedSuccedd = await userManager.CreateAsync(user, "Pa$$word123!");

                    if (cretedSuccedd.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RolesDefault.Admin.ToString());
                    }
                }
            }
        }
    }
}
