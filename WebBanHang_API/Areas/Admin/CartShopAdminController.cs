using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;

namespace WebBanHang_API.Areas.Admin
{
    public class CartShopAdminController : ApiController
    {
        Shop db = new Shop();
        public CartShopAdminController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        [HttpGet]
        [Route("api/admin/GetProduct_CartShop")]
        public IHttpActionResult GetProduct_CartShop(string id)
        {
            int _id = int.Parse(id);
            var product_id = new SqlParameter("@product_id", _id);
            var dt = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_product_id @product_id", product_id).FirstOrDefault();
            return Ok(dt);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        [Route("api/admin/insertTransaction_CartShop")]
        public IHttpActionResult insertTransaction_CartShop([FromBody] TRANSACTION fc)
        {
            TRANSACTION info = new TRANSACTION();
            info.member_name =fc.member_name;
            info.member_id = fc.member_id;
            info.member_phone_number = fc.member_phone_number;
            info.status = 1;
            info.payment = true;
            info.amount = 0;
            db.TRANSACTIONs.Add(info);
            db.SaveChanges();
            return Ok(info);
        }

        [HttpPost]
        [Route("api/admin/SaveInfoPayment_CartShop")]
        public IHttpActionResult SaveInfoPayment_CartShop([FromBody] TRANSACTION fc)
        {
            var item_sold = db.TRANSACTIONs.OrderByDescending(p => p.transaction_id).FirstOrDefault();
            item_sold.status = 2;
            item_sold.amount = fc.amount;
            db.SaveChanges();
            return Ok(item_sold);
        }

        [HttpPost]
        [Route("api/admin/SaveItemSold_CartShop")]
        public IHttpActionResult SaveItemSold_CartShop([FromBody] ITEM_SOLD fc)
        {
            ITEM_SOLD item = new ITEM_SOLD();
            item.product_id = fc.product_id;
            item.qty = fc.qty;
            item.price = fc.price;
            item.size = fc.size;
            item.transaction_id = fc.transaction_id;
            db.ITEM_SOLD.Add(item);
            db.SaveChanges();
            return Ok(item);
        }
    }
}
