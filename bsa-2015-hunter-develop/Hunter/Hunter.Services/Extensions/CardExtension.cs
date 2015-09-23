using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;

namespace Hunter.Services.Extensions
{
    public static class CardExtension
    {
        public static CardDto ToCardDto(this Card card)
        {
            var dto = new CardDto
            {
                Id = card.Id,
                VacancyId = card.VacancyId,
                CandidateId = card.CandidateId,
                Added = card.Added,
                AddedByProfileId = card.AddedByProfileId,
                Stage = card.Stage
            };

            return dto;
        }

        public static Card ToCardModel(this CardDto dto, int? userProfileId)
        {
            return new Card()
            {
                CandidateId = dto.CandidateId,
                VacancyId = dto.VacancyId,
                Added = DateTime.UtcNow,
                Stage = dto.Stage,
                AddedByProfileId = userProfileId
            };
        }
    }
}
