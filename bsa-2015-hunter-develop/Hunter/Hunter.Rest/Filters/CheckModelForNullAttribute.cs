using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Hunter.Rest.Filters
{
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.ContainsValue(null))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    String.Format("The arguments cann't be null {0}",
                        string.Join(",", actionContext.ActionArguments.Where(i => i.Value == null).Select(i => i.Key))));
            }
        }
    }
}