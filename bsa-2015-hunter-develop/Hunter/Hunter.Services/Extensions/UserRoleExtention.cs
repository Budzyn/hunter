using System.Collections.Generic;
using System.Linq;
using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;

namespace Hunter.Services.Extensions
{
    public static class UserRoleExtention
    {
        public static IEnumerable<UserRoleDto> ToUserRoleDtos(this IEnumerable<UserRole> userRoles)
        {
            return userRoles.Select(r => new UserRoleDto()
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        public static UserRoleDto ToUserRoleDto(this UserRole userRole)
        {
            return new UserRoleDto()
            {
                Id = userRole.Id,
                Name = userRole.Name
            };
        }

        public static UserRole ToUserRole(this UserRoleDto userRoleDto)
        {
            return new UserRole()
            {
                Id = userRoleDto.Id,
                Name = userRoleDto.Name
            };
        }

    }
}
