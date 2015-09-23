using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;
using System.Collections.Generic;

namespace Hunter.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByName(string name);
        User GetUserById(int id);
        void CreateUser(User user);
        void AddToRole(User user, string roleName);
        void DeleteUser(User user);
        void UpdateUser(User user);
        FilterInfoDto GetFilterInfo(string roleName);
    }
}