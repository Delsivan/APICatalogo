using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    //[ApiVersion("2.0")]
    //[Route("api/teste")]
    //[ApiController]
    public class TesteV2Controller : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>TesteV2Controller - V 2.0 </h2></body></html>", "text/html");
        }
    }
}
