using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hunter.Services.Dto;
using Hunter.Services.Services.Interfaces;

namespace Hunter.Rest.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost]
        [Route("change/{id:int}")]
        public HttpResponseMessage ChangeCheckedTest(int id)
        {
            try 
            {
                _testService.ChangeCheckedTest(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            
        }
        
        [HttpGet]
        [Route("notChecked")]
        public HttpResponseMessage GetTestByUser() 
        {
            try 
            {
                var tests = _testService.GetTestByUser(User.Identity.Name);
                return Request.CreateResponse(HttpStatusCode.OK, tests);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpGet]
        [Route("count")]
        public HttpResponseMessage GetCountNotCheck() 
        {
            try 
            {
                int count = _testService.GetCountNoChecked(User.Identity.Name);
                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetTest(int vacancyId, int candidateId)
        {
            try
            {
                var tests = _testService.GetCardTests(vacancyId, candidateId);
                return Ok(tests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("all/{candidateId}")]
        public IHttpActionResult GetAllTests(int candidateId)
        {
            try
            {
                var tests = _testService.GetAllCandidatesTests(candidateId);
                return Ok(tests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddTest(TestDto test)
        {
            try
            {
                int id = _testService.AddTest(test);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public IHttpActionResult UpdateTest(TestDto test)
        {
            try
            {
                _testService.UpdateTest(test);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("addChecking/{testId:int}/{userId:int}")]
        public IHttpActionResult AddCheckingToTest(int testId, int userId)
        {
            try 
            {
                _testService.AddCheckingToTest(testId, userId);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpDelete]
        public IHttpActionResult DeleteTest([FromBody]int testId)
        {
            try
            {
                _testService.DeleteTestById(testId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("comment")]
        public HttpResponseMessage UpdateComment([FromBody]TestCommentDto testComment)
        {
            try
            {
                _testService.UpdateComment(testComment.Id, testComment.Comment);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
