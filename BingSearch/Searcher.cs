using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace BingSearch
{
    public class Searcher
    {
        private readonly RestClient _client;

        public Searcher(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new NullReferenceException("Bing Api Key is missing!");

            _client = new RestClient();
            _client.Authenticator = new HttpBasicAuthenticator(apiKey, apiKey);
            _client.AddHandler("application/json", new DynamicJsonDeserializer());
        }

        public IEnumerable<BingSearch.Result> Search(string query)
        {
            var request = new RestRequest(SearchUrl(query))
            {
                Method = Method.GET,
                RequestFormat = DataFormat.Json
            };

            var response = _client.Execute<dynamic>(request);
            return JsonConvert.DeserializeObject<BingSearch>(response.Data.d.ToString()).Results;
        }

        private string SearchUrl(string query)
        {
            return string.Format("https://api.datamarket.azure.com/Bing/SearchWeb/v1/Web?Query=%27%22{0}%22%27", query.UrlEncode());
        }
    }
}
