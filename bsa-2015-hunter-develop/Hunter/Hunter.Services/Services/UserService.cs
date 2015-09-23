using System.Collections.Generic;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;
using Hunter.DataAccess.Interface.Repositories;
using Hunter.Services.Dto;
using Hunter.Services.Extensions;
using System.Linq;

namespace Hunter.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPoolService _poolService;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IUserRoleRepository userRoleRepository,
            IUserProfileRepository userProfileRepository,
            IPoolService poolService
                )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userRoleRepository = userRoleRepository;
            _userProfileRepository = userProfileRepository;
            _poolService = poolService;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.Query().ToList();
        }

        public User GetUserByName(string name)
        {
            return _userRepository.Get(u => u.UserName.ToLower() == name.ToLower());
        }

        public User GetUserById(int id)
        {
            return _userRepository.Get(id);
        }

        public void CreateUser(User user)
        {
            _userRepository.UpdateAndCommit(user);
        }

        public void AddToRole(User user, string roleName)
        {
            var role = _userRoleRepository.Get(r => r.Name.ToLower() == roleName.ToLower());
            if (role != null)
            {
                user.RoleId = role.Id;
                _userRepository.Update(user);
                _unitOfWork.SaveChanges();
            }
        }

        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
            _unitOfWork.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
        }

        public FilterInfoDto GetFilterInfo(string roleName)
        {
            //var users = _userRepository.Query()
            //    .Where(e => e.UserRole.Name.ToLower() == roleName.ToLower())
            //    .Select(e => _userProfileRepository.All().Where(up => up.UserLogin == e.Login).FirstOrDefault())
            //    .ToUserProfilesDto();
            var users = _userRepository.Query().Where(e => e.UserRole.Name.ToLower() == roleName.ToLower()).ToList();
            IList<UserProfile> userProfiles = new List<UserProfile>();

            foreach (var user in users)
            {
                var userProfile = _userProfileRepository.Query().Where(e => e.UserLogin == user.UserName).FirstOrDefault();
                if (userProfile != null)
                    userProfiles.Add(userProfile);
            }
                
            var pools = _poolService.GetAllPools();

            return new FilterInfoDto() 
            { 
                Users = userProfiles.ToUserProfilesDto(),
                Pools = pools
            };
        }
    }
}
