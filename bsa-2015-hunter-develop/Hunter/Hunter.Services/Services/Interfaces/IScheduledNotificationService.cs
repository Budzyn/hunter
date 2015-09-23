using System.Collections.Generic;
using Hunter.Services.Dto.ScheduledNotification;

namespace Hunter.Services.Interfaces
{
    public interface IScheduledNotificationService
    {
        void Add(ScheduledNotificationDto notificationDto);
        void Update(ScheduledNotificationDto notificationDto);
        void Delete(int id);
        IList<ScheduledNotificationDto> Get(string userAlias);
        ScheduledNotificationDto Get(int id);
        IList<ScheduledNotificationDto> GetActive(string userLogin);
        void NotificationShown(int id);
        IList<ScheduledNotificationDto> GetCandidateNotifications(string userLogin, int candidateId);
        void Notify();
        PageDto<ScheduledNotificationDto> Get(string userAlias, ScheduledNotificationFilterDto filter);
    }
}
