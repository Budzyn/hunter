using System.Collections.Generic;
using System.Linq;
using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;

namespace Hunter.DataAccess.Db
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public int GetCountOfActivitiesSince(int userLastSeenActivity)
        {
            var res = DataContext.Set<Activity>().Count(x => x.Id > userLastSeenActivity);
            return res;
        }

        public IList<Activity> GetLatestActivities()
        {
            return Query().OrderByDescending(x => x.Time).ToList();
        }
    }
}
