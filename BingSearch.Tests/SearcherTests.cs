using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Xunit;
using Xunit.Extensions;

namespace BingSearch.Tests
{
    public class SearcherTests
    {
        private const string ApiKey = "YOUR VALID API KEY NEEDS TO LIVE HERE... You could move it to a config too i guess.";
        private readonly Searcher _searcher;

        public SearcherTests()
        {
            _searcher = new Searcher(ApiKey);
        }

        [Fact]
        public void SearcherConstructorDealsCorrectlyWithApiKey()
        {
            Assert.Throws<ArgumentNullException>(() => new Searcher(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new Searcher(null));

            var validSearcher = new Searcher(ApiKey);
            Assert.NotNull(validSearcher);
        }

        [Fact]
        public void BingRequestHasCorrectResourceUrl()
        {
            var searcher = new Searcher(ApiKey);

            var request = searcher.BuildBingRequest("Just a Test Query", 0);
            Assert.Same(request.Resource, "https://api.datamarket.azure.com/Bing/SearchWeb/v1/Web");
        }

        [Theory]
        [InlineData("Test Query - 0", -5)]
        [InlineData("Test Query - 0", -1)]
        [InlineData("Test Query - 0", 0)]
        [InlineData("Test Query - 1", 1)]
        [InlineData("Test Query - 5", 5)]
        public void BingRequestsHasCorrectParametersBasedOnCountInput(string query, int resultsCount)
        {
            var request = _searcher.BuildBingRequest(query, resultsCount);

            var queryParameter = request.Parameters.FirstOrDefault(parameter => parameter.Name == "Query");
            Assert.NotNull(queryParameter);

            var resultCountParamater = request.Parameters.FirstOrDefault(parameter => parameter.Name == "$top");
            if (resultsCount < 1)
            {
                Assert.Null(resultCountParamater);
            }
            else
            {
                Assert.NotNull(resultCountParamater);
            }
        }

        [Fact]
        public void BasicSearchReturnsAtLeastOneResult()
        {
            var results = _searcher.Search("Lucas Moffitt", 1).ToList();

            Assert.NotNull(results);
            Assert.IsType(typeof(List<BingSearch.Result>), results);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void InvalidApiKeyThrowsUnAuthorizedOnRequest()
        {
            var searcher = new Searcher("INVALID API KEY LOL!");
            Assert.Throws<AuthenticationException>(() => searcher.Search("Lucas Moffitt", 1));
        }
    }
}
