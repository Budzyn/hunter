using System;
using System.Collections.Generic;
using System.Linq;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Entities.Entites.Enums;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using Hunter.Services.Extensions;
using Hunter.Services.Interfaces;

namespace Hunter.Services
{
    public class PoolService : IPoolService
    {
        private readonly IPoolRepository _poolRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IActivityHelperService _activityHelperService;
        public PoolService(IPoolRepository poolRepository, IUnitOfWork unitOfWork, ILogger logger,
            IActivityHelperService activityHelperService)
        {
            _poolRepository = poolRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _activityHelperService = activityHelperService;
        }

        public IEnumerable<PoolViewModel> GetAllPools()
        {
            try
            {
                return _poolRepository.Query().ToList().ToPoolViewModel();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new PoolViewModel[0];
            }
        }

        public PoolViewModel GetPoolById(int id)
        {
            try
            {
                return _poolRepository.Get(id).ToPoolViewModel();

            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new PoolViewModel();
            }
        }

        public PoolViewModel CreatePool(PoolViewModel poolViewModel)
        {
            try
            {
                // TODO seems like redundant conversions here
                var entity = poolViewModel.ToPoolModel();
                _poolRepository.UpdateAndCommit(entity);
                
                _activityHelperService.CreateAddedPoolActivity(entity);

                return entity.ToPoolViewModel();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        // TODO think of join add and edit methods
        public void UpdatePool(PoolViewModel poolViewModel)
        {
            try
            {
                _poolRepository.UpdateAndCommit(poolViewModel.ToPoolModel());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public void DeletePool(int id)
        {
            try
            {
                _poolRepository.DeleteAndCommit(_poolRepository.Get(id));
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public bool IsPoolNameExist(string name)
        {
            try
            {
                return _poolRepository.Query().Any(p => p.Name.ToLower() == name.ToLower());
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return false;
            }
        }

        public bool IsPoolExist(int id)
        {
            try
            {
                var res = _poolRepository.Query().Any(p => p.Id == id);
                return res;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return false;
            }
        }
        
        public bool IsPoolNameExistExceptCurrentPool(PoolViewModel pool)
        {
            try
            {
                return _poolRepository.Query().Any(p => (p.Name.ToLower() == pool.Name.ToLower() && p.Id != pool.Id));
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return false;
            }
        }
    }
}
