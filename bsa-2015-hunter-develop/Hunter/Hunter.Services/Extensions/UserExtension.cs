using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Hunter.Services.Extensions
{
    static public class UserExtension
    {
        static public UserDto ToUserDto(this User user)
        {
            var userDto = new UserDto() 
            {
                Id = user.Id,
                Name = user.UserName
            };
            return userDto;
        }

        static public User ToUser(this UserDto userDto)
        {
            var user = new User() 
            {
                Id = userDto.Id,
                Login = userDto.Name
            };
            return user;
        }

        static public IEnumerable<UserDto> ToUsersDto(this IEnumerable<User> users)
        {
            return users.Select(e => e.ToUserDto());
        }
    }
}
