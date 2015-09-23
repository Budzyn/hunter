using System;
using System.Collections.Generic;
using System.Linq;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using Hunter.Services.Extensions;
using Hunter.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using Hunter.DataAccess.Entities;
using System.IO;
using System.Text;
using Hunter.Services.Dto.ScheduledNotification;

namespace Hunter.Services
{
    public class ScheduledNotificationService : IScheduledNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IScheduledNotificationRepository _scheduledNotificationRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public ScheduledNotificationService(
            IUnitOfWork unitOfWork,
            IScheduledNotificationRepository scheduledNotificationRepository,
            IUserProfileRepository userProfileRepository
            )
        {
            _unitOfWork = unitOfWork;
            _scheduledNotificationRepository = scheduledNotificationRepository;
            _userProfileRepository = userProfileRepository;
        }

        public void Add(ScheduledNotificationDto notificationDto)
        {
            var userProfile = _userProfileRepository.Get(p => p.UserLogin == notificationDto.UserLogin);
            if (userProfile == null) return;
            var notification = notificationDto.ToScheduledNotification();
            notification.UserProfileId = userProfile.Id;
            _scheduledNotificationRepository.UpdateAndCommit(notification);
        }

        public void Update(ScheduledNotificationDto notificationDto)
        {
            var notification = _scheduledNotificationRepository.Get(notificationDto.Id);
            if (notification != null)
            {
                notificationDto.ToScheduledNotification(notification);
                _scheduledNotificationRepository.UpdateAndCommit(notification);
            }
        }

        public void Delete(int id)
        {
            var notification = _scheduledNotificationRepository.Get(id);
            if (notification != null)
                _scheduledNotificationRepository.DeleteAndCommit(notification);
        }

        public IList<ScheduledNotificationDto> Get(string userLogin)
        {
            var userProfile = _userProfileRepository.Get(p => p.UserLogin == userLogin);
            var notifications = _scheduledNotificationRepository.Query().Where(n => n.UserProfile.Id == userProfile.Id).ToList();
            return notifications.Select(item => item.ToScheduledNotificationDto()).ToList();
        }

        public IList<ScheduledNotificationDto> GetActive(string userLogin)
        {
            var currentDate = DateTime.UtcNow;
            var userProfile = _userProfileRepository.Get(p => p.UserLogin == userLogin);
            var notifications = _scheduledNotificationRepository.Query().Where(n => n.UserProfile.Id == userProfile.Id && n.NotificationDate < currentDate && !n.IsShown).ToList();
            return notifications.Select(item => item.ToScheduledNotificationDto()).ToList();
        }

        public ScheduledNotificationDto Get(int id)
        {
            var notification = _scheduledNotificationRepository.Get(id);
            if (notification != null)
                return notification.ToScheduledNotificationDto();
            return null;
        }

        public void NotificationShown(int id)
        {
            var notification = _scheduledNotificationRepository.Get(id);
            notification.IsShown = true;
            _scheduledNotificationRepository.UpdateAndCommit(notification);
        }

        public IList<ScheduledNotificationDto> GetCandidateNotifications(string userLogin, int candidateId)
        {
            var userProfile = _userProfileRepository.Get(p => p.UserLogin == userLogin);
            var notifications = _scheduledNotificationRepository.Query().Where(n => n.UserProfileId == userProfile.Id && n.CandidateId == candidateId).ToList();
            return notifications.Select(item => item.ToScheduledNotificationDto()).ToList();
        }

        public void Notify()
        {
            //throw new NotImplementedException();
            var hunterHost = "localhost:53147";
            var notificationDate = DateTime.UtcNow.Date.AddDays(1);
            var notifications = _scheduledNotificationRepository.Query().Where(n => n.NotificationDate < notificationDate && !n.IsSent).OrderBy(n => n.UserProfileId).ToList();
            var currentEmail = string.Empty;
            var body = string.Empty;
            var idList = new List<ScheduledNotification>();
            for (var i = 0; i < notifications.Count; ++i)
            {
                var n = notifications[i];
                idList.Add(n);
                if (i == 0)
                    currentEmail = n.UserProfile.UserLogin;

                if (currentEmail != n.UserProfile.UserLogin)
                {
                    currentEmail = n.UserProfile.UserLogin;
                    SendMail(currentEmail, body, idList);
                    body = string.Empty;
                }
                body += string.Format("{0}<br/><a href='{5}/#/candidate/{3}'>{1} {2}</a><br/>{4}<br/><br/>",
                                             n.NotificationDate,
                                             n.Candidate.FirstName,
                                             n.Candidate.LastName,
                                             n.CandidateId,
                                             n.Message,
                                             hunterHost);
            }
            if (currentEmail != string.Empty)
                SendMail(currentEmail, body, idList);
        }

        private void SendMail(string to, string body, List<ScheduledNotification> ids)
        {
            try
            {
                var from = new MailAddress("hunter.bsa2015@mail.ru", "Hunter Application");
                var subject = string.Format("Hunter Scheduled Notifications {0}", DateTime.UtcNow.Date.ToShortDateString());
                using (var mail = new MailMessage())
                {
                    mail.IsBodyHtml = true;
                    mail.From = from;
                    mail.To.Add(new MailAddress(to));
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.Subject = subject;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.Body = body;
                    using (var client = new SmtpClient())
                    {
                        client.Host = "smtp.mail.ru";
                        client.Port = 25;
                        client.Credentials = new NetworkCredential(from.Address, "hunterapplication");
                        client.EnableSsl = true;
                        client.Send(mail);
                        UpdateSentNotifications(ids);
                    }
                }
            }
            catch
            { }
        }

        private void UpdateSentNotifications(List<ScheduledNotification> notifications)
        {
            notifications.ForEach(n => n.IsSent = true);
            _unitOfWork.SaveChanges();
            notifications.Clear();
        }


        public PageDto<ScheduledNotificationDto> Get(string userLogin, ScheduledNotificationFilterDto filter)
        {
            IQueryable<ScheduledNotification> query = _scheduledNotificationRepository.QueryIncluding(v => v.UserProfile);
            query = query.Where(n => n.UserProfile.UserLogin == userLogin);
            if (filter.NotificationTypes.Any())
            {
                var notofocationTypes = filter.NotificationTypes.Select(nt => nt).ToList();
                query = query.Where(n => notofocationTypes.Contains((int)n.NotificationType));
            }

            IOrderedQueryable<ScheduledNotification> orderedQuery = null;
            if (filter.OrderField == "notificationDate")
            {
                orderedQuery = filter.InvertOrder ? query.OrderByDescending(v => v.NotificationDate) : query.OrderBy(v => v.NotificationDate);
            }
            else
            {
                orderedQuery = query.OrderByDescending(v => v.NotificationDate);
            }

            var countRecords = query.Count();
            var notificationsFiltered = orderedQuery.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToList();
            var list = notificationsFiltered.Select(item => item.ToScheduledNotificationDto()).ToList();
            return new PageDto<ScheduledNotificationDto>
            {
                TotalCount = countRecords,
                Rows = list
            };
        }
    }
}