using System;
using Hunter.Services;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;

namespace Hunter.Rest
{
    //[Authorize]
    [RoutePrefix("api/pool")]
    public class PoolController : ApiController
    {
        private readonly IPoolService _poolService;

        public PoolController(IPoolService poolService)
        {
            _poolService = poolService;
        }
        
        // GET: api/Pool
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<PoolViewModel>))]
        public HttpResponseMessage Get()
        {
            try
            {
                var pools = _poolService.GetAllPools();
                return Request.CreateResponse(HttpStatusCode.OK, pools);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }   
        }

        // GET: api/Pool/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(PoolViewModel))]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var pool = _poolService.GetPoolById(id);
                return Request.CreateResponse(HttpStatusCode.OK, pool);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // POST: api/Pool
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(PoolViewModel))]
        public HttpResponseMessage Post([FromBody] PoolViewModel poolViewModel)
        {
            try
            {
                if (poolViewModel == null || _poolService.IsPoolNameExist(poolViewModel.Name))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Pool name empty or already exists!");
                }

                poolViewModel = _poolService.CreatePool(poolViewModel);

                return poolViewModel != null ? Request.CreateResponse(HttpStatusCode.OK, poolViewModel) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            
        }

        // PUT: api/Pool/5
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(PoolViewModel))]
        public HttpResponseMessage Put(int id, [FromBody] PoolViewModel poolViewModel)
        {
            try
            {
                if (!_poolService.IsPoolExist(id) || poolViewModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if (_poolService.IsPoolNameExistExceptCurrentPool(poolViewModel))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Cann't update. Another pool with same name alreafy exists!");
                }

                poolViewModel.Id = id;
                _poolService.UpdatePool(poolViewModel);

                return Request.CreateResponse(HttpStatusCode.OK, poolViewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // DELETE: api/Pool/5
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(int))]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (!_poolService.IsPoolExist(id))
                {
                    Request.CreateResponse(HttpStatusCode.NotFound, id);
                }

                _poolService.DeletePool(id);

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }   
        }
    }
}
