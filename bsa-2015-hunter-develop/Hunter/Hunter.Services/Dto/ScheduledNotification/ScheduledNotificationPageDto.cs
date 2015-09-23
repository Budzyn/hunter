using System.Collections.Generic;

namespace Hunter.Services.Dto.ScheduledNotification
{
    public class ScheduledNotificationPageDto
    {
        public int TotalCount { get; set; }
        public IEnumerable<ScheduledNotificationDto> ScheduledNotifications { get; set; }
    }
}
