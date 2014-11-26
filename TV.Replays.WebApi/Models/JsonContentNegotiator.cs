using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace TV.Replays.WebApi.Models
{
    public class JsonContentNegotiator : IContentNegotiator
    {
        public ContentNegotiationResult Negotiate(Type type, System.Net.Http.HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(new JsonMediaTypeFormatter(), new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}