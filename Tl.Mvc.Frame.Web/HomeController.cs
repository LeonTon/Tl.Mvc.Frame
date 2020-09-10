using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tl.Mvc.Frame.Web;

namespace Tl.Mvc.Frame
{
    public class HomeController : Web.Controller
    {

        private static readonly string _html = @"<html>
                                                <head>
                                                    <title>Hello</title>
                                                </head>
                                                <body>
                                                    <p>Hello World!</p>
                                                </body>
                                                </html>";

        //[HttpGet("{contrller}/{action}")]
        //[HttpGet("/")]
        //[HttpGet("foobar")]
        //public Task Index() => ActionContext.HttpContext.Response.WriteAsync("hello world");

        [HttpGet("ContentResult")]
        [HttpGet("/")]
        public Task<Web.ContentResult> GetContentResultAsync() =>
            Task.FromResult(new Web.ContentResult(_html, "text/html"));

        [HttpGet("JsonResult")]
        public Task<Web.JsonResult> GetJsonResultAsync() => 
            Task.FromResult(new Web.JsonResult(new { result = "hello" }));

        [HttpGet("StringAsync")]
        public Task<string> GetStringResultAsync() => Task.FromResult(_html);


        [HttpGet("Task")]
        public Task GetTaskAsync() => Task.CompletedTask;


        [HttpGet("String")]
        public string String() =>"hello";


        [HttpGet("foo")]
        public Task<Web.JsonResult> Foo(string foo, int bar, double baz)
        {
            return Task.FromResult(new Web.JsonResult(new { Foo = foo, Bar = bar, Baz = baz }));
        }

        [HttpPost("baz")]
        public Task<Web.JsonResult> Baz([FromForm]RequestParam param)
        {
            return Task.FromResult(new Web.JsonResult(param));
        }


        [HttpPost("bar")]
        public Task<Web.JsonResult> Bar([FromForm]string[] bar)
        {
            return Task.FromResult(new Web.JsonResult(new { bar }));
        }
    }

    public class RequestParam
    {
        public string Foo { get; set; }

        public string Bar { get; set; }

        public string[] Baz { get; set; }
    }
}
