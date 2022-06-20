using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;

namespace WebBanHang_API.Areas.Sales.Controllers
{
    [Route("api/Home")]

    public class HomeSalesController : ApiController
    {
        Shop db = new Shop();

    }
}
