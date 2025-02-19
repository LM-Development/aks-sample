using System;

namespace RecordingBot.Services.ServiceSetup
{
    [Serializable]
    internal class CertNotFoundException : Exception
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