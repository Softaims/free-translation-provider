using System;
using System.Collections.Generic;
using System.Net;

namespace FreeTranslateProvider
{
    public class FreeTranslateOptions
    {
        public string ApiKey { get; set; }
        public int Timeout { get; set; } = 30000; // 30 seconds default timeout
        public string ServiceUrl { get; set; } = "https://api.openai.com"; // OpenAI service URL

        public FreeTranslateOptions() { }

        public FreeTranslateOptions(Uri uri)
        {
            if (uri?.Query != null)
            {
                var queryParameters = ParseQueryString(uri.Query); // Custom method to parse the query string
                ApiKey = queryParameters.ContainsKey("apikey") ? queryParameters["apikey"] : null;
                if (queryParameters.ContainsKey("timeout") && int.TryParse(queryParameters["timeout"], out int timeout))
                {
                    Timeout = timeout;
                }

                ServiceUrl = queryParameters.ContainsKey("serviceurl") ? queryParameters["serviceurl"] : ServiceUrl;
            }
        }

        public Uri ToUri()
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = "openai",  // OpenAI URI scheme
                Host = "localhost"
            };

            var queryString = BuildQueryString(new Dictionary<string, string>
            {
                { "apikey", ApiKey },
                { "timeout", Timeout.ToString() },
                { "serviceurl", ServiceUrl }
            });

            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        // Method to manually parse query string into key-value pairs
        private Dictionary<string, string> ParseQueryString(string query)
        {
            var parameters = new Dictionary<string, string>();
            var queryString = query.StartsWith("?") ? query.Substring(1) : query;
            var pairs = queryString.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    parameters[WebUtility.UrlDecode(keyValue[0])] = WebUtility.UrlDecode(keyValue[1]);
                }
            }
            return parameters;
        }

        // Method to build query string from a dictionary
        private string BuildQueryString(Dictionary<string, string> parameters)
        {
            var queryParts = new List<string>();
            foreach (var param in parameters)
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    queryParts.Add($"{WebUtility.UrlEncode(param.Key)}={WebUtility.UrlEncode(param.Value)}");
                }
            }
            return string.Join("&", queryParts);
        }
    }
}
