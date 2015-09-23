using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;

namespace Hunter.Services.Extensions
{
    static class SpecialNoteExtension
    {
        public static SpecialNoteDto ToDto(this SpecialNote specialNote)
        {
            var specialNotesDto = new SpecialNoteDto
            {
                Id = specialNote.Id,
                UserLogin = specialNote.UserProfile != null ? specialNote.UserProfile.UserLogin : "",
                Text = specialNote.Text,
                LastEdited = specialNote.LastEdited,
                CandidateId = specialNote.CandidateId,
                VacancyId = specialNote.VacancyId,
                UserAlias = specialNote.UserProfile != null ? specialNote.UserProfile.Alias : ""
            };

            return specialNotesDto;
        }

        public static SpecialNote ToEntity(this SpecialNoteDto specialNoteDto)
        {
            var specialNote = new SpecialNote()
            {
                Id = specialNoteDto.Id,
                Text = specialNoteDto.Text,
                LastEdited = specialNoteDto.LastEdited,
                VacancyId = specialNoteDto.VacancyId,
                CandidateId = specialNoteDto.CandidateId               
            };

            return specialNote;
        }
    }
}
