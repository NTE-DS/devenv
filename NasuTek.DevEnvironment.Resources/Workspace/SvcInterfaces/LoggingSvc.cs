using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Svcs
{
    public class LoggingSvc : IDevEnvLoggingSvc
    {
        public void Error(Exception exception)
        {
            //throw new NotImplementedException();
        }

        public void Error(string message)
        {
            //throw new NotImplementedException();
        }

        public void Info(string message)
        {
            //throw new NotImplementedException();
        }

        public void ShowDialog(string message, MessageType type)
        {
            //throw new NotImplementedException();
        }

        public void Warning(string message)
        {
            //throw new NotImplementedException();
        }
    }
}
