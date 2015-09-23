using System.Linq;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Enums;
using Hunter.Services.Dto;
using System;
using System.Collections.Generic;
using Hunter.Services.Extensions;

namespace Hunter.Services
{
    public static class CandidateExtensions
    {
        public static CandidateDto ToCandidateDto(this Candidate candidate)
        {
            double experiance = candidate.YearsOfExperience ?? 0;
            var resumes = candidate.Resume ?? new List<Resume>();
            var dto = new CandidateDto()
            {
                Id = candidate.Id,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Email = candidate.Email,
                CurrentPosition = candidate.CurrentPosition,
                Company = candidate.Company,
                Location = candidate.Location,
                Skype = candidate.Skype,
                Phone = candidate.Phone,
                Linkedin = candidate.Linkedin ?? "",
                Salary = candidate.Salary,
                YearsOfExperience = Math.Round(experiance + candidate.CalculateYearsOfExperiance(), 1),
                //Resumes = resumes.Select(r => new ResumeDto()
                Resumes = candidate.Resume != null ? candidate.Resume.Select(r => new ResumeDto()
                {
                    Id = r.Id,
                    FileId = r.FileId,
                    Uploaded = r.File.Added,
                    FileName = r.File.FileName,
                    
                    DownloadUrl = "api/file/download/" + r.FileId
                }) : new HashSet<ResumeDto>(),
                //ResumeSummary = candidate.ResumeSummary,
                AddedByProfileId = candidate.AddedByProfileId,
                AddedBy = candidate.AddedByProfileId.HasValue ? candidate.UserProfile.UserLogin : "",
                AddDate = candidate.AddDate,
                Cards = candidate.Card.Select(x => x.ToCardDto()).ToList(),
                PoolNames = candidate.Pool.Select(x => x.Name).ToList(),
                
                Resolution = (int)candidate.Resolution,
                ShortListed = candidate.Shortlisted,
                
                Origin = (int)candidate.Origin,
                DateOfBirth = candidate.DateOfBirth,
                
                PhotoUrl = "api/fileupload/pictures/" + candidate.Id,
                UserAlias = candidate.UserProfile != null ? candidate.UserProfile.Alias : "",
                
                LastResumeUrl = candidate.Resume.Count > 0 ? "api/file/open/" + candidate.Resume.LastOrDefault().FileId : ""
            };
            return dto;
        }

        public static void ToCandidateModel(this CandidateDto dto, Candidate candidate)
        {
            candidate.Id = dto.Id;
            candidate.FirstName = dto.FirstName;
            candidate.LastName = dto.LastName;
            candidate.Email = dto.Email;
            candidate.CurrentPosition = dto.CurrentPosition;
            candidate.Company = dto.Company;
            candidate.Location = dto.Location;
            candidate.Skype = dto.Skype;
            candidate.Phone = dto.Phone;
            candidate.Linkedin = dto.Linkedin;
            candidate.Salary = dto.Salary;
            candidate.ResumeSummary = dto.ResumeSummary;
            candidate.YearsOfExperience = dto.YearsOfExperience;
            candidate.Resolution = (Resolution)dto.Resolution;
            candidate.Shortlisted = dto.ShortListed;
            candidate.Origin = (Origin)dto.Origin;
            candidate.DateOfBirth = dto.DateOfBirth;
            candidate.EditDate = DateTime.Now;
        }

        public static IEnumerable<CandidateLongListDto> ToDto(this IEnumerable<Card> cards)
        {
            return cards.Select(c => new CandidateLongListDto()
            {
                Id = c.CandidateId,
                FirstName = c.Candidate != null ? c.Candidate.FirstName : "",
                LastName = c.Candidate != null ? c.Candidate.LastName : "",
                Email = c.Candidate != null ? c.Candidate.Email : "",
                Company = c.Candidate != null ? c.Candidate.Company : "",
                Location = c.Candidate != null ? c.Candidate.Location : "",
                Salary = c.Candidate != null ? c.Candidate.Salary : 0,
                YearsOfExperience = c.Candidate != null ? c.Candidate.YearsOfExperience : 0,
                Stage = c.Stage,
                Resolution = c.Candidate != null ? (int)c.Candidate.Resolution : 0,
                AddedBy = c.UserProfile != null ? c.UserProfile.UserLogin : "",
                AddDate = c.Added,
                PhotoUrl = "api/fileupload/pictures/" + c.CandidateId,
                PoolNames = c.Candidate.Pool.Select(x => x.Name),
                Shortlisted = c.Candidate != null && c.Candidate.Shortlisted,
                UserAlias = c.UserProfile != null ? c.UserProfile.Alias : ""
            });
        }

        
        public static CandidateLongListDetailsDto ToCandidateLongListDetailsDto(this Candidate candidate, int vid)
        {
            
            var card = candidate.Card.FirstOrDefault(c => c.CandidateId == candidate.Id && c.VacancyId == vid);

            return new CandidateLongListDetailsDto
            {
                Id = candidate.Id,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                
                PhotoUrl = "api/fileupload/pictures/" + candidate.Id,
                Salary = candidate.Salary,
                
                Resolution = (int)candidate.Resolution,
                Linkedin = candidate.Linkedin,
                Stage = card != null ? card.Stage : 0,
                Shortlisted = candidate.Shortlisted,
                UserAlias = candidate.UserProfile != null ? candidate.UserProfile.Alias : "",
                LastResumeUrl = candidate.Resume.Count > 0 ? "api/file/open/" + candidate.Resume.LastOrDefault().FileId : "",
                PoolNames = candidate.Pool.Select(x => x.Name)
            };
        }

        public static IQueryable<CandidateDto> MapToDto(this IQueryable<Candidate> candidate)
        {
            return candidate.Select(c =>

                new CandidateDto()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CurrentPosition = c.CurrentPosition,
                    Company = c.Company,
                    Location = c.Location,
                    Skype = c.Skype,
                    Phone = c.Phone,
                    Linkedin = c.Linkedin,
                    Salary = c.Salary,
                    
                    YearsOfExperience = c.YearsOfExperience,
                    AddedByProfileId = c.AddedByProfileId,
                    AddedBy = c.AddedByProfileId.HasValue ? c.UserProfile.UserLogin : "",
                    AddDate = c.AddDate,
                    Cards = c.Card.Select(x =>
                    new CardDto
                    {
                        Id = x.Id,
                        VacancyId = x.VacancyId,
                        CandidateId = x.CandidateId,
                        Added = x.Added,
                        AddedByProfileId = x.AddedByProfileId,
                        Stage = x.Stage
                    }
                    ),
                    PoolNames = c.Pool.Select(x => x.Name),
                   
                    Resolution = (int)c.Resolution,
                    ShortListed = c.Shortlisted,
                   
                    Origin = (int)c.Origin,
                    DateOfBirth = c.DateOfBirth,
                    PhotoUrl = "api/fileupload/pictures/" + c.Id,
                    
                    UserAlias = c.UserProfile != null ? c.UserProfile.Alias : ""
                }
                );
        }

    }
}