using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Hunter.Services.Extensions
{
    static public class UserProfileExtension
    {
        static public UserProfileDto ToUserProfileDto(this UserProfile user)
        {
            var userDto = new UserProfileDto() 
            {
                Id = user.Id,
                Login = user.UserLogin,
                Alias = user.Alias
            };
            return userDto;
        }

        static public UserProfile ToUserProfile(this UserProfileDto userDto)
        {
            var userProfile = new UserProfile() 
            {
                Id = userDto.Id,
                UserLogin = userDto.Login,
                Alias = userDto.Alias
            };
            return userProfile;
        }

        static public IEnumerable<UserProfileDto> ToUserProfilesDto(this IEnumerable<UserProfile> users)
        {
            return users.Select(e => e.ToUserProfileDto());
        }
    }
}
