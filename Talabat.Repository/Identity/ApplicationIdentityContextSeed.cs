using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
	public static class ApplicationIdentityContextSeed
	{
		public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Ahmed Nasr",
					Email = "ahmed.nasr@linkdev.com",
					UserName = "ahmed.nasr",
					PhoneNumber = "01122334455"
				}; 

				await userManager.CreateAsync(user,"P@ssw0rd");

			}
		}
	}
}
