using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Repositories;
using Hunter.Services.Interfaces;
using File = System.IO.File;

namespace Hunter.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly ILogger _logger;
        private readonly ICandidateService _candidateService;
        private readonly IResumeRepository _resumeRepository;
        private readonly IActivityHelperService _activityHelperService;
        private readonly ICardRepository _cardRepository;
		private readonly ICandidateRepository _candidateRepository;
        private string _localStorage = System.Configuration.ConfigurationManager.AppSettings["FilesRootPath"];

        public FileService(IFileRepository fileRepository, ILogger logger, ICandidateService candidateService,
            IResumeRepository resumeRepository, IActivityHelperService activityHelperService, ICardRepository cardRepository,
			ICandidateRepository candidateRepository)
        {
            _fileRepository = fileRepository;
            _logger = logger;
            _candidateService = candidateService;
            _resumeRepository = resumeRepository;
            _candidateRepository = candidateRepository;
            _activityHelperService = activityHelperService;
            _cardRepository = cardRepository;
        }

        public int Add(FileDto file)
        {
            file.Added = DateTime.Now;

            //_logger.Log(string.Format("{0}, {1}", file.CandidateId, file.VacancyId));

            var formatedFileName = FormatFileName(file);
            switch (file.FileType)
            {
                case FileType.Resume:
                    file.Path = "Resume\\";
                    break;
                case FileType.Test:
                    file.Path = string.Format("Test\\{0}\\", file.VacancyId);
                    break;
                case FileType.Other:
                    file.Path = "Other\\";
                    break;
            }
            var fullPath = Path.Combine(_localStorage, file.Path);

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string fileName = Path.Combine(file.Path, formatedFileName);
            try
            {
                SaveFile(file.File, fileName);
                var newFile = file.ToFile();
                newFile.Path = fileName;
                _fileRepository.UpdateAndCommit(newFile);

                if (file.FileType == FileType.Resume)
                {
                    var candidate = _candidateService.Get(file.CandidateId);
                    if (candidate != null)
                    {
                        candidate.Resume.Add(new Resume()
                        {
                            FileId = newFile.Id
                        });

                        _candidateRepository.UpdateAndCommit(candidate);
                    }
                   _activityHelperService.CreateUploadedResumeActivity(candidate);                                            
                }
                if (file.FileType==FileType.Test)
                {
                    _activityHelperService.CreateUploadedTestActivity(_cardRepository.GetByCandidateAndVacancy(file.CandidateId, file.VacancyId));
                }

                return newFile.Id;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
            return -1;
        }

        private string FormatFileName(FileDto file)
        {
            return string.Format("{0}##{1}##{2}", file.CandidateId, file.Added.ToShortDateString(), file.FileName);
        }

        private void SaveFile(Stream file, string fileName)
        {
            using (var fileStream = File.Open(Path.Combine(_localStorage, fileName), FileMode.Create))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        private Stream LoadFile(string relativePath)
        {
            string path = Path.Combine(_localStorage, relativePath);
            if (!File.Exists(path)) return null;

            Stream stream = null;
            using (var fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                stream = new MemoryStream();
                fileStream.CopyTo(stream);
            }
            return stream;
        }

        public byte[] GetPhoto(int candidateId)
        {
            var array = _candidateService.Get(candidateId).Photo;
            return array;
        }

        public FileDto GetResumeFileDto(int resumeId)
        {
            var resume = _resumeRepository.Get(resumeId);
            var fileDto = _fileRepository.Get((int)resume.FileId).ToFileDto();
            return fileDto;
        }

        public void Update(FileDto file)
        {
            file.Added = DateTime.Now;
            var entity = _fileRepository.Get(file.Id);
            var path = Path.Combine(_localStorage, entity.Path);
            if (entity == null) 
                return;

            if (File.Exists(path))
                File.Delete(path);

            file.ToFile(entity);
            _fileRepository.UpdateAndCommit(entity);
            SaveFile(file.File, file.Path);
        }

        public FileDto Get(int id)
        {
            var entity = _fileRepository.Get(id);
            if (entity != null)
            {
                var dto = entity.ToFileDto();
                //dto.File = LoadFile(entity.Path);
                return dto;
            }
            return null;
        }

        public IEnumerable<FileDto> Get()
        {
            var files = _fileRepository.Query().ToList().Select(f => f.ToFileDto());
            //foreach (var file in files)
            //    file.File = LoadFile(file.Path);
            return files;
        }

        public void Delete(int id)
        {
            var entity = _fileRepository.Get(id);
            var path = Path.Combine(_localStorage, entity.Path);
            if (entity != null)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    _fileRepository.DeleteAndCommit(entity);
                }
            }
        }

        public FileDto DownloadFile(int id)
        {
            var entity = _fileRepository.Get(id);
            if (entity != null)
            {
                var dto = entity.ToFileDto();
                dto.File = LoadFile(dto.Path);
                dto.File.Position = 0;
                return dto;
            }
            return null;
        }

        public void UploadPhotoFromUrl(string source, int candidateId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var wr = WebRequest.Create(source);
                var response = wr.GetResponse();
                Stream stream = response.GetResponseStream();
                stream.CopyTo(ms);
                var candidate = _candidateService.Get(candidateId);
                SavePhoto(candidate, ms.ToArray());
            }        
        }

        public void SavePhoto(Candidate candidate, byte[] sourceBytes)
        {
            candidate.Photo = sourceBytes;
            _candidateService.Update(candidate.ToCandidateDto());
            _activityHelperService.CreateUploadedPhotoActivity(candidate);
        }
    }
}
