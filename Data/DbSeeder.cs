using System;
using bookshop.Constant;
using Microsoft.AspNetCore.Identity;
namespace bookshop.Data
{
	public class DbSeeder
	{
		public static async Task SeedDefaultData(IServiceProvider service) {

			var userManager = service.GetService<UserManager<IdentityUser>>();
			var rolesManager = service.GetService<RoleManager<IdentityRole>>();

			// add roles
			await rolesManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
			await rolesManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

			// add user

			var admin = new IdentityUser
			{
				UserName = "admin@gmail.com",
				Email = "admin@gmail.com",
				EmailConfirmed = true
			};


			var isUserExists = await userManager.FindByEmailAsync(admin.Email);

			if(isUserExists is null)
			{
				await userManager.CreateAsync(admin, "P@ssw0rd");
				await userManager.AddToRoleAsync(admin,Roles.Admin.ToString());

			}
		}

	}
}

