﻿using System;
﻿using Hunter.Services.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Entities;
﻿using Hunter.DataAccess.Entities.Entites.Enums;

namespace Hunter.Services.Extensions
{
    public static class FeedbackExtension
    {
        public static IEnumerable<FeedbackDto> ToFeedbacksDto(this IEnumerable<Feedback> feedbacks)
        {
            return feedbacks.Select(f => new FeedbackDto()
            {
                Id = f.Id,
                CardId = f.CardId,
                Type = f.Type,
                Text = f.Text,
                Date = f.Edited == null ? f.Added : f.Edited.Value,
                SuccessStatus = (int)f.SuccessStatus,
                UserAlias = f.UserProfile!=null ? f.UserProfile.Alias : "" ,
                UserName = f.UserProfile != null
                    ? f.UserProfile.UserLogin.Substring(0, f.UserProfile.UserLogin.IndexOf("@"))
                    : "",
                Vacancy = f.Card.Vacancy.Name
            }).OrderBy(f => f.Type);
        }

        public static FeedbackDto ToFeedbackDto(this Feedback feedback)
        {
            return new FeedbackDto() 
            {
                Id = feedback.Id,
                CardId = feedback.CardId,
                Type = feedback.Type,
                Text = feedback.Text,
                SuccessStatus = (int)feedback.SuccessStatus,
                Date = feedback.Edited == null
                        ? feedback.Added
                        : feedback.Edited.Value,
                UserName = feedback.UserProfile != null
                    ? feedback.UserProfile.UserLogin.Substring(0, feedback.UserProfile.UserLogin.IndexOf("@"))
                    : "",
                UserAlias = feedback.UserProfile != null ? feedback.UserProfile.Alias : "" 
            };
        }

        public static void ToFeedback(this FeedbackDto hrInterviewDto, Feedback feedback)
        {
            feedback.CardId = hrInterviewDto.CardId;
            feedback.Type = hrInterviewDto.Type;
            feedback.Text = hrInterviewDto.Text;
            feedback.SuccessStatus = (SuccessStatus) hrInterviewDto.SuccessStatus;
            if (hrInterviewDto.Id != 0)
            {
                feedback.Edited = DateTime.Now;
            }
            else
            {
                feedback.Added = DateTime.Now;
            }
        }
    }
}
