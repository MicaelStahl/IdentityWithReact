using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace DataAccessLibrary.Database
{
    /// <summary>
    /// The class that contains the initializer that seeds the database on first startup.
    /// </summary>
    public class DbInitializer
    {
        /// <summary>
        /// The initializer that seeds the database at a first startup.
        /// </summary>
        /// <exception cref="None"></exception>
        /// <param name="context">The <value>DbContext</value> used in the application.</param>
        /// <param name="_userManager">The default UserManager used in the application.</param>
        /// <param name="_roleManager">The default RoleManager used in the application.</param>
        public static void Initializer(ReactDbContext context, UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            context.Database.EnsureCreated();

            #region Role Creation

            // If a role with name "Administrator" doesn't exist, create it.
            if (_roleManager.FindByNameAsync("Administrator").Result == null)
            {
                IdentityRole role = new IdentityRole("Administrator");

                _roleManager.CreateAsync(role).Wait();
            }

            // If a role with name "NormalUser" doesn't exist, create it.
            if (_roleManager.FindByNameAsync("NormalUser").Result == null)
            {
                IdentityRole role = new IdentityRole("NormalUser");

                _roleManager.CreateAsync(role).Wait();
            }

            #endregion Role Creation

            #region User Creation

            // If a user with the email "Administrator@context.com" doesn't exist, create it.
            if (_userManager.FindByEmailAsync("Administrator@context.com").Result == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "Administrator@context.com",
                    FirstName = "Administrator",
                    LastName = "Admin",
                    Age = 31,
                    Email = "Administrator@context.com",
                    EmailConfirmed = true,
                    PhoneNumber = "123456789",
                    IsAdmin = true
                };

                var result = _userManager.CreateAsync(user, "Password!23").Result;

                if (result.Succeeded)
                {
                    // Add to the roles previously created.
                    _userManager.AddToRolesAsync(user, new string[] { "Administrator", "NormalUser" }).Wait();
                }
            }

            // If a user with the email "NormalUser@context.com" doesn't exist, create it.
            if (_userManager.FindByEmailAsync("NormalUser@context.com").Result == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "NormalUser@context.com",
                    FirstName = "NormalUser",
                    LastName = "User",
                    Age = 35,
                    Email = "NormalUser@context.com",
                    EmailConfirmed = true,
                    PhoneNumber = "123456789",
                    IsAdmin = false
                };

                var result = _userManager.CreateAsync(user, "Password!23").Result;

                if (result.Succeeded)
                {
                    // Add user to "NormalUser" role since well... it's a normal user.
                    _userManager.AddToRoleAsync(user, "NormalUser").Wait();
                }
            }

            #endregion User Creation

            #region Product Creation

            #region Person Creation

            // Checks if any Person models exists in database, creates some if not.
            if (!context.People.Any())
            {
                // Adds a large amount of models to test around with.
                var people = new Person[]
                {
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson1", Age = 23, City = "TESTTOWN", Email = "Test.Testsson1@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson2", Age = 53, City = "TOWN", Email = "Test.Testsson2@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson3", Age = 21, City = "TEST", Email = "Test.Testsson3@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson4", Age = 57, City = "DOWNTOWN", Email = "Test.Testsson4@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson5", Age = 11, City = "UPTOWN", Email = "Test.Testsson5@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson6", Age = 103, City = "RIGHTTOWN", Email = "Test.Testsson6@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson7", Age = 7, City = "LEFTTOWN", Email = "Test.Testsson7@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson8", Age = 12, City = "NORTHTOWN", Email = "Test.Testsson8@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson9", Age = 65, City = "SOUTHTOWN", Email = "Test.Testsson9@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson10", Age = 45, City = "WESTTOWN", Email = "Test.Testsson10@context.com", PhoneNumber = "123456789" },
                    new Person { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Testsson11", Age = 32, City = "EASTTOWN", Email = "Test.Testsson11@context.com", PhoneNumber = "123456789" },
                };

                // Adds them all to database.
                context.People.AddRange(people);

                // Saves database.
                context.SaveChanges();
            }

            #endregion Person Creation

            #endregion Product Creation
        }
    }
}