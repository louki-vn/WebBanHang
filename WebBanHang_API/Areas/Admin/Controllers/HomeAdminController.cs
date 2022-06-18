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
        [Route("api/admin/get_count_HomeAdmin")]
        public IHttpActionResult getCountHomeAdmin()
        {
            var mem = db.MEMBERs.ToList();
            var order = db.Database.SqlQuery<TRANSACTION>("SelectFinishTransaction").ToList();
            var topproduct = db.PRODUCTs.SqlQuery("exec SelectTopProduct").ToList();
            var category = db.CATEGORies.ToArray();
            var count_mem = mem.Count();
            var count_order = order.Count();
            var item = db.ITEM_SOLD.ToList();
            int amount = 0, total = 0;

            foreach (var o in order)
            {
                total += o.amount;
            }

            foreach (var o in item)
            {
                amount += (int)o.qty;

            }
            Dictionary<int, string> category_count = new Dictionary<int, string>();
            foreach (var c in category)
            {
                category_count.Add(c.category_id, c.name);
            }
            var brand = db.BRANDs.ToList();
            Dictionary<int, string> brand_count = new Dictionary<int, string>();
            foreach (var i in brand)
            {
                brand_count.Add(i.brand_id, i.brand_name);
            }
            var topmem = db.Database.SqlQuery<Mem_Cart>("exec SelectTopMember").ToList();
            return Ok(new { count_mem, count_order, amount, total });
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpGet]
        [Route("api/admin/get_topProduct_HomeAdmin")]
        public IHttpActionResult getTopProductHomeAdmin()
        {
            var topproduct = db.PRODUCTs.SqlQuery("exec SelectTopProduct").ToList();
           
            return Ok(topproduct);
        }
        [HttpGet]
        [Route("api/admin/get_topMember_HomeAdmin")]
        public IHttpActionResult getTopMemBerHomeAdmin()
        {
           
            var topmem = db.Database.SqlQuery<Mem_Cart>("exec SelectTopMember").ToList();
            return Ok(topmem);
        }
    }
}
