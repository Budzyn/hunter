using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hunter.Services;
using Hunter.Services.Dto.ScheduledNotification;
using Hunter.Services.Interfaces;
using System.Linq;

namespace Hunter.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/notifications")]
    public class ScheduledNotificationController : ApiController
    {
        private readonly IScheduledNotificationService _scheduledNotificationService;

        public ScheduledNotificationController(IScheduledNotificationService scheduledNotificationService)
        {
            _scheduledNotificationService = scheduledNotificationService;
        }

        //[HttpGet]
        //[Route("")]
        //[ResponseType(typeof(IEnumerable<ScheduledNotificationDto>))]
        //public HttpResponseMessage Get()
        //{
        //    try
        //    {
        //        var login = RequestContext.Principal.Identity.Name;
        //        var notifications = _scheduledNotificationService.Get(login);
        //        return Request.CreateResponse(HttpStatusCode.OK, notifications);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //}

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(PageDto<ScheduledNotificationDto>))]
        public HttpResponseMessage Get(int page, int pageSize, string orderField, bool invertOrder, string notificationTypes)
        {
            try
            {
                var login = RequestContext.Principal.Identity.Name;
                var nTypes = notificationTypes.Trim('"');
                var filter = new ScheduledNotificationFilterDto
                {
                    Page = page,
                    PageSize = pageSize,
                    OrderField = orderField,
                    InvertOrder = invertOrder,
                    NotificationTypes = !string.IsNullOrWhiteSpace(nTypes) ?
                                        nTypes.Split(',').Select(n => int.Parse(n)).ToArray() : 
                                        new int[0]
                };
                var notifications = _scheduledNotificationService.Get(login, filter);
                return Request.CreateResponse(HttpStatusCode.OK, notifications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("active")]
        [ResponseType(typeof(IEnumerable<ScheduledNotificationDto>))]
        public HttpResponseMessage GetActive()
        {
            try
            {
                var login = RequestContext.Principal.Identity.Name;
                var notifications = _scheduledNotificationService.GetActive(login);
                return Request.CreateResponse(HttpStatusCode.OK, notifications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(ScheduledNotificationDto))]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var notification = _scheduledNotificationService.Get(id);
                return Request.CreateResponse(HttpStatusCode.OK, notification);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ScheduledNotificationDto value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var login = RequestContext.Principal.Identity.Name;
                    value.UserLogin = login;
                    _scheduledNotificationService.Add(value);
                    return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model state");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public HttpResponseMessage Put(int id, [FromBody]ScheduledNotificationDto value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _scheduledNotificationService.Update(value);
                    return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model state");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/shown")]
        public HttpResponseMessage NotificationShown(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _scheduledNotificationService.NotificationShown(id);
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
                _scheduledNotificationService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("candidate/{id:int}")]
        [ResponseType(typeof(IEnumerable<ScheduledNotificationDto>))]
        public HttpResponseMessage GetCandidateNotifications(int id)
        {
            try
            {
                var login = RequestContext.Principal.Identity.Name;
                var notifications = _scheduledNotificationService.GetCandidateNotifications(login, id);
                return Request.CreateResponse(HttpStatusCode.OK, notifications);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("notify")]
        public HttpResponseMessage Notify()
        {
            try
            {
                _scheduledNotificationService.Notify();
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}