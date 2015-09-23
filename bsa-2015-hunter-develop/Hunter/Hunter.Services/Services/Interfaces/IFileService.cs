
using System.Collections.Generic;
using System.Net.Http;
using Hunter.DataAccess.Entities;
using Hunter.Services.Dto;

namespace Hunter.Services.Interfaces
{
    public interface IFileService
    {
        int Add(FileDto file);
        void Update(FileDto file);
        FileDto Get(int id);
        IEnumerable<FileDto> Get();
        void Delete(int id);
        FileDto DownloadFile(int id);
        byte[] GetPhoto(int id);
        FileDto GetResumeFileDto(int resumeId);
        void UploadPhotoFromUrl(string url, int candidateId);
        void SavePhoto(Candidate candidate, byte[] sourceBytes);
    }
}
