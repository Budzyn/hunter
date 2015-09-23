using System;
using System.Collections.Generic;
using System.Linq;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using Hunter.Services.Dto.Vacancy;
using Hunter.Services.Interfaces;

namespace Hunter.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger _logger;
        private readonly IActivityHelperService _activityHelperService;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPoolRepository _poolRepository;

        public VacancyService(
            IVacancyRepository vacancyRepository,
            ICandidateRepository candidateRepository,
            ICardRepository cardRepository,
            ILogger logger,
            IUnitOfWork unitOfWork,
            IActivityHelperService activityHelperService,
            IUserProfileRepository userProfileRepository,
            IPoolRepository poolRepository)
        {
            _vacancyRepository = vacancyRepository;
            _candidateRepository = candidateRepository;
            _cardRepository = cardRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _activityHelperService = activityHelperService;
            _userProfileRepository = userProfileRepository;
            _poolRepository = poolRepository;
        }
        public IList<VacancyRowDto> Get()
        {
            var vacancies = _vacancyRepository.QueryIncluding(v => v.Pool).OrderByDescending(v => v.StartDate).ToList();
            return vacancies.Select(item => item.ToRowDto()).ToList();
        }
        public PageDto<VacancyRowDto> Get(VacancyFilterParams filterParams)
        {
            IQueryable<Vacancy> query = _vacancyRepository.QueryIncluding(v => v.Pool, v => v.UserProfile);

            if (filterParams.Pools.Any())
            {
                var selectedPools = filterParams.Pools.Select(Int32.Parse).ToList();
//                query = query.Where(vac => selectedPools.Contains(vac.PoolId));
                query = query.Where(vac => selectedPools.Any(x => vac.Pool.Any(p => p.Id == x)));//??
            }

            if (filterParams.Statuses.Any())
            {
                var selectedStatuses = filterParams.Statuses.Select(Int32.Parse).ToList();
                query = query.Where(vac => selectedStatuses.Contains(vac.Status));
            }
            if (filterParams.Statuses.Any())
            {
                var selectedStatuses = filterParams.Statuses.Select(Int32.Parse).ToList();
                query = query.Where(vac => selectedStatuses.Contains(vac.Status));
            }

            if (filterParams.AddedByArray.Any())
            {
                var selectedCreators = filterParams.AddedByArray.Select(Int32.Parse).ToList();
                query = query.Where(vac => selectedCreators.Any(x=> vac.UserProfileId == x));
            }

            if (!string.IsNullOrWhiteSpace(filterParams.Filter))
            {
                var nameFilter = filterParams.Filter.ToLowerInvariant();
                query = query.Where(vac => vac.Name.Contains(nameFilter));
            }
            IOrderedQueryable<Vacancy> orderedQuery = null;
            if (filterParams.SortColumn == "startDate")
            {
                orderedQuery = filterParams.ReverceSort ? query.OrderByDescending(v => v.StartDate) : query.OrderBy(v => v.StartDate);
            }
            else if (filterParams.SortColumn == "name")
            {
                orderedQuery = filterParams.ReverceSort ? query.OrderByDescending(v => v.Name) : query.OrderBy(v => v.Name);
            }
            else
            {
                orderedQuery = query.OrderByDescending(v => v.StartDate);
            }

            var countRecords = query.Count();

            var vacanciesFiltered = orderedQuery.Skip((filterParams.Page - 1) * filterParams.PageSize).Take(filterParams.PageSize).ToList();

            var list = vacanciesFiltered.Select(item => item.ToRowDto()).ToList();
            return new PageDto<VacancyRowDto>
            {
                TotalCount = countRecords,
                Rows = list
            };
        }

        public VacancyDto Get(int id)
        {
            var vacancy = _vacancyRepository.Get(id);
            if (vacancy != null)
                return vacancy.ToVacancyDTO();
            return null;
        }

        public void Add(VacancyDto dto)
        {
            if (dto == null) return;
            var vacancy = dto.ToVacancy();
            var user = _userProfileRepository.Get(x => x.UserLogin == dto.UserLogin);
            vacancy.UserProfileId = user != null ? user.Id : (int?) null;

            //!!!!!
            vacancy.Pool = new List<Pool>();
            foreach (string poolName in dto.PoolNames)
            {
                vacancy.Pool.Add(_poolRepository.Query().FirstOrDefault(x => x.Name == poolName));
            }

            _vacancyRepository.UpdateAndCommit(vacancy);
            _activityHelperService.CreateAddedVacancyActivity(vacancy);
        }

        public void Update(VacancyDto entity)
        {
            var vacancy = _vacancyRepository.Get(entity.Id);
            entity.ToVacancy(vacancy);

            //!!!!
            vacancy.Pool.Clear();
            foreach (string poolName in entity.PoolNames)
            {
                vacancy.Pool.Add(_poolRepository.Query().FirstOrDefault(x => x.Name == poolName));
            }

            _vacancyRepository.UpdateAndCommit(vacancy);
        }

        public void Delete(int id)
        {
            var vacancy = _vacancyRepository.Get(id);
            if (vacancy != null)
            {
                _vacancyRepository.Delete(vacancy);
                _unitOfWork.SaveChanges();
            }

        }

        public VacancyLongListDto GetLongList(int id)
        {
            try
            {
                var vacancyLongList = _vacancyRepository.Get(id).ToVacancyLongListDto();

                return vacancyLongList;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new VacancyLongListDto();
            }
        }

        public IEnumerable<AddedByDto> GetLongListAddedBy(int vid)
        {
            try
            {
                var addedByLongList = _cardRepository.Query().Where(c => c.VacancyId == vid)
                    .GroupBy(c => c.UserProfile)
                    .Select(c => new AddedByDto()
                    {
                        UserLogin = c.Key.UserLogin ?? "",
                        Alias = c.Key.Alias,
                        CountOfAddedCandidates = c.Count()
                    });

                return addedByLongList;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new List<AddedByDto>();
            }
        }

        public IEnumerable<AddedByDto> GetVacanciesAddedBy()
        {
            try
            {
                var addedBy = _vacancyRepository.Query()
                    .GroupBy(c => c.UserProfile)
                    .Select(c => new AddedByDto()
                    {
                        Id = c.Key.Id,
                        UserLogin = c.Key.UserLogin ?? "",
                        Alias = c.Key.Alias ?? "Nobody"
                    });

                return addedBy;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new List<AddedByDto>();
            }
        }

        public IEnumerable<VacancyByStateDto> GetVacancyByState(int id)
        {
            try
            {
                var vacancy = _vacancyRepository.Query()
                    .Where(v => v.Status == id)
                    .Select(v => new VacancyByStateDto()
                    {
                        Id = v.Id,
                        Name = v.Name
                    });

                return vacancy;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new List<VacancyByStateDto>();
            }
        }

        public Dictionary<string, string> GetColors(int vacancyId)
        {
            try
            {
                return _vacancyRepository.Query()
                    .First(x => x.Id == vacancyId)
                    .Pool.ToDictionary(x => x.Name.ToLower(), x => x.Color);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }
    }
}