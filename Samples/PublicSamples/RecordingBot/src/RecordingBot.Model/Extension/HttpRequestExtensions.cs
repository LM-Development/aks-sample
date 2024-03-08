using Microsoft.AspNetCore.Http;
using System;

namespace RecordingBot.Model.Extension
{
    //
    // Summary:
    //     Set of extension methods for Microsoft.AspNetCore.Http.HttpRequest.
    public static class HttpRequestExtensions
    {
        private const string UnknownHostName = "UNKNOWN-HOST";

        private const string MultipleHostName = "MULTIPLE-HOST";

        private const string Comma = ",";

        public static Uri GetUri(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Scheme))
            {
                throw new ArgumentException("Http request Scheme is not specified");
            }

            string hostValue = request.Host.HasValue ? request.Host.Value : UnknownHostName;
            if (hostValue.IndexOf(Comma, StringComparison.Ordinal) > 0)
            {
                hostValue = MultipleHostName;
            }

            string pathBaseValue = request.PathBase.HasValue ? request.PathBase.Value : string.Empty;
            string pathValue = request.Path.HasValue ? request.Path.Value : string.Empty;
            string queryStringValue = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;

            string uriString = $"{request.Scheme}://{hostValue}{pathBaseValue}{pathValue}{queryStringValue}";
            return new Uri(uriString);
        }
    }
}
