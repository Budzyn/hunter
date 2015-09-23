using System;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.Services.Interfaces;

namespace Hunter.Services.Services
{
    public class ActivityHelperService : IActivityHelperService
    {
        private readonly IActivityPostService _activityPostService;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger _logger;
        
        public ActivityHelperService(IActivityPostService activityPostService, ICardRepository cardRepository, ILogger logger)
        {
            _activityPostService = activityPostService;
            _cardRepository = cardRepository;
            _logger = logger;
        }

        public void CreateAddedUserProfileActivity(UserProfile userProfile)
        {
            try
            {
                string message = string.Format("{0} has joined Hunter", userProfile.UserLogin);
                ActivityType type = ActivityType.User;
                Uri url = new Uri("#/user/edit/" + userProfile.Id, UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
            
        }

        public void CreateAddedCandidateActivity(Candidate candidate)
        {
            try
            {
                string message = string.Format("A new candidate has been added : {0} {1}", candidate.FirstName,
                    candidate.LastName);
                ActivityType type = ActivityType.Candidate;
                Uri url = new Uri("#/candidate/"+candidate.Id, UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateAddedVacancyActivity(Vacancy vacancy)
        {
            try
            {
                string message = string.Format("A new vacancy has been created : {0}", vacancy.Name);
                ActivityType type = ActivityType.Vacancy;
                Uri url = new Uri("#/vacancy/" + vacancy.Id, UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateAddedPoolActivity(Pool pool)
        {
            try
            {
                string message = string.Format("A new pool has been created : {0}", pool.Name);
                ActivityType type = ActivityType.Pool;
                Uri url = new Uri("#/pool", UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateUpdatedFeedbackActivity(Feedback feedback)
        {
            try
            {
                var card = _cardRepository.Get(x => x.Id == feedback.CardId);
                string feedbackType;
                string feedbackRoute;
                switch (feedback.Type)
                {
                    case 0:
                        feedbackType = "English";
                        feedbackRoute = "hrinterview";
                        break;
                    case 1:
                        feedbackType = "Personal";
                        feedbackRoute = "hrinterview";
                        break;
                    case 2:
                        feedbackType = "Technical";
                        feedbackRoute = "technicalinterview";
                        break;
                    case 3:
                        feedbackType = "Test";
                        feedbackRoute = "test";
                        break;
                    case 4:
                        feedbackType = "Summary";
                        feedbackRoute = "summary";
                        break;
                    default:
                        feedbackType = "";
                        feedbackRoute = "";
                        break;
                }

                string message = string.Format("{0} feedback for {1} {2} on {3} has been updated",
                    feedbackType, card.Candidate.FirstName, card.Candidate.LastName, card.Vacancy.Name);
                ActivityType type = ActivityType.Feedback;
                //todo: change activity url to /card/:id when the latter is implemented
                Uri url = new Uri(string.Format("#/vacancy/{0}/candidate/{1}?tab={2}",
                    card.VacancyId, card.CandidateId, feedbackRoute), UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateAddedSpecialNoteActivity(SpecialNote specialNote)
        {
            try
            {
                var card = _cardRepository.Get(x => x.CandidateId == specialNote.CandidateId &&  x.VacancyId == specialNote.VacancyId);
                string message = string.Format("A special note for {0} {1} on '{2}' has been added",
                card.Candidate.FirstName, card.Candidate.LastName, card.Vacancy.Name);
                ActivityType type = ActivityType.SpecialNote;
                Uri url = new Uri(string.Format("#/vacancy/{0}/candidate/{1}?tab={2}",
                    card.VacancyId, card.CandidateId, "specialnotes"), UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateUpdatedSpecialNoteActivity(SpecialNote specialNote)
        {
            try
            {
                var card = _cardRepository.Get(x => x.CandidateId == specialNote.CandidateId && x.VacancyId == specialNote.VacancyId);
                string message = string.Format("A special note for {0} {1} on '{2}' has been updated",
                card.Candidate.FirstName, card.Candidate.LastName, card.Vacancy.Name);
                ActivityType type = ActivityType.SpecialNote;
                Uri url = new Uri(string.Format("#/vacancy/{0}/candidate/{1}?tab={2}",
                    card.VacancyId, card.CandidateId, "specialnotes"), UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateUploadedResumeActivity(Candidate candidate)
        {
            try
            {
                string message = string.Format("A resume of {0} {1} has been uploaded ", candidate.FirstName,
                    candidate.LastName);
                ActivityType type = ActivityType.Resume;
                Uri url = new Uri("#/candidate/" + candidate.Id, UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateUploadedTestActivity(Card card)
        {
            try
            {
                string message = string.Format("A test for {0} {1} on {2} has been uploaded", card.Candidate.FirstName,
                    card.Candidate.LastName, card.Vacancy.Name);
                ActivityType type = ActivityType.Test;
                Uri url = new Uri(string.Format("#/vacancy/{0}/candidate/{1}?tab={2}",
                    card.VacancyId, card.CandidateId, "test"), UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateUploadedPhotoActivity(Candidate candidate)
        {
            try
            {
                string message = string.Format("A photo of {0} {1} has been uploaded ", candidate.FirstName,
                    candidate.LastName);
                ActivityType type = ActivityType.Photo;
                Uri url = new Uri("#/candidate/" + candidate.Id, UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateChangedCardStageActivity(Card card, Stage oldStage)
        {
            try
            {
                string message = string.Format("Card stage for candidate {0} {1} in vacancy '{2}' has been changed from '{3}' to '{4}'", 
                    card.Candidate.FirstName, card.Candidate.LastName, card.Vacancy.Name, oldStage.GetCustomDescription(), ((Stage)card.Stage).GetCustomDescription());
                ActivityType type = ActivityType.Vacancy;
                Uri url = new Uri("#/vacancy/" + card.VacancyId + "/candidate/" + card.CandidateId, UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }

        public void CreateUpdateCandidateResolution(Candidate candidate, Resolution oldResolution)
        {
            try
            {
                string message = string.Format("Resolution for {0} {1} has been changed from '{2}' to '{3}'",
                    candidate.FirstName, candidate.LastName, oldResolution.GetCustomDescription(), ((Resolution)candidate.Resolution).GetCustomDescription());
                ActivityType type = ActivityType.Candidate;
                Uri url = new Uri("#/candidate/"+candidate.Id, UriKind.Relative);
                _activityPostService.Post(message, type, url);
            }
            catch (Exception e)
            {
                _logger.Log("Creating activity exception : " + e.Message);
            }
        }
    }
}
