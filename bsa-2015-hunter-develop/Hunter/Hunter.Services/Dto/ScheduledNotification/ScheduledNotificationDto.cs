using System;

namespace Hunter.Services.Dto.ScheduledNotification
{
    public class ScheduledNotificationDto
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public int UserProfileId { get; set; }
        public DateTime NotificationDate { get; set; }
        public int NotificationType { get; set; }
        public string Message { get; set; }
        public bool IsSent { get; set; }
        public bool IsShown { get; set; }
        public string UserLogin { get; set; }
    }
}
