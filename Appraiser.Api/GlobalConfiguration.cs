using System.Collections.Generic;

namespace Appraiser.Api
{
    public static class GlobalConfiguration
    {
        public static readonly bool UseHttps = false;

        public static readonly Dictionary<bool, string[]> Urls = new Dictionary<bool, string[]>
        {
            { true,  new[] { "http://*:7823", "https://*:7824"} },
            { false, new[] { "http://*:7823" } }
        };
    }
}