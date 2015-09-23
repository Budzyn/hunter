using System;
using System.Threading;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Entites.Enums;
using Hunter.DataAccess.Interface;
using Hunter.Services.Interfaces;

namespace Hunter.Services
{
    public class ActivityPostService : IActivityPostService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public ActivityPostService(IActivityRepository activityRepository, IUserProfileRepository userProfileRepository)
        {
            _activityRepository = activityRepository;
            _userProfileRepository = userProfileRepository;
        }

        public void Post(string message, ActivityType tag, Uri url = null)
        {
            var activity = new Activity
            {
                Message = message,
                Tag = tag,
                Url = url != null ? url.ToString() : null,
                UserProfileId = _userProfileRepository.Get(x=>x.UserLogin==Thread.CurrentPrincipal.Identity.Name).Id,
                Time = DateTime.UtcNow
            };
            _activityRepository.UpdateAndCommit(activity);
        }
    }
}