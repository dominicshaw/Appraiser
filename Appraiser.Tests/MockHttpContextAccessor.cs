using Microsoft.AspNetCore.Http;

namespace Appraiser.Tests
{
    internal class MockHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }
    }
}