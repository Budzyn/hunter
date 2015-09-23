using System;
using Hunter.Common.Interfaces;
using log4net;

namespace Hunter.Common.Concrete
{
    public class Logger : ILogger
    {
        private static readonly Lazy<ILogger> _instance = new Lazy<ILogger>(() => new Logger());

        public static ILogger Instance { get { return _instance.Value; } }

        private readonly ILog _log = LogManager.GetLogger("Hunter");

        public void Log(Exception ex)
        {
            _log.Error(ex.Message, ex);
        }

        public void Log(string message)
        {
            _log.Error(message);
        }
    }
}
