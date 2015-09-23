using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web.Http;
using Hunter.Common.Concrete;
using Hunter.Services.Dto.ApiResults;
using Hunter.Services.Dto.User;
using Hunter.Services.Interfaces;
using System.Net.Http;
using System.Web.Http.Description;

namespace Hunter.Rest.Controllers
{
    [RoutePrefix("api/userprofile")]
    public class UserProfileController : ApiController
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        [Route("users/role/{roleName}")]
        [ResponseType(typeof(IEnumerable<UserProfileRowVm>))]
        public HttpResponseMessage GetAllTech(string roleName)
        {
            try 
            {
                var users = _profileService.GetUserProfile(roleName);
                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        // GET: api/userprofile
        // GET: api/userprofile?page=1
        [HttpGet]
        [Route("")]
        public IEnumerable<UserProfileRowVm> GetPage([FromUri(Name = "page")]int page = 1)
        {
            return _profileService.LoadPage(page);
        }

        // GET: api/userprofile/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            return FromApiResult(() => _profileService.GetById(id));
        }

        // GET: api/userprofile/page/1
        [HttpGet]
        [Route("page/{id:int}")]
        public IEnumerable<UserProfileRowVm> GetPageV2(int id)
        {
            return _profileService.LoadPage(id);
        }

        // POST: api/Pool - add new profile
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] EditUserProfileVm profileVm)
        {
            return FromApiResult(() => _profileService.Save(profileVm));
        }

        // DELETE: api/Pool/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            return FromApiResult(() => _profileService.Delete(id));
        }

        private IHttpActionResult FromApiResult(Func<ApiResult> apiResultFunc)
        {
            if (apiResultFunc == null)
            {
                Logger.Instance.Log("function not passed to FromApiResult");
                return InternalServerError();
            }

            ApiResult result = null;

            try
            {
                result = apiResultFunc();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            var resourceResult = result as ResourceApiResult;

            if (resourceResult != null && !resourceResult.IsError)
            {
                if (resourceResult.Code == HttpStatusCode.Created)
                {
                    return Created(Request.RequestUri + resourceResult.Id.ToString(CultureInfo.InvariantCulture),
                                   resourceResult.Resource);
                }
                if (resourceResult.Code == HttpStatusCode.OK)
                {
                    return Ok(resourceResult.Resource);
                }

                return StatusCode(resourceResult.Code);
            }

            if (result.IsError)
            {
                return Content(result.Code, result.Message);
            }

            return StatusCode(result.Code);
        }
    }
}
