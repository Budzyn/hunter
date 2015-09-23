using System.Collections.Generic;
using Hunter.Services.Dto;

namespace Hunter.Services
{
    public interface IUserRoleService
    {
        IEnumerable<UserRoleDto> GetAllUserRoles();
        UserRoleDto GetUserRoleById(int id);
        UserRoleDto CreateUserRole(UserRoleDto userRole);
        void UpdateUserRole(UserRoleDto userRole);
        void DeleteUserRole(int id);
        bool IsRoleExist(string name);
        bool IsRoleExist(int id);
    }
}