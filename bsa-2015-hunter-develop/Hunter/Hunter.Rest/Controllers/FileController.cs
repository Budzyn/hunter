using Hunter.Services;
using Hunter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;

namespace Hunter.Rest.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof (IEnumerable<FileDto>))]
        public HttpResponseMessage Get()
        {
            try
            {
                var files = _fileService.Get();
                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

//        [HttpGet]
//        [Route("download/{id:int}")]
//        public IHttpActionResult GetFile(int fileId)
//        {
//            try
//            {
//                FileDto fileDto = _fileService.DownloadFile(fileId);
//                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
//                response.Content = new StreamContent(fileDto.File);
//                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
//
//                return ResponseMessage(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof (FileDto))]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var file = _fileService.Get(id);
                return Request.CreateResponse(HttpStatusCode.OK, file);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("resume/{id:int}")]
        [ResponseType(typeof (FileDto))]
        public HttpResponseMessage GetCandidateResume(int id)
        {
            try
            {
                var fileDto = _fileService.GetResumeFileDto(id);
                return Request.CreateResponse(HttpStatusCode.OK, fileDto);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody] FileDto value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var id = _fileService.Add(value);
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, -1);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public HttpResponseMessage Put(int id, [FromBody] FileDto value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _fileService.Update(value);
                    return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model state");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _fileService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("download/{id:int}")]
        [ResponseType(typeof (FileDto))]
        public HttpResponseMessage DownloadFile(int id)
        {
            try
            {
                var file = _fileService.DownloadFile(id);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(file.File);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = file.OriginalFileName();
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("open/{id:int}")]
        [ResponseType(typeof (FileDto))]
        public HttpResponseMessage OpenInBrowser(int id)
        {
            var file = _fileService.DownloadFile(id);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(file.File);
            if (!Regex.IsMatch(file.FileName, ".*(.pdf)"))
            {
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            }
            else
            {
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
            } 
            result.Content.Headers.ContentDisposition.FileName = file.OriginalFileName();
            return result;
        }
    }
}