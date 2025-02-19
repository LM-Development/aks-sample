using System;

namespace RecordingBot.Services.ServiceSetup
{
    [Serializable]
    public class CertNotFoundException : Exception
    {
        public CertNotFoundException()
        {
        }

        public CertNotFoundException(string message) : base(message)
        {
        }

        public CertNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public string Thumbprint { get; internal set; }
    }
}
