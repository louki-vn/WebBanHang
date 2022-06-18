using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;

namespace WebBanHang_API.Areas.Admin.Controllers
{
    public class HomeAdminController : ApiController
    {
        Shop db = new Shop();
        public HomeAdminController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        [HttpGet]
        [Route("api/admin/get_count_member")]
        public IHttpActionResult getCountMember()
        {
            var mem = db.MEMBERs.ToList().Count();
            var order = db.Database.SqlQuery<TRANSACTION>("SelectFinishTransaction").ToList().Count();
            return Ok(new { data=mem, order });
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }
    }
}
