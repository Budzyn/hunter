using System;

namespace Hunter.Common.Interfaces
{
    public interface ILogger
    {
        void Log(Exception ex);
        void Log(string message);
    }
}
