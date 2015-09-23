using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Extensions
{
    public static class TestForCheckExtension
    {
        public static TestForCheckDto ToTestForCheckDto(this Test test)
        {
            return new TestForCheckDto
            {
                Id = test.Id,
                Added = test.Added,
                IsChecked = test.IsChecked,
                VacancyId = test.Card.VacancyId,
                CandidateId = test.Card.CandidateId,
                FeedbackStatus = test.Feedback != null ? (int)test.Feedback.SuccessStatus : 0
            };
        }

        public static void ToTestForCheck(this TestForCheckDto testDto, Test test)
        {
            test.Id = testDto.Id;
            test.Added = testDto.Added;
            test.IsChecked = testDto.IsChecked;
        }
    }
}
