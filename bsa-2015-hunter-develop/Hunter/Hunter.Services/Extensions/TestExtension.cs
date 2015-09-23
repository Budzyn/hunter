using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;

namespace Hunter.Services.Extensions
{
    public static class TestExtension
    {
        public static TestDto ToTestDto(this Test test)
        {
            var feedbackDto = test.FeedbackId != null ? test.Feedback.ToFeedbackDto() : null;
            FileDto file = test.File != null ? test.File.ToFileDto() : null;

            return new TestDto
            {
                Id = test.Id,
                CardId = test.CardId,
                Comment = test.Comment,
                FeedbackId = test.FeedbackId,
                FileId = test.FileId,
                AssignedUserProfile = test.AssignedUserProfile == null ? null : test.AssignedUserProfile.ToUserProfileDto(),
                Added = test.Added,
                Url = test.Url,
                File = file,
                Feedback = feedbackDto,
                IsChecked = test.IsChecked
            };
        }

        public static void ToTest(this TestDto testDto, Test test)
        {
            test.Id = testDto.Id;
            test.CardId = testDto.CardId;
            test.Comment = testDto.Comment;
            test.FeedbackId = testDto.FeedbackId;
            test.FileId = testDto.FileId;
            test.Url = testDto.Url;
            test.Added = testDto.Added;
            test.IsChecked = test.IsChecked;

            if(test.File != null)
                testDto.File.ToFile(test.File = new File());

            if (testDto.Feedback != null)
                testDto.Feedback.ToFeedback((test.Feedback = new Feedback()));
        }
    }
}
