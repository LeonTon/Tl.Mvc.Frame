using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class ContentResult : IActionResult
    {
        public readonly string _content;
        public readonly string _contentType;

        public ContentResult(string content, string contentType)
        {
            _content = content;
            _contentType = contentType;
        }

        public Task ExcuteResultAsync(ActionContext actionContext)
        {
            var response = actionContext.HttpContext.Response;
            response.ContentType = _contentType;
            return response.WriteAsync(_content);
        }
    }
}
