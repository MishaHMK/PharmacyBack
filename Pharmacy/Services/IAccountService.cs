using Microsoft.AspNetCore.Mvc;
using Pharmacy.Entities;

namespace Pharmacy.Services
{
    public interface IAccountService
    {
        public Task SaveAllAsync();
        public Task<User> GetUserAsync(string name);
        public Task<List<string>> GetRoles();
        public Task<string> GetUsername(string name);

        Task<User> GetUserById(string id, string role);
    }
}
