using System.Collections.Generic;
using Hunter.Services.Dto;

namespace Hunter.Services.Interfaces
{
    public interface IActivityService
    {
        IEnumerable<ActivityDto> GetAllActivities();
        ActivityDto GetActivityById(int id);
        void AddActivity(ActivityDto entitiy);
        void UpdateActivity(ActivityDto entity);
        void DeleteActivityById(int id);

        int GetUnreadActivitiesForUser(string login);
        void UpdateLastSeenActivity(string login, int lastSeenActivityId);

        IEnumerable<ActivityFilterDto> GetFilters();
    }
}
