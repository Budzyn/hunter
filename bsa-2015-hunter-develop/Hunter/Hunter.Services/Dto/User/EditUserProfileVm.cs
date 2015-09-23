using System;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.Services.Dto.User
{
    public class EditUserProfileVm
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Alias { get; set; }

        public string Position { get; set; }

        public string Added { get; set; }

        public static EditUserProfileVm Create(UserProfile user)
        {
            if (user == null)
                return null;
            return new EditUserProfileVm
            {
                Id = user.Id,
                Login = user.UserLogin,
                Position = user.Position,
                Alias = user.Alias,
                Added = user.Added.ToString("dd.MM.yy"),
            };
        }

        public void Map(UserProfile user, IUnitOfWork unitOfWork)
        {
            user.UserLogin = Login;
            user.Position = Position;
            user.Alias = Alias;
        }
    }
}
