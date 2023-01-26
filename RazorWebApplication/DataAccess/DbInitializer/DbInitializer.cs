using DataAccess.Data;
using Models;
using Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (! _roleManager.RoleExistsAsync(SD.StudentRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.StudentRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.InstructorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.SuperAdminRole)).GetAwaiter().GetResult();

                // create superAdmin user
                _userManager.CreateAsync(new AdminUser
                {
                    UserName = "superadmin@rckr.com",
                    Email = "superadmin@rckr.com",
                    EmailConfirmed = true,
                    FirstName = "Super",
                    LastName = "Admin"
                }, "SuperAdmin123*").GetAwaiter().GetResult();

                AdminUser superAdmin = _db.AdminUser.FirstOrDefault(u => u.Email == "superadmin@rckr.com");

                // map user with superadmin role
                _userManager.AddToRoleAsync(superAdmin, SD.SuperAdminRole).GetAwaiter().GetResult();


                // Create Admin user
                _userManager.CreateAsync(new AdminUser
                {
                    UserName = "admin@rckr.com",
                    Email = "admin@rckr.com",
                    EmailConfirmed = true,
                    FirstName = "Primary",
                    LastName = "Admin"
                }, "Admin123*").GetAwaiter().GetResult();

                AdminUser admin = _db.AdminUser.FirstOrDefault(u => u.Email == "admin@rckr.com");

                // map user with admin role
                _userManager.AddToRoleAsync(admin, SD.AdminRole).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
