using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{


    public class JsonResult : IActionResult
    {

        public readonly object _content;

        public JsonResult(object content)
        {
            _content = content;
        }

        public Task ExcuteResultAsync(ActionContext actionContext)
        {
            var response = actionContext.HttpContext.Response;
            response.ContentType = "application/json";
            return response.WriteAsync(JsonSerializer.Serialize(_content));
        }
    }
}
