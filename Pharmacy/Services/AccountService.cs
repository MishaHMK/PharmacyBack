using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Entities;

namespace Pharmacy.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        public async Task<User> GetUserAsync(string name)
        {
            var result = await _db.Users.Where(u => u.Name == name).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<string>> GetRoles()
        {
            var roles = new[]
            {
                Roles.Admin,
                Roles.Customer,
                Roles.Pharmacist
            };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var idRole = new IdentityRole(role);
                    await _roleManager.CreateAsync(idRole);
                }
            }

            var identityRoles = await _roleManager.Roles.ToListAsync();

            var roleList = new List<string>();

            foreach (var role in identityRoles)
            {
                roleList.Add(role.Name);
            }

            return roleList;
        }

        public async Task<string> GetUsername(string name)
        {
            var userName = await _db.Users.Where(u => u.Name == name).Select(x => x.Name).FirstOrDefaultAsync();
            return userName;
        }

        public async Task SaveAllAsync()
        {
            await _db.SaveChangesAsync();
        }


        public async Task<User> GetUserById(string id, string role)
        {

            var user = _db.Users.Where(x => x.Id == id)
               .Select(c => new User()
               {
                   Id = c.Id,
                   Name = c.Name,
                   FatherName = c.FatherName,
                   Surname = c.Surname,
                   Email = c.Email
               }).SingleOrDefault();


            return user;
        }

    }
}