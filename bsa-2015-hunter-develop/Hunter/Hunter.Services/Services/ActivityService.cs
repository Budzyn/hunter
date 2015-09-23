using System.Collections.Generic;
using System.Linq;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using Hunter.Services.Dto;
using Hunter.Services.Extensions;
using Hunter.Services.Interfaces;

namespace Hunter.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public ActivityService(IActivityRepository activityRepository, IUserProfileRepository userProfileRepository)
        {
            _activityRepository = activityRepository;
            _userProfileRepository = userProfileRepository;
        }

        public IEnumerable<ActivityDto> GetAllActivities()
        {
            var activities = _activityRepository.Query();

            return activities.ToList().Select(item => item.ToActivityDto());
        }


        public ActivityDto GetActivityById(int id)
        {
            return _activityRepository.Get(id).ToActivityDto();
        }

        public IEnumerable<ActivityFilterDto> GetFilters()
        {
            var tags = _activityRepository.Query()
                .GroupBy(f => (int)f.Tag)
                .Select(f => new ActivityFilterDto()
                {
                    FilterId = 0,
                    Count = f.Count(),
                    Name = "",
                    OptionId = f.Key
                }).ToList();

            var users = _activityRepository.Query()
                .GroupBy(f => f.UserProfile)
                .Select(f => new ActivityFilterDto()
                {
                    FilterId = 1,
                    Count = f.Count(),
                    OptionId = f.Key.Id,
                    Name = f.Key.Alias
                }).ToList();

            var filters = tags.Concat(users);

            return filters;
        }

        public void AddActivity(ActivityDto entity)
        {
            _activityRepository.UpdateAndCommit(entity.ToActivity());
        }

        public void UpdateActivity(ActivityDto entity)
        {
            _activityRepository.UpdateAndCommit(entity.ToActivity());
        }


        public void DeleteActivityById(int id)
        {
            var activity = _activityRepository.Get(id);
            _activityRepository.DeleteAndCommit(activity);
        }

        public int GetUnreadActivitiesForUser(string login)
        {
            var user = _userProfileRepository.Get(login);

            return _activityRepository.GetCountOfActivitiesSince(user.LastViewedActivityId);
        }

        public void UpdateLastSeenActivity(string login, int lastSeenActivityId)
        {
            var userProfile = _userProfileRepository.Get(login);
            if (userProfile != null && userProfile.LastViewedActivityId < lastSeenActivityId)
            {
                userProfile.LastViewedActivityId = lastSeenActivityId;
                _userProfileRepository.UpdateAndCommit(userProfile);
            }
        }
    }
}
