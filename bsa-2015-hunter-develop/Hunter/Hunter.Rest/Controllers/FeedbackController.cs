using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hunter.Services.Dto;
using System.Web.Http.Description;
using Hunter.DataAccess.Entities.Entites.Enums;
using Hunter.Services;
using Hunter.Services.Dto.Feedback;
using Hunter.Services.Services.Interfaces;

namespace Hunter.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ITestService _testService;

        public FeedbackController(IFeedbackService feedbackService, ITestService testService)
        {
            _feedbackService = feedbackService;
            _testService = testService;
        }


        [HttpGet]
        [Route("hr/{vid:int}/{cid:int}")]
        //[ActionName("HrInterview")]
        [ResponseType(typeof(IEnumerable<FeedbackDto>))]
        public HttpResponseMessage GetHrInterview(int vid, int cid)
        {
            try
            {
                var interviews = _feedbackService.GetAllHrInterviews(vid, cid,User.Identity.Name);
                return interviews == null ? Request.CreateResponse(HttpStatusCode.BadRequest, "Vacancy has no candidates!") 
                    : Request.CreateResponse(HttpStatusCode.OK, interviews);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("tech/{vacancyId:int}/{candidateId:int}")]
        [ResponseType(typeof(IEnumerable<FeedbackDto>))]
        public HttpResponseMessage GetTechInterview(int vacancyId, int candidateId)
        {
            try
            {
                var interview = _feedbackService.GetTechInterview(vacancyId, candidateId,User.Identity.Name);
                if (interview == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent, "not technical interview");
                return Request.CreateResponse(HttpStatusCode.OK, interview);
            }
            catch(Exception e) 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            
        }

        [HttpGet]
        [Route("summary/{vacancyId:int}/{candidateId:int}")]
        [ResponseType(typeof(FeedbackDto))]
        public HttpResponseMessage GetSummary(int vacancyId, int candidateId)
        {
            try
            {
                var summary = _feedbackService.GetSummary(vacancyId, candidateId);
                if (summary == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent, "not summary");
                return Request.CreateResponse(HttpStatusCode.OK, summary);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

        }

        // POST: api/Feedback
        [HttpPost]
        [Route("save")]
        public HttpResponseMessage Post([FromBody]FeedbackDto FeedbackDto)
        {
            try
            {
                var result = _feedbackService.SaveFeedback(FeedbackDto, User.Identity.Name);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("test/update")]
        public IHttpActionResult UpdateTestFeedback(TestFeedbackHrInterviewDto testFeedback)
        {
            try
            {
                var result =_feedbackService.SaveFeedback(testFeedback.Feedback, User.Identity.Name);
                _testService.UpdateFeedback(testFeedback.TestId, result.Id);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{feedbackId:int}/success/update")]
        public IHttpActionResult UpdateSuccessStatus(int feedbackId, [FromBody] int status)
        {
            try
            {
                SuccessStatus stat = (SuccessStatus) status;
                _feedbackService.UpdateSuccessStatus(feedbackId, stat, User.Identity.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("overview/{vacancyId:int}/{candidateId:int}")]
        [ResponseType(typeof(IEnumerable<FeedbackDto>))]
        public HttpResponseMessage GetAll(int vacancyId, int candidateId)
        {
            try
            {
                var feedbacks = _feedbackService.GetLastFeedbacks(vacancyId, candidateId);
                if (feedbacks == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent, "not feedbacks");
                return Request.CreateResponse(HttpStatusCode.OK, feedbacks);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

        }

        [HttpGet]
        [Route("history/{vacancyId:int}/{candidateId:int}")]
        [ResponseType(typeof(IEnumerable<FeedbackDto>))]
        public HttpResponseMessage GetHistory(int vacancyId, int candidateId)
        {
            try
            {
                var feedbacks = _feedbackService.GetFeedbacksHistory(vacancyId, candidateId);
                if (feedbacks == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent, "not feedbacks");
                return Request.CreateResponse(HttpStatusCode.OK, feedbacks);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

    }
}
