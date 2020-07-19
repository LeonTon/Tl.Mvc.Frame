
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tl.Mvc.Frame.Web;

namespace Tl.Mvc.Frame
{
    public class HomeController: Web.Controller
    {
        [HttpGet("{contrller}/{action}")]
        [HttpGet("/")]
        [HttpGet("foobar")]
        public Task Index() => ActionContext.HttpContext.Response.WriteAsync("hello world");
    }
}
