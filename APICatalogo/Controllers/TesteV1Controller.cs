﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{v:apiVersion}/teste")]
    [ApiController]
    public class TesteV1Controller : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>TesteV1Controller - V 1.0 </h2></body></html>", "text/html");
        }
    }
}
