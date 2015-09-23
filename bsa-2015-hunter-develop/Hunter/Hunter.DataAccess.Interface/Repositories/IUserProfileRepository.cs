using System;
using System.Linq;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Interface
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        UserProfile Get(string login);
    }
}
