using System;
using System.Collections.Generic;
using System.Linq;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Enums;
using Hunter.Services.Dto;

namespace Hunter.Services.Interfaces
{
    public interface ICandidateService
    {
        Candidate Get(int id);
        Candidate Get(Func<Candidate, bool> predicat);
        IQueryable<Candidate> Query();
        IEnumerable<CandidateDto> GetAllInfo();
        CandidateDto GetInfo(int id);
        void Add(CandidateDto candidate, string name);
        void Delete(Candidate candidate);
        void Update(CandidateDto candidate);
        IEnumerable<CandidateLongListDto> GetLongList(int id);
        CandidateLongListDetailsDto GetLongListDetails(int vid, int cid);
        IEnumerable<AddedByDto> GetCandidatesAddedBy();
        void UpdateShortFlag(int id, bool isShort);
        void UpdateResolution(int id, Resolution resolution);
        void UpdateCandidatePool(int candidateId, int poolId, bool delete = false);
        Dictionary<string, string> GetColors(int id);
        void ChangeCandidatesPool(int oldId, int newId, int candidateId);
    }
}
