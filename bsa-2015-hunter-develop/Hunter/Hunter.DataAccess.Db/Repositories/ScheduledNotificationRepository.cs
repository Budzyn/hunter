using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;

namespace Hunter.DataAccess.Db
{
    public class ScheduledNotificationRepository : Repository<ScheduledNotification>, IScheduledNotificationRepository
    {
        public ScheduledNotificationRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
