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
    public class CategoryController : ApiController

    {
        Shop db = new Shop();
        public CategoryController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        
        [HttpGet]
        [Route("api/admin/getcategory")]
        public IHttpActionResult getCategory()
        {
            var result = db.CATEGORies.ToList();
            return Ok( result );
                //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
                //    Request.CreateResponse(HttpStatusCode.Created, mc)
                //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }
        [HttpPost]
        [Route("api/admin/insertcategory")]
        public IHttpActionResult postCategory([FromBody] CATEGORY cate)
        {
            var cate1 = db.CATEGORies.ToList();
            var id = new SqlParameter("@id", cate1.Last().category_id + 1);
            cate.category_id = cate1.Last().category_id + 1;
            var name = new SqlParameter("@name", cate.name);
            var group_id = new SqlParameter("@group_id", cate.group_id);
            db.Database.ExecuteSqlCommand("AddCategory @id, @name, @group_id", id, name, group_id);
            return Ok(new { data = cate });
        }
        [HttpPut]
        [Route("api/admin/updatecategory")]
        public IHttpActionResult putCategory(int id,[FromBody] CATEGORY cate)
        {
            var id_category = new SqlParameter("@id", id);
            var name = new SqlParameter("@name", cate.name);
            db.Database.ExecuteSqlCommand("EditCategory @id, @name", id_category, name);
            return Ok(new { data = "Sửa thành công!" });
        }

        [HttpDelete]
        [Route("api/admin/deletecategory")]

        public IHttpActionResult deleteCategory(int id)
        {
            var delete_id = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand("DeleteCategory @id", delete_id);
            return Ok(new { data = "Xóa thành công!" });
        }

        [HttpGet]
        [Route("api/admin/searchcategory")]

        public IHttpActionResult seatchCategory(string filter)
        {
            //  Lọc danh mục theo nhóm sản phẩm
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "3")
            {
                var result = db.CATEGORies.ToList();
                return Ok( new { data=result});
            }
            else
            {
                var result = db.CATEGORies.SqlQuery("FilterCategory @type", type).ToList();
                return Ok(result);
            }
        }
    }
}
