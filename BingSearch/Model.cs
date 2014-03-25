using System.Collections.Generic;

namespace BingSearch
{
    public class BingSearch
    {
        public List<Result> Results { get; set; }

        public class Result
        {
            public Metadata __Metadata { get; set; }
            public string ID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string DisplayUrl { get; set; }
            public string Url { get; set; }
        }

        public class Metadata
        {
            public string Uri { get; set; }
            public string Type { get; set; }
        }
    }
}
