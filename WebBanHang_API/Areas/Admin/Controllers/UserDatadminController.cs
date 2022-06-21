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
    public class UserDatadminController : ApiController
    {
        Shop db = new Shop();
        public UserDatadminController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        [Route("api/admin/getMember_UserData")]

        public IHttpActionResult getMember_UserData()
        {
            var result = db.MEMBERs.ToList();
            return Ok(result);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpDelete]
        [Route("api/admin/deleteMember_UserData")]

        public IHttpActionResult deleteMember_UserData(string id)
        {
            var id_member = new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = id };
            db.Database.ExecuteSqlCommand("DeleteMember @id", id_member);
            return Ok(new { data = "Xóa thành công!" });
        }

        [HttpGet]
        [Route("api/admin/searchMember_UserData")]

        public IHttpActionResult searchMember_UserData(string filter)
        {
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "3")
            {
                var result = db.MEMBERs.ToList();
                return Ok(result);
            }
            else
            {
                var result = db.MEMBERs.SqlQuery("FilterMember @type", type).ToList();
                return Ok(result);
            }
            //  Lọc danh mục theo nhóm sản phẩm

        }
    }
}
