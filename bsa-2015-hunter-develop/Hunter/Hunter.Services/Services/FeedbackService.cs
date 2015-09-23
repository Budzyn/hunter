using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Entites.Enums;
using Hunter.DataAccess.Interface;
using Hunter.Services.Dto;
using Hunter.Services.Extensions;
using Hunter.Services.Dto.ApiResults;
using Hunter.DataAccess.Entities.Enums;
using Hunter.Services.Interfaces;

namespace Hunter.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILogger _logger;
        private readonly IActivityHelperService _activityHelperService;

        public FeedbackService(IFeedbackRepository feedbackRepository, ICardRepository cardRepository, ILogger logger, 
            IUserProfileRepository userProfileRepository, IActivityHelperService activityHelperService)
        {
            _feedbackRepository = feedbackRepository;
            _cardRepository = cardRepository;
            _logger = logger;
            _userProfileRepository = userProfileRepository;
            _activityHelperService = activityHelperService;
        }

        public IEnumerable<FeedbackDto> GetAllHrInterviews(int vid, int cid, string name)
        {

            try
            {
                IEnumerable<Feedback> feedbacks;

                if (vid == 0)
                {
                    feedbacks = _cardRepository
                        .Query()
                        .Where(e => e.CandidateId == cid)
                        .SelectMany(c => c.Feedback.Where(f => f.Type == 0 || f.Type == 1));
                }
                else if (vid == -1)
                {
                    feedbacks = _cardRepository
                        .Query()
                        .Where(e => e.CandidateId == cid)
                        .SelectMany(c => c.Feedback.Where(f => (f.Type == 0 || f.Type == 1) && f.UserProfile.UserLogin == name));
                }
                else
                {
                    feedbacks = _cardRepository
                        .Query()
                        .Where(e => e.VacancyId == vid && e.CandidateId == cid)
                        .SelectMany(c => c.Feedback.Where(f => f.Type == 0 || f.Type == 1));
                }

                return feedbacks.ToFeedbacksDto().OrderBy(f => f.Type);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }

        }

        public IEnumerable<FeedbackDto> GetTechInterview(int vacancyId, int candidateId, string name)
        {
            int type = (int)FeedbackType.TechFeedback;
            try
            {
                IEnumerable<Feedback> feedbacks;

                if (vacancyId == 0)
                {
                    feedbacks = _cardRepository
                        .Query()
                        .Where(e => e.CandidateId == candidateId)
                        .SelectMany(c => c.Feedback.Where(f => f.Type == type));
                }
                else if (vacancyId == -1)
                {
                    feedbacks = _cardRepository
                        .Query()
                        .Where(e => e.CandidateId == candidateId)
                        .SelectMany(c => c.Feedback.Where(f => f.Type == type && f.UserProfile.UserLogin == name));
                }
                else
                {
                    feedbacks = _cardRepository
                        .Query()
                        .Where(e => e.VacancyId == vacancyId && e.CandidateId == candidateId)
                        .SelectMany(c => c.Feedback.Where(f => f.Type == type));
                }

                return feedbacks.ToFeedbacksDto();

            }
            catch (Exception e)
            {
                _logger.Log(e);
                return null;
            }
             
        }

        public FeedbackDto GetSummary(int vacancyId, int candidateId)
        {
            try
            {
                var card = _cardRepository.Query()
                    .SingleOrDefault(e => e.VacancyId == vacancyId && e.CandidateId == candidateId);


                var summary = card.Feedback.FirstOrDefault(i => i.Type == (int)FeedbackType.Summary);
                if (summary == null)
                    return new FeedbackDto()
                    {
                        Id = 0,
                        Type = (int)FeedbackType.Summary,
                        CardId = card.Id,
                        Text = "",
                        Date = DateTime.Now,
                        UserName = ""
                    };

                return summary.ToFeedbackDto();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public FeedbackDto SaveFeedback(FeedbackDto feedbackDto, string name)
        {
            Feedback feedback;
            var userProfile = _userProfileRepository.Get(u => u.UserLogin.ToLower() == name.ToLower());
            try
            {

                if (feedbackDto.Id != 0)
                {
                    feedback = _feedbackRepository.Get(feedbackDto.Id);
                    if (feedback == null)
                    {
    //                    return Api.NotFound(hrInterviewDto.Id);
                        throw new Exception("Feedback not found");
                    }
                }
                else
                {
                    feedback = new Feedback();
                    feedback.Added = DateTime.Now;
                }

                feedback.ProfileId = userProfile != null ? userProfile.Id : (int?)null;
                feedbackDto.ToFeedback(feedback);
            
            
                _feedbackRepository.UpdateAndCommit(feedback);
                _activityHelperService.CreateUpdatedFeedbackActivity(feedback);
                var dto = feedback.ToFeedbackDto();
                return feedback.ToFeedbackDto();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
//                return Api.Error((long)feedback.Id, ex.Message);
                throw ex;
            }
        }

        public FeedbackDto UpdateSuccessStatus(int feedbackId, SuccessStatus status, string name)
        {
            try
            {
                var feedback = _feedbackRepository.Get(feedbackId);
                feedback.SuccessStatus = status;
                return SaveFeedback(feedback.ToFeedbackDto(), name);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public IEnumerable<FeedbackDto> GetLastFeedbacks(int vacancyId, int candidateId)
        {

            IEnumerable<Feedback> feedbacks = new List<Feedback>();
            try
            {

                if (vacancyId != 0)
                {
                    Card card = _cardRepository.Query()
                        .FirstOrDefault(c => c.CandidateId == candidateId && c.VacancyId == vacancyId);
                    if (card != null)
                        feedbacks = card.Feedback.GroupBy(f => f.Type).Select(f => f.Last());
                }
                else
                {
                    var cards = _cardRepository.Query()
                        .Where(c => c.CandidateId == candidateId);

                    feedbacks = cards.SelectMany(c => c.Feedback).ToList().GroupBy(f => f.Type).Select(f => f.Last());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }

            return feedbacks.ToFeedbacksDto();
        }

        public IEnumerable<FeedbackDto> GetFeedbacksHistory(int vacancyId, int candidateId)
        {
            IEnumerable<Feedback> feedbacks = new List<Feedback>();
            IEnumerable<Feedback> lastFeedbacks = new List<Feedback>();
            try
            {

                if (vacancyId != 0)
                {
                    Card card = _cardRepository.Query()
                        .FirstOrDefault(c => c.CandidateId == candidateId && c.VacancyId == vacancyId);
                    if (card != null)
                    {
                        lastFeedbacks = card.Feedback.GroupBy(f => f.Type).Select(f => f.Last());
                        feedbacks = card.Feedback.Where(f => !lastFeedbacks.Contains(f));
                    }
                       
                }
                else
                {
                    var cards = _cardRepository.Query()
                        .Where(c => c.CandidateId == candidateId);

                    lastFeedbacks = cards.SelectMany(c => c.Feedback).ToList().GroupBy(f => f.Type).Select(f => f.Last());
                    feedbacks = cards.SelectMany(c => c.Feedback).ToList().Where(f => !lastFeedbacks.Contains(f));
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }

            return feedbacks.ToFeedbacksDto();
        }
    }
}
