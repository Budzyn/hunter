using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Enums;

namespace Hunter.Services.Interfaces
{
    public interface IActivityHelperService
    {
        void CreateAddedUserProfileActivity(UserProfile userProfile);
        void CreateAddedCandidateActivity(Candidate candidate);
        void CreateAddedVacancyActivity(Vacancy vacancy);
        void CreateAddedPoolActivity(Pool pool);
        void CreateUpdatedFeedbackActivity(Feedback feedback);
        void CreateAddedSpecialNoteActivity(SpecialNote specialNote);
        void CreateUpdatedSpecialNoteActivity(SpecialNote specialNote);
        void CreateUploadedTestActivity(Card card);
        void CreateUploadedResumeActivity(Candidate candidate);
        void CreateUploadedPhotoActivity(Candidate candidate);
        void CreateChangedCardStageActivity(Card card, Stage oldStage);
        void CreateUpdateCandidateResolution(Candidate candidate, Resolution oldResolution);
    }
}
