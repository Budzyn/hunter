using System;
using System.Collections.Generic;
using System.Linq;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Entites.Enums;
using Hunter.DataAccess.Entities.Enums;
using Hunter.DataAccess.Interface.Base;
using Hunter.Services.Dto;
using Hunter.Services.Extensions;
using Hunter.Services.Interfaces;

namespace Hunter.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger _logger;
        private readonly IPoolRepository _poolRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IActivityHelperService _activityHelperService;

        public CandidateService(ICandidateRepository candidateRepository, ICardRepository cardRepository,
            IPoolRepository poolRepository, ILogger logger, IUserProfileRepository userProfileRepository, IActivityHelperService activityHelperService)
        {
            _candidateRepository = candidateRepository;
            _cardRepository = cardRepository;
            _logger = logger;
            _userProfileRepository = userProfileRepository;
            _poolRepository = poolRepository;
            _activityHelperService = activityHelperService;
        }

        public IEnumerable<CandidateDto> GetAllInfo()
        {
            try
            {
                var data = _candidateRepository.Query().ToList().Select(x => x.ToCandidateDto());
                return data;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public IQueryable<Candidate> Query()
        {
            try
            {
                return _candidateRepository.Query();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public Candidate Get(int id)
        {
            try
            {
                return _candidateRepository.Get(id);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public Candidate Get(Func<Candidate, bool> predicate)
        {
            try
            {
                return _candidateRepository.Get(predicate);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public CandidateDto GetInfo(int id)
        {
            try
            {
                var candidate = _candidateRepository.Get(id);
                if (candidate == null)
                {
                    return null;
                }
                return candidate.ToCandidateDto();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }

        }

        public IEnumerable<CandidateLongListDto> GetLongList(int vid)
        {
            try
            {
                var cardsLongList =
                    _cardRepository.QueryIncluding(c => c.Candidate.Pool, c => c.UserProfile)
                        .Where(card => card.VacancyId == vid)
                        .ToList()
                        .ToDto();// All().Where(card => card.VacancyId == vid).ToCandidateLongListDto().OrderByDescending(c => c.AddDate);
                return cardsLongList;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new List<CandidateLongListDto>();
            }
        }

        public CandidateLongListDetailsDto GetLongListDetails(int vid, int cid)
        {
            try
            {
                var candidate = _candidateRepository.Get(cid).ToCandidateLongListDetailsDto(vid);
                return candidate;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public IEnumerable<AddedByDto> GetCandidatesAddedBy()
        {
            try
            {
                var addedBy = _candidateRepository.Query()
                    .GroupBy(c => c.UserProfile)
                    .Select(c => new AddedByDto()
                    {
                        UserLogin = c.Key.UserLogin ?? "",
                        Alias = c.Key.Alias ?? "Nobody",
                        CountOfAddedCandidates = c.Count()
                    });

                return addedBy;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new List<AddedByDto>();
            }
        }

        public void UpdateShortFlag(int id, bool isShort)
        {
            var candidate = _candidateRepository.Get(id);
            if (candidate != null)
            {
                candidate.Shortlisted = isShort;
                _candidateRepository.UpdateAndCommit(candidate);
            }
        }

        public void UpdateResolution(int id, Resolution resolution)
        {
            var candidate = _candidateRepository.Get(id);
            var oldResolution = candidate.Resolution;
            candidate.Resolution = resolution;
            _candidateRepository.UpdateAndCommit(candidate);
            _activityHelperService.CreateUpdateCandidateResolution(candidate, oldResolution);
        }

        public void Add(CandidateDto dto, string name)
        {
            var candidate = new Candidate();
            dto.ToCandidateModel(candidate);

            var user = _userProfileRepository.Get(name);
            if (user != null)
            {
                candidate.AddedByProfileId = user.Id;
            }

            foreach (var item in dto.PoolNames)
            {
                var pool = _poolRepository.Get(x => x.Name == item);
                if (pool != null)
                {
                    candidate.Pool.Add(pool);
                }
            }
            try
            {
                candidate.AddDate = DateTime.Now;
                _candidateRepository.UpdateAndCommit(candidate);
                _activityHelperService.CreateAddedCandidateActivity(candidate);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw new Exception("Database save error!");
            }
        }

        public void Delete(Candidate candidate)
        {
            try
            {
                _candidateRepository.DeleteAndCommit(candidate);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public void Update(CandidateDto dto)
        {
            var candidate = _candidateRepository.Get(dto.Id);
            dto.ToCandidateModel(candidate);
            candidate.Pool.Clear();
            foreach (var item in dto.PoolNames)
            {
                var pool = _poolRepository.Get(x => x.Name == item);
                if (pool != null)
                {
                    candidate.Pool.Add(pool);
                }
            }
            try
            {
                _candidateRepository.UpdateAndCommit(candidate);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public void UpdateCandidatePool(int candidateId, int poolId, bool delete = false)
        {
            try
            {
                var candidate = _candidateRepository.Get(candidateId);
                var pool = _poolRepository.Get(poolId);
                if (!delete)
                {
                    if (candidate.Pool.Any(x => x.Id == poolId))
                    {
                        throw new Exception("This candidate is already in this pool");
                    }

                    candidate.Pool.Add(pool);
                    Update(candidate.ToCandidateDto());
                }
                else
                {
                    candidate.Pool.Remove(pool);
                    Update(candidate.ToCandidateDto());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public Dictionary<string, string> GetColors(int id)
        {
            try
            {
                var colors = _candidateRepository.Query()
                    .Where(x => x.Id == id)
                    .SelectMany(cand => cand.Pool, (cand, pool) => new {Name = pool.Name, Color = pool.Color})
                    .ToDictionary(x => x.Name.ToLower(), x => x.Color);

                return colors;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                throw ex;
            }
        }

        public void ChangeCandidatesPool(int oldId, int newId, int candidateId)
        {
            try
            {
                var oldPool = _poolRepository.Get(oldId);
                var newPool = _poolRepository.Get(newId);
                var candidate = _candidateRepository.Get(candidateId);

                candidate.Pool.Remove(oldPool);
                candidate.Pool.Add(newPool);

                _candidateRepository.UpdateAndCommit(candidate);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }
    }
}
