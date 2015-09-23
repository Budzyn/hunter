using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hunter.Services;
using Hunter.Services.Interfaces;
using WebGrease.Css.Extensions;

namespace Hunter.Rest.Controllers
{
    [RoutePrefix("api/vacancy")]
    public class VacancyController : ApiController
    {
        private readonly IVacancyService _vacancyService;
        private readonly IUserService _userService;

        public VacancyController(IVacancyService vacancyService, IUserService userService)
        {
            _vacancyService = vacancyService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IList<VacancyRowDto>))]
        public HttpResponseMessage Get([FromUri] int page, int pageSize, string sortColumn, bool reverceSort, string filter, string pool, string status, string addedBy)
        {
            try
            {
                var filterParams = new VacancyFilterParams
                {
                    Page = page,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    ReverceSort = reverceSort,
                    Filter = filter,
                    Pool = pool,
                    Status = status,
                    AddedBy = addedBy
                };
                var vacancies = _vacancyService.Get(filterParams);
                vacancies.Rows.ForEach(x => x.PoolColors = _vacancyService.GetColors(x.Id));
                return Request.CreateResponse(HttpStatusCode.OK, vacancies);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(VacancyDto))]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var vacancy = _vacancyService.Get(id);
                vacancy.PoolColors = _vacancyService.GetColors(id);

                return Request.CreateResponse(HttpStatusCode.OK, vacancy);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("longlist/{id:int}")]
        [ActionName("Longlist")]
        [ResponseType(typeof(VacancyLongListDto))]
        public HttpResponseMessage GetVacancyLongList(int id)
        {
            try
            {
                var vacancy = _vacancyService.GetLongList(id);
                vacancy.PoolColors = _vacancyService.GetColors(id);

                return Request.CreateResponse(HttpStatusCode.OK, vacancy);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }    
        }

        [HttpGet]
        [Route("longlist/{id:int}/addedby")]
        [ActionName("LonglistAddedBy")]
        [ResponseType(typeof(AddedByDto))]
        public HttpResponseMessage GetVacancyLongListAddedBy(int id)
        {
            try
            {
                var addedBy = _vacancyService.GetLongListAddedBy(id);

                return Request.CreateResponse(HttpStatusCode.OK, addedBy);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("state/{id:int}")]
        [ActionName("stage")]
        [ResponseType(typeof(AddedByDto))]
        public HttpResponseMessage GetVacancyByState(int id)
        {
            try
            {
                var vacancy = _vacancyService.GetVacancyByState(id);

                return Request.CreateResponse(HttpStatusCode.OK, vacancy);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post(VacancyDto value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _vacancyService.Add(value);
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
        public HttpResponseMessage Update(int id, VacancyDto value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _vacancyService.Update(value);
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
                _vacancyService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("filterInfo/{roleName}")]
        public HttpResponseMessage GetFilterInfo(string roleName)
        {
            try 
            {
                var filterInfo = _userService.GetFilterInfo(roleName);
                return Request.CreateResponse(HttpStatusCode.OK, filterInfo);
    
            } catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpGet]
        [Route("addedby")]
        [ResponseType(typeof(IEnumerable<AddedByDto>))]
        public HttpResponseMessage GetCandidatesListAddedBy()
        {
            try
            {
                var addedBy = _vacancyService.GetVacanciesAddedBy();

                return Request.CreateResponse(HttpStatusCode.OK, addedBy);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}