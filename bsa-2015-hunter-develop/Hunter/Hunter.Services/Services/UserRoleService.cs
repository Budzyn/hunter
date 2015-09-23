using System;
using System.Collections.Generic;
using System.Linq;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using Hunter.Services.Dto;
using Hunter.Services.Extensions;

namespace Hunter.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UserRoleService( IUnitOfWork unitOfWork, ILogger logger, IUserRoleRepository userRoleRepository)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userRoleRepository = userRoleRepository;
        }

        public IEnumerable<UserRoleDto> GetAllUserRoles()
        {
            try
            {
                return _userRoleRepository.Query().ToList().ToUserRoleDtos();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new UserRoleDto[0];
            }
        }

        public UserRoleDto GetUserRoleById(int id)
        {
            try
            {
                return _userRoleRepository.Get(id).ToUserRoleDto();

            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new UserRoleDto();
            }
        }

        public UserRoleDto CreateUserRole(UserRoleDto userRole)
        {
            try
            {
                var role = userRole.ToUserRole();
                _userRoleRepository.UpdateAndCommit(role);
                return role.ToUserRoleDto();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public void UpdateUserRole(UserRoleDto userRole)
        {
            try
            {
                _userRoleRepository.Update(userRole.ToUserRole());
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public void DeleteUserRole(int id)
        {
            try
            {
                _userRoleRepository.Delete(_userRoleRepository.Get(id));
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public bool IsRoleExist(string name)
        {
            try
            {
                return _userRoleRepository.Query().Any(p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return false;
            }
        }

        public bool IsRoleExist(int id)
        {
            try
            {
                return _userRoleRepository.Query().Any(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return false;
            }
        }
    }
}
