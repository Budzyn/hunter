using System.Collections.Generic;
using System;
using System.Linq;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Enums;
using Hunter.Services.Extensions;


namespace Hunter.Services
{
    public static class VacancyExtersion
    {
        public static VacancyDto ToVacancyDTO(this Vacancy vacancy)
        {
            var v = new VacancyDto
            {
                Id = vacancy.Id,
                Name = vacancy.Name,
                Status = vacancy.Status,
                StartDate = vacancy.StartDate,
                EndDate = vacancy.EndDate,
                Description = vacancy.Description,
//                PoolId = vacancy.PoolId,
                PoolNames = vacancy.Pool.Select(x => x.Name).ToList(),
                StatusName = ((Status)vacancy.Status).ToString(),
                UserLogin = vacancy.UserProfile != null ? vacancy.UserProfile.Alias : string.Empty
                
            };
            return v;
        }

        public static VacancyRowDto ToRowDto(this Vacancy vacancy)
        {
            var v = new VacancyRowDto
            {
                Id = vacancy.Id,
                Name = vacancy.Name,
                Status = vacancy.Status,
                StartDate = vacancy.StartDate,
                EndDate = vacancy.EndDate,
//                PoolName = vacancy.Pool.Name,
                PoolNames = vacancy.Pool.Select(x => x.Name).ToList(),
                AddedByName = vacancy.UserProfile != null ? vacancy.UserProfile.Alias : string.Empty,
                AddedById = vacancy.UserProfile != null ? vacancy.UserProfile.Id : 0
            };
            return v;
        }

        public static Vacancy ToVacancy(this VacancyDto vacancy)
        {
            var v = new Vacancy
            {
                Id = vacancy.Id,
                Name = vacancy.Name,
                Status = vacancy.Status,
                StartDate = vacancy.StartDate,
                EndDate = vacancy.EndDate,
                Description = vacancy.Description,
//                Pool = (ICollection<Pool>) vacancy.Pools.Select(x => x.ToPoolModel())
            };
            return v;
        }

        public static void ToVacancy(this VacancyDto vacancyDto, Vacancy vacancy)
        {
            vacancy.Id = vacancyDto.Id;
            vacancy.Name = vacancyDto.Name;
            vacancy.Status = vacancyDto.Status;
            vacancy.StartDate = vacancyDto.StartDate;
            vacancy.EndDate = vacancyDto.EndDate;
            vacancy.Description = vacancyDto.Description;
//            vacancy.Pool = (ICollection<Pool>) vacancyDto.Pools.Select(x => x.ToPoolModel());
        }

        public static VacancyLongListDto ToVacancyLongListDto(this Vacancy vacancy)
        {
            var vll = new VacancyLongListDto()
            {
               
                Id = vacancy.Id,
                Name = vacancy.Name,
                AddedByName = vacancy.UserProfile != null ? vacancy.UserProfile.UserLogin : string.Empty,
                PoolNames = vacancy.Pool.Select(x => x.Name)
                //PoolId = vacancy.PoolId
            };
            return vll;
        }
    }
}