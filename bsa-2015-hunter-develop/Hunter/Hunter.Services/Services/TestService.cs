using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using Hunter.DataAccess.Interface.Repositories;
using Hunter.Services.Dto;
using Hunter.Services.Dto.ApiResults;
using Hunter.Services.Extensions;
using Hunter.Services.Services.Interfaces;

namespace Hunter.Services.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILogger _logger;

        public TestService(ITestRepository testRepository, ICardRepository cardRepository, 
            IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork,ILogger logger,
            IUserProfileRepository userProfileRepository)
        {
            _testRepository = testRepository;
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _feedbackRepository = feedbackRepository;
            _userProfileRepository = userProfileRepository;
        }

        public void ChangeCheckedTest(int testId)
        {
            var test = _testRepository.Get(testId);
            test.IsChecked = true;
            _testRepository.UpdateAndCommit(test);
        }

        public IEnumerable<TestForCheckDto> GetTestByUser(string login) 
        {
            var tests = _userProfileRepository
                .Query()
                .Where(e => e.UserLogin.ToLower() == login.ToLower())
                .FirstOrDefault()
                .Test
                .Select(e => e.ToTestForCheckDto());
            
            return tests;
        }

        public int GetCountNoChecked(string userName) 
        {
            var count = _userProfileRepository
                .Query()
                .Where(e => e.UserLogin.ToUpper() == userName.ToUpper())
                .FirstOrDefault()
                .Test.Where(e => e.IsChecked == false)
                .Count();
            
            return count;
        }

        public TestsResult GetAllCandidatesTests(int candidateId)
        {
            try
            {
//                var cardId = findCardId(candidateId);

                var tests = _testRepository
                    .Query()
                    .Where(x => x.Card.CandidateId == candidateId).ToList()
                    .Select(x => x.ToTestDto());

                return new TestsResult
                {
                    Tests = tests
                };
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public TestsResult GetCardTests(int vacancyId, int candidateId)
        {
            try
            {
                int cardId = findCardId(vacancyId, candidateId);

                var data = _cardRepository
                    .Query()
                    .Where(x => x.Id == cardId)
                    .Select(x => new {Tests = x.Test, Feedback = x.Feedback.FirstOrDefault(f => f.Type == 4)})
                    .First();

                var tests = data.Tests.Select(x => x.ToTestDto());

                return new TestsResult
                {
                    Tests = tests,
//                    Feedback = data.Feedback.ToFeedbackHrInterviewDto(),
                    CardId = cardId
                };
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public int AddTest(TestDto newTestDto)
        {
            Test test = new Test();
            newTestDto.ToTest(test);
            try
            {
                _testRepository.UpdateAndCommit(test);
                _unitOfWork.SaveChanges();

                return test.Id;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public void UpdateTest(TestDto newTestDto)
        {
            Test test = new Test();
            newTestDto.ToTest(test);

            try
            {
                _testRepository.Update(test);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public void UpdateFeedback(int testId, int feedbackId)
        {
            try
            {
                Test test = _testRepository.Get(testId);
                test.FeedbackId = feedbackId;

                _testRepository.Update(test);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public void DeleteTestById(int testId)
        {
            try
            {
                Test test = _testRepository.Get((long) testId);
                _testRepository.Delete(test);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        private int findCardId(int vacancyId, int candidateId)
        {
           return _cardRepository
                .Query()
                .Where(x => x.VacancyId == vacancyId && x.CandidateId == candidateId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public void UpdateComment(int id, string comment)
        {
            try
            {
                Test test = _testRepository.Get(id);
                test.Comment = comment;
                _testRepository.UpdateAndCommit(test);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                throw ex;
            }
        }

        public void AddCheckingToTest(int testId, int userId) 
        {
            var test = _testRepository.Get(testId);
            test.AssignedUserProfileId = userId;
            _testRepository.UpdateAndCommit(test);
        }

    }
}
