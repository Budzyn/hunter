using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Hunter.Services;
using Hunter.Services.Interfaces;

namespace Hunter.Rest.Controllers
{
    [RoutePrefix("api/fileupload")]
    public class FileUploadController : ApiController
    {
        private IFileService _fileService;
        private ICandidateService _candidateService;

        public FileUploadController(IFileService fileService, ICandidateService candidateService)
        {
            _fileService = fileService;
            _candidateService = candidateService;
        }

        [HttpGet]
        [Route("pictures/{id:int}")]
        public IHttpActionResult GetPicture(int id)
        {   
            try
            {
                var photo = _fileService.GetPhoto(id);
                if (photo == null)
                {
                    return Redirect(Url.Content("~/Content/img/no_photo.png"));
                }
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(photo);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return ResponseMessage(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }


        [HttpPost]
        [Route("pictures/{id:int}")]
        public async Task<HttpResponseMessage> PostPicture(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                byte[] ms = await provider.Contents[0].ReadAsByteArrayAsync();
                var candidate = _candidateService.Get(id);

                _fileService.SavePhoto(candidate, ms);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("pictures/fromUrl/{id:int}")]
        public HttpResponseMessage PostPictureFromUrl(int id, [FromBody]string photoUrl)
        {
            try
            {
                _fileService.UploadPhotoFromUrl(photoUrl, id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            
        }

        [HttpDelete]
        [Route("pictures/{id:int}")]
        public HttpResponseMessage DeletePicture(int id)
        {
            var candidate = _candidateService.Get(id);
            if (candidate == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            try
            {
                candidate.Photo = null;
                _candidateService.Update(candidate.ToCandidateDto());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Route("")]
        public int Post(FileDto data)
        {
            return _fileService.Add(data);
        }
    }
}