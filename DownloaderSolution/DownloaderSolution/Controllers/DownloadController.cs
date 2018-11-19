using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DownloaderSolution.Controllers
{
    [Produces("application/json")]
    [Route("api/Download")]
    public class DownloadController : Controller
    {
    }
}