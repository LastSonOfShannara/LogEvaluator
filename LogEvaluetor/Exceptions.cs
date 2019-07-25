using System;

namespace LogEvaluetor
{
    public class ReferenceLineException : Exception
    {
        public ReferenceLineException(string message) : base(message)
        {

        }
    }

    public class DeviceNotInicializedException : Exception
    {
        public DeviceNotInicializedException(string message) : base(message)
        {

        }
    }

    public class UnknownDeviceException : Exception
    {
        public UnknownDeviceException(string message) : base(message)
        {

        }
    }
}
