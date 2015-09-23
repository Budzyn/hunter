using System.Collections.Generic;
using Hunter.Services.Dto;
using Hunter.Services.Dto.ApiResults;

namespace Hunter.Services.Services.Interfaces
{
    public interface ITestService
    {
        TestsResult GetAllCandidatesTests(int candidateId);
        int AddTest(TestDto newTestDto);
        void UpdateTest(TestDto newTestDto);
        void DeleteTestById(int testId);
        TestsResult GetCardTests(int vacancyId, int candidateId);
        void UpdateFeedback(int testId, int feedbackId);
        void UpdateComment(int id, string comment);
        void AddCheckingToTest(int testId, int userId);
        int GetCountNoChecked(string userName);
        IEnumerable<TestForCheckDto> GetTestByUser(string login);
        void ChangeCheckedTest(int testId);
    }
}