using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using Newtonsoft.Json;
using RestSharp;

namespace BingSearch
{
    public class Searcher
    {
        private readonly RestClient _client;

        public Searcher(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("apiKey");

            _client = new RestClient();
            _client.Authenticator = new HttpBasicAuthenticator(apiKey, apiKey);
            _client.AddHandler("application/json", new DynamicJsonDeserializer());
        }

        public IEnumerable<BingSearch.Result> Search(string query, int resultsCount)
        {
            var request = BuildBingRequest(query, resultsCount);
            var response = _client.Execute<dynamic>(request);
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new AuthenticationException("Api Key is not correct");

                throw new Exception(string.Format("Request Error Message: {0}. Content: {1}.", response.ErrorMessage, response.Content));
            }

            return response.Data == null ? null : JsonConvert.DeserializeObject<BingSearch>(response.Data.d.ToString()).Results;
        }

        public RestRequest BuildBingRequest(string query, int resultsCount)
        {
            var request = new RestRequest
            {
                Resource = "https://api.datamarket.azure.com/Bing/SearchWeb/v1/Web",
                Method = Method.GET,
                RequestFormat = DataFormat.Json
            };

            if (resultsCount > 0)
            {
                request.AddParameter(new Parameter
                {
                    Name = "$top",
                    Value = resultsCount,
                    Type = ParameterType.GetOrPost
                });
            }

            request.AddParameter(new Parameter
            {
                Name = "Query",
                Value = string.Format("'{0}'", query.Trim()),
                Type = ParameterType.GetOrPost
            });

            return request;
        }
    }
}
