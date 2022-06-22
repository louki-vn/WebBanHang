using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;

namespace WebBanHang_API.Areas.Admin.Controllers
{
    public class OderAdminController : ApiController
    {
        Shop db = new Shop();
        public OderAdminController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        [HttpGet]
        [Route("api/admin/getTransactions_admin")]

        public IHttpActionResult getTransactions()
        {
            var result = db.TRANSACTIONs.ToList();
            return Ok(result);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }
        [HttpGet]
        [Route("api/admin/getTransactionById_admin")]

        public IHttpActionResult getTransactionById(string id)
        {
            var tran_id = new SqlParameter("@tran_id", id);
            var result = db.TRANSACTIONs.SqlQuery("GetTransaction @tran_id", tran_id).ToList();
            return Ok(result);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }
        [HttpGet]
        [Route("api/admin/getProductInTransaction_admin")]

        public IHttpActionResult getProductInTransaction(string id)
        {
            var tran_id_2 = new SqlParameter("@tran_id_2", id);
            var product = db.Database.SqlQuery<Order_Products>("GetProductInTransaction @tran_id_2", tran_id_2).ToList();
            return Ok(product);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }
        [HttpGet]
        [Route("api/admin/getMemberInTransaction_admin")]

        public IHttpActionResult getMemberInTransaction(string id)
        {
            var tran_id = new SqlParameter("@tran_id", id);
            var result = db.TRANSACTIONs.SqlQuery("GetTransaction @tran_id", tran_id).ToList();
            if (result.Count <= 0)
            {
                return NotFound();
            }
            var member_id = new SqlParameter("@member_id", result[0].member_id);
            var user = db.MEMBERs.SqlQuery("get_MEMBER_from_member_id @member_id", member_id).ToList();
            if (user.Count <= 0)
            {
                return NotFound();
            }
            var username = user[0].username;
            return Ok(username);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpGet]
        [Route("api/admin/getCheckReport_admin")]
        public IHttpActionResult getCheckReport(string tran_id)
        {
            var tran_id1 = new SqlParameter("@tran_id1", int.Parse(tran_id));
            var result = db.REPORTs.SqlQuery("CheckReport @tran_id1", tran_id1).ToList();
            return Ok(result);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        [Route("api/admin/insertReport")]

        public IHttpActionResult insertReport([FromBody] REPORT rp)
        {
            var tran_id_var = new SqlParameter("@tran_id",rp.transaction_id);
            var employee_id_var = new SqlParameter("@employee_id", rp.employee_id);
            var amount_var = new SqlParameter("@amount", rp.amount);
            var date_var = new SqlParameter("@date", rp.report_date);
            var status_var = new SqlParameter("@status", rp.status);
            db.Database.ExecuteSqlCommand("AddReport @tran_id, @employee_id,  @amount, @date, @status",tran_id_var, employee_id_var, amount_var, date_var, status_var);
            return Ok(rp);
        }

        [HttpPut]
        [Route("api/admin/UpdateTransactionStatus")]
        public IHttpActionResult UpdateTransactionStatus([FromBody] REPORT rp)
        {
            var tran_id_var = new SqlParameter("@tran_id", rp.transaction_id);
            var status_var1 = new SqlParameter("@status1", rp.status);

            db.Database.ExecuteSqlCommand("UpdateTransactionStatus @tran_id, @status1", tran_id_var, status_var1);
            return Ok(new { data = "Sửa thành công!" });
        }

        [HttpGet]
        [Route("api/admin/searchTransaction")]

        public IHttpActionResult searchTransaction(string filter)
        {
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "3")
            {
                var result = db.TRANSACTIONs.ToList();
                return Ok(result);
            }
            else
            {
                var result = db.TRANSACTIONs.SqlQuery("FilterTransaction @type", type).ToList();
                return Ok(result);
            }
            //  Lọc danh mục theo nhóm sản phẩm

        }
    }
}
