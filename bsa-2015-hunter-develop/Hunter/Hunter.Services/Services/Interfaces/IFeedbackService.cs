using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Entities.Entites.Enums;
using Hunter.Services.Dto;
using Hunter.Services.Dto.ApiResults;

namespace Hunter.Services
{
    public interface IFeedbackService
    {
        IEnumerable<FeedbackDto> GetAllHrInterviews(int vid, int cid, string name);
        FeedbackDto SaveFeedback(FeedbackDto hrInterviewDto, string name);
        IEnumerable<FeedbackDto> GetTechInterview(int vacancyId, int candidateId, string name);
        FeedbackDto GetSummary(int vacancyId, int candidateId);
        FeedbackDto UpdateSuccessStatus(int feedbackId, SuccessStatus status, string name);
        IEnumerable<FeedbackDto> GetLastFeedbacks(int vacancyId, int candidateId);
        IEnumerable<FeedbackDto> GetFeedbacksHistory(int vacancyId, int candidateId);
    }
}
