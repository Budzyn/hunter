using System;
using Hunter.DataAccess.Entities;

namespace Hunter.Services.Interfaces
{
    public interface IActivityPostService
    {
        void Post(string message, ActivityType tag, Uri url = null);
    }
}