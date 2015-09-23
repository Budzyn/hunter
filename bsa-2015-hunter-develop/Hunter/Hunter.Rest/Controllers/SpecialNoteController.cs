using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hunter.Services.Dto;
using Hunter.Services.Services.Interfaces;

namespace Hunter.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/specialnote")]
    public class SpecialNoteController : ApiController
    {
        private readonly ISpecialNoteService _specialNoteService;

        public SpecialNoteController(ISpecialNoteService specialNoteService)
        {
            _specialNoteService = specialNoteService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetSpecialNote()
        {
            try
            {
                var specialNotes = _specialNoteService.GetAllSpecialNotes();
                return Request.CreateResponse(HttpStatusCode.OK, specialNotes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("candidate/{cid}")]
        public HttpResponseMessage GetSpecialNoteForCandidate(int cid)
        {
            try
            {
                var specialNotes = _specialNoteService.GetSpecialNotesForCandidate(cid);
                return Request.CreateResponse(HttpStatusCode.OK, specialNotes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("{vid}/{cid}")]
        public HttpResponseMessage GetSpecialNoteForCard(int vid,int cid)
        {
            try
            {
                var specialNotes = _specialNoteService.GetSpecialNotesForCard(vid,cid);
                return Request.CreateResponse(HttpStatusCode.OK, specialNotes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{cid}")]
        public HttpResponseMessage GetSpecialNoteForUser(int cid)
        {
            try
            {
                var specialNotes = _specialNoteService.GetSpecialNotesForUser(User.Identity.Name, cid);
                return Request.CreateResponse(HttpStatusCode.OK, specialNotes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetSpecialNoteById(int id)
        {
            try
            {
                var specialNotes = _specialNoteService.GetSpecialNoteById(id);
                return Request.CreateResponse(HttpStatusCode.OK, specialNotes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage PostSpecialNote([FromBody] SpecialNoteDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var login = RequestContext.Principal.Identity.Name;
                    model.UserLogin = login;
                    model.LastEdited = DateTime.UtcNow;
                    var result = _specialNoteService.AddSpecialNote(model);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model state");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage UpdateSpecialNote(int id,[FromBody] SpecialNoteDto model)
        {
            try
            {
                if (ModelState.IsValid && id == model.Id)
                {
                    model.LastEdited = DateTime.Now;
                    model.UserLogin = User.Identity.Name;
                    var result = _specialNoteService.UpdateSpecialNote(model);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model state");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage DeleteSpecialNote(int id)
        {
            try
            {
                _specialNoteService.DeleteSpecialNoteById(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
