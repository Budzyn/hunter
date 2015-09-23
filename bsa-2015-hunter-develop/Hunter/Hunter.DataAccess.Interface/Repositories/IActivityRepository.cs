using System.Collections.Generic;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Interface
{
    public interface IActivityRepository : IRepository<Activity>
    {
        int GetCountOfActivitiesSince(int userLastSeenActivity);
        IList<Activity> GetLatestActivities();
    }
}
