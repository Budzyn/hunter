using System;
using System.Net;
using Hunter.DataAccess.Entities;

namespace Hunter.Services.Dto.ApiResults
{
    public class ApiResult
    {
        public bool IsError { get; set; }

        public string Message { get; set; }

        public HttpStatusCode Code { get; set; }

        public ApiResult()
        {
        }

        public ApiResult(bool isError = false, string message = null, HttpStatusCode code = HttpStatusCode.OK)
        {
            IsError = isError;
            Message = message;
            Code = code;
        }

        /// <summary>
        ///  to make fluent chains
        /// </summary>
        internal ApiResult And(Action<ApiResult> action)
        {
            if (action != null)
                action(this);
            return this;
        }

        internal virtual void SetNotFound(string message = null)
        {
            IsError = false;
            Message = message ?? "resource with given id doesn't exist";
            Code = HttpStatusCode.NotFound;
        }

        internal virtual void SetError(string message, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            IsError = true;
            Message = message ?? "something wrong happened on server";
            Code = code;
        }
    }

    public class IdApiResult : ApiResult
    {
        public long Id { get; set; }

        public IdApiResult()
        {
        }

        public IdApiResult(long id, bool isError = false, HttpStatusCode code = HttpStatusCode.OK, string message = null)
            : base(isError, message, code)
        {
            Id = id;
        }

        /// <summary>
        ///  to make fluent chains
        /// </summary>
        internal IdApiResult And(Action<IdApiResult> action)
        {
            if (action != null)
                action(this);
            return this;
        }
    }

    public class ResourceApiResult : IdApiResult
    {
        public object Resource { get; set; }

        public ResourceApiResult()
        {
        }

        public ResourceApiResult(long id, bool isError = false, HttpStatusCode code = HttpStatusCode.Created, string message = null)
            : base(id, isError, code, message)
        {
        }

        internal override void SetNotFound(string message = null)
        {
            base.SetNotFound(message);
            Resource = null;
        }

        /// <summary>
        ///  to make fluent chains
        /// </summary>
        internal ResourceApiResult And(Action<ResourceApiResult> action)
        {
            if (action != null)
                action(this);
            return this;
        }
    }

    public class ResourceApiResult<T> : ResourceApiResult where T : class, new()
    {
        public new T Resource
        {
            get
            {
                return base.Resource as T;
            }
            set { base.Resource = value; }
        }

        public ResourceApiResult()
        {
        }

        public ResourceApiResult(long id, bool isError = false, HttpStatusCode code = HttpStatusCode.Created, string message = null) : base(id, isError, code, message)
        {
        }

        /// <summary>
        ///  to make fluent chains
        /// </summary>
        internal ResourceApiResult<T> And(Action<ResourceApiResult<T>> action)
        {
            if (action != null)
                action(this);
            return this;
        }
    }


    public static class Api
    {
        public static ResourceApiResult<T> Details<T>(long id, T model) where T: class, new()
        {
            return new ResourceApiResult<T>(id, code: HttpStatusCode.OK) { Resource = model };
        }

        public static IdApiResult Updated(long id, HttpStatusCode code = HttpStatusCode.OK)
        {
            return new IdApiResult(id, false, code: code);
        }

        public static ResourceApiResult<T> Added<T>(long id, T model) where T : class, new()
        {
            return new ResourceApiResult<T>(id, code: HttpStatusCode.Created) {Resource = model};
        }

        public static ResourceApiResult<T> ResourceNotFound<T>(long id) where T : class, new()
        {
            return new ResourceApiResult<T>(id).And(res=> res.SetNotFound(string.Format("{0} with id was not found", typeof(T).Name)));
        }

        public static IdApiResult NotFound(long id)
        {
            return new IdApiResult(id).And(res => res.SetNotFound());
        }

        public static IdApiResult Deleted(long id)
        {
            return new IdApiResult(id, code: HttpStatusCode.OK);
        }

        public static IdApiResult Error(long id, string message, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            return new IdApiResult(id).And(res=> res.SetError(message, code));
        }

        public static ApiResult Conflict(string conflictMessage)
        {
            return new ApiResult(true, conflictMessage, HttpStatusCode.Conflict);
        }
    }
}
