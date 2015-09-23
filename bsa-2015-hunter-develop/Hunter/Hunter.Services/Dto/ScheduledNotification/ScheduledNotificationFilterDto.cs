using System.Collections.Generic;

namespace Hunter.Services.Dto.ScheduledNotification
{
    public class ScheduledNotificationFilterDto
    {
        public string Search { get; set; }
        public int[] NotificationTypes { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string OrderField { get; set; }
        public bool InvertOrder { get; set; }
    }
}
