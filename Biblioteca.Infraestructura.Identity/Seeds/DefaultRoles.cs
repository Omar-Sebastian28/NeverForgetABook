using Biblioteca.Infraestructura.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Infraestructura.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task CreateDefaultRoles(RoleManager<IdentityRole> identityRole) 
        {
            await identityRole.CreateAsync(new IdentityRole(RolesDefault.SuperAdmin.ToString()));
            await identityRole.CreateAsync(new IdentityRole(RolesDefault.Admin.ToString()));    
            await identityRole.CreateAsync(new IdentityRole(RolesDefault.Usuario.ToString()));
        }
    }
}
