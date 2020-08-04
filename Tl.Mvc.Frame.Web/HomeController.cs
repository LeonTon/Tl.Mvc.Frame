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
        public Task<Web.ContentResult> GetContentResultAsync() => Task.FromResult(new Web.ContentResult(_html, "text/html"));

        [HttpGet("JsonResult")]
        public Task<Web.JsonResult> GetJsonResultAsync() => Task.FromResult(new Web.JsonResult(new { result = "hello" }));

        [HttpGet("stringAsync")]
        public Task<string> GetStringResultAsync() => Task.FromResult(_html);


        [HttpGet("task")]
        public Task GetTaskAsync() => Task.CompletedTask;


        [HttpGet("string")]
        public string String() =>"hello";
    }
}
