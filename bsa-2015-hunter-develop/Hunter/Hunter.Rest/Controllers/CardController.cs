using Hunter.Services;
using Hunter.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hunter.Services.Interfaces;
using System.Web.Http.Description;

namespace Hunter.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/card")]
    public class CardController : ApiController
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        // GET: api/Card
        [HttpGet]
        [Route("")]
        public IEnumerable<CardDto> Get()
        {
            return new List<CardDto>() { new CardDto() { Id = 1 }, new CardDto(){Id = 2} };
        }

        // GET: api/Card/5
        [HttpGet]
        [Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            try 
            {
                var card = _cardService.GetCard(id);
                return Request.CreateResponse(HttpStatusCode.OK, card);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        // GET: api/Card/5
        [HttpGet]
        [Route("appResults/{cid:int}")]
        [ResponseType(typeof(AppResultCardDto))]
        public HttpResponseMessage ApplicationResults(int cid)
        {
            try
            {
                var results = _cardService.GetApplicationResults(cid);
                if (results == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Candidate has no other vacancy!");
                }
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // GET
        [HttpGet]
        [Route("isUsed/{vid:int}/{cid:int}")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage IsCardUsed(int vid, int cid)
        {
            try
            {
                var result = _cardService.IsCardUsed(vid, cid);
                if (result == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        
        // POST: api/Card
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody] IEnumerable<CardDto> cards)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _cardService.AddCards(cards, User.Identity.Name);
                    return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model state");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // PUT: api/Card/5
        [HttpPut]
        [Route("stage")]
        public HttpResponseMessage GetStage([FromUri]int vid, [FromUri]int cid, [FromBody]int newStage)
        {
            try
            {
                if (_cardService.UpdateCardStage(vid, cid, newStage))
                    return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid update stage");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("stage")]
        public HttpResponseMessage UpdateStage([FromUri]int vid, [FromUri]int cid)
        {
            try
            {
                var stage = _cardService.GetCardStage(vid, cid);
                return Request.CreateResponse(HttpStatusCode.OK, stage);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // DELETE: api/Card/5/5
        [HttpDelete]
        [Route("{vid:int}/{cid:int}")]
        public HttpResponseMessage Delete(int vid, int cid)
        {
            try
            {
                if (!_cardService.IsCardExist(vid, cid))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        string.Format("No card for vacancy with id={0} and candidate with id={1}!", vid, cid));
                }
                _cardService.DeleteCard(vid, cid);

                return Request.CreateResponse(HttpStatusCode.OK, string.Format("Card for vacancy with id={0} and candidate with id={1} was deleted!", vid, cid));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // DELETE: api/Card/5/5
        [HttpDelete]
        [Route("deleteallinfo/{vid:int}/{cid:int}")]
        public HttpResponseMessage DeleteAllInfo(int vid, int cid)
        {
            try
            {
                if (!_cardService.IsCardExist(vid, cid))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        string.Format("No card for vacancy with id={0} and candidate with id={1}!", vid, cid));
                }
                _cardService.DeleteAllInfo(vid, cid);

                return Request.CreateResponse(HttpStatusCode.OK, string.Format("Card and all interviews and notes for vacancy with id={0} and candidate with id={1} were deleted!", vid, cid));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("info/{vid:int}/{cid:int}")]
        [ResponseType(typeof(AppResultCardDto))]
        public IHttpActionResult GetCardInfo(int vid, int cid)
        {
            try
            {
                var results = _cardService.GetCardInfo(vid, cid);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
