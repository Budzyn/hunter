using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Entities;

namespace Hunter.Services.Dto.User
{
    public class UserProfileRowVm
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Alias { get; set; }

        public string Position { get; set; }

        public string Added { get; set; }

        public static UserProfileRowVm Create(UserProfile user)
        {
            return new UserProfileRowVm
            {
                Id = user.Id,
                Login = user.UserLogin,
                Alias = user.Alias,
                Position = user.Position
            };
        }
    }
}
