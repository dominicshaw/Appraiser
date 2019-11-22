using System.Collections.Generic;

namespace Appraiser.Web
{
    public static class GlobalConfiguration
    {
        public static readonly bool UseHttps = false;

        public static readonly Dictionary<bool, string[]> Urls = new Dictionary<bool, string[]>
        {
            { true,  new[] { "http://*:7825", "https://*:7826"} },
            { false, new[] { "http://*:7825" } }
        };
    }
}