using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using Hunter.Services.Extensions;
using Hunter.Services.Interfaces;
using Hunter.DataAccess.Entities;

namespace Hunter.Services
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ISpecialNoteRepository _specialNoteRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly ITestRepository _testRepository;
        private readonly ILogger _logger;
        private readonly IActivityHelperService _activityHelperService;

        public CardService(ICardRepository cardRepository, IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork, ILogger logger, IActivityHelperService activityHelperService, ISpecialNoteRepository specialNoteRepository, IFeedbackRepository feedbackRepository, IInterviewRepository interviewRepository, ITestRepository testRepository)
        {
            _cardRepository = cardRepository;
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _activityHelperService = activityHelperService;
            _specialNoteRepository = specialNoteRepository;
            _feedbackRepository = feedbackRepository;
            _interviewRepository = interviewRepository;
            _testRepository = testRepository;
        }

        public CardDto GetCard(int id)
        {
            return _cardRepository.Get(id).ToCardDto();
        }

        public void AddCards(IEnumerable<CardDto> dtoCards, string name)
        {
            //if (dtoCards == null) return;

            //var newCard = new Card();

            var userProfile = _userProfileRepository.Get(u => u.UserLogin.ToLower() == name.ToLower());

            var userProfileId = userProfile != null ? userProfile.Id : (int?)null;

            foreach (var dto in dtoCards)
            {
                if (_cardRepository.Query().Any(c => (c.CandidateId == dto.CandidateId && c.VacancyId == dto.VacancyId)))
                {
                    continue;
                }

                _cardRepository.UpdateAndCommit(dto.ToCardModel(userProfileId));
            }
        }

        public bool UpdateCardStage(int vid, int cid, int stage)
        {
            var card = _cardRepository.Get(c => c.VacancyId == vid && c.CandidateId == cid);
            if (card == null) return false;
            Stage oldStage = (Stage) card.Stage;
            card.Stage = stage;
            _cardRepository.UpdateAndCommit(card);
            _activityHelperService.CreateChangedCardStageActivity(card, oldStage);
            return true;
        }

        public int GetCardStage(int vid, int cid)
        {
            var card = _cardRepository.Get(c => c.VacancyId == vid && c.CandidateId == cid);
            if (card == null) return 0;
            return card.Stage;
        }

        public bool IsCardExist(int vid, int cid)
        {
            try
            {
                return _cardRepository.Query().Any(c => c.VacancyId == vid && c.CandidateId == cid);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return false;
            }
        }

        public void DeleteCard(int vid, int cid)
        {
            try
            {
                _cardRepository.DeleteAndCommit(_cardRepository.Get(c => c.VacancyId == vid && c.CandidateId == cid));

            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public void DeleteAllInfo(int vid, int cid)
        {
            try
            {
                var card = _cardRepository.Query().FirstOrDefault(c => c.VacancyId == vid && c.CandidateId == cid);
                if (card != null)
                {
                    var tests = card.Test;
                    foreach (var test in tests)
                    {
                        _testRepository.DeleteAndCommit(test);
                    }
                    
                    var feedbacks = card.Feedback;
                    foreach (var feedback in feedbacks)
                    {
                        _feedbackRepository.DeleteAndCommit(feedback);
                    }

                    var interviews = card.Interview;
                    foreach (var interview in interviews)
                    {
                        _interviewRepository.DeleteAndCommit(interview);
                    }
                }

                var specialNotes = _specialNoteRepository.Query().Where(n => n.VacancyId == vid && n.CandidateId == cid);
                
                if (specialNotes.Any())
                {
                    foreach (var specialNote in specialNotes)
                    {
                        _specialNoteRepository.DeleteAndCommit(specialNote);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public IEnumerable<AppResultCardDto> GetApplicationResults(int cid)
        {
            try
            {
                var appResults = _cardRepository.Query()
                    .Where(c => c.CandidateId == cid)
                    .Select(c => new AppResultCardDto()
                {
                        CardId = c.Id,
                    VacancyId = c.VacancyId,
                    Stage = c.Stage,
                    Date = c.Added,
                            Vacancy = c.Vacancy.Name
                        })
                    .OrderByDescending(c => c.Date);

                return appResults;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return new List<AppResultCardDto>();
            }
        }

        public AppResultCardDto GetCardInfo(int vid, int cid)
        {
            try
            {
                var appResults = _cardRepository.Query()
                    .Where(c => c.VacancyId == vid && c.CandidateId == cid)
                    .Select(c => new AppResultCardDto()
                    {
                        CardId = c.Id,
                        VacancyId = c.VacancyId,
                        Stage = c.Stage,
                        Date = c.Added,
                    }).FirstOrDefault();

                return appResults;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw new Exception("no card found!");
            }
        }

        public bool IsCardUsed(int vid, int cid)
        {
            try
            {
                var card = _cardRepository.Query().FirstOrDefault(c => c.VacancyId == vid && c.CandidateId == cid);

                if (card != null)
                {
                    var feedback = card.Feedback.Any();
                    var interview = card.Interview.Any();
                    var test = card.Test.Any();

                    if (feedback || interview || test)
                    {
                        return true;
                    }
                }

                var specialNote = _specialNoteRepository.Query().Any(n => n.VacancyId == vid && n.CandidateId == cid);
                return specialNote;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return false;
            }
        }
    }
}
