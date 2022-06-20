using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web;
using System.Web.Http;

using WebBanHang_API.Models;

namespace WebBanHang_API.Areas.Admin.Controllers
{
    public class ProductAdminController : ApiController
    {
        Shop db = new Shop();
        public ProductAdminController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        [Route("api/admin/getproduct")]

        public IHttpActionResult getProduct()
        {
            var result = db.PRODUCTs.ToList();
            return Ok(result);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpGet]
        [Route("api/admin/get_listcategory_product")]

        public IHttpActionResult getList_CategoryProduct()
        {
            var category = db.CATEGORies.ToArray();
            Dictionary<int, string> p = new Dictionary<int, string>();
            foreach (var item in category)
            {
                p.Add(item.category_id, item.name);
            }
            return Ok(p);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpGet]
        [Route("api/admin/get_listbrand_product")]

        public IHttpActionResult getList_BrandProduct()
        {
            var category = db.CATEGORies.ToArray();
            var brand = db.BRANDs.ToList();
            Dictionary<int, string> p1 = new Dictionary<int, string>();
            foreach (var item in brand)
            {
                p1.Add(item.brand_id, item.brand_name);
            }
            return Ok(p1);
            //return con.THEMTHONGTINDANGKYCLVUKHITAU(id, mc) ?
            //    Request.CreateResponse(HttpStatusCode.Created, mc)
            //    : Request.CreateResponse(HttpStatusCode.BadRequest);

        }
       

        [HttpPost]
        [Route("api/admin/insertProduct")]
        public IHttpActionResult postCategory([FromBody] PRODUCT fc)
        {
            var category_id = new SqlParameter("@category_id", fc.category_id);
            var name = new SqlParameter("@name", fc.name);
            var price = new SqlParameter("@price", fc.price);
            var content = new SqlParameter("@content", fc.content);
            var brand = new SqlParameter("@brand", fc.brand_id);
            var size = new SqlParameter("@size", fc.size);
            var sale_id = new SqlParameter("@sale_id",fc.sale_id);
            var sold = new SqlParameter("@sold", int.Parse("0"));
            var image_list = new SqlParameter("@image_list", "");
            var image_link = new SqlParameter("@image_link", fc.image_link);
            db.Database.ExecuteSqlCommand("AddProduct @name, @category_id, @sale_id, @price, @brand, @sold, @size, @content, @image_link, @image_list", name, category_id, sale_id, price, brand, sold, size, content, image_link, image_list);
            return Ok(fc);
        }

        [HttpPut]
        [Route("api/admin/updateProduct")]
        public IHttpActionResult putProduct(int id, [FromBody] PRODUCT fc)
        {
            var id_pro = new SqlParameter("@id", id);
            var name_var = new SqlParameter("@name", fc.name);
            var size_var = new SqlParameter("@size", fc.size);
            var price_var = new SqlParameter("@price", fc.price);
            var content_var = new SqlParameter("@content", fc.content);
            var sale_var = new SqlParameter("@sale", fc.sale_id);

            db.Database.ExecuteSqlCommand("exec EditProduct @id, @name, @size, @price, @content, @sale",
                                                        id_pro, name_var, size_var, price_var, content_var, sale_var);
            return Ok(new { data = "Sửa thành công!" });
        }

        [HttpDelete]
        [Route("api/admin/deletecategory")]

        public IHttpActionResult deleteCategory(int id)
        {
            var id_product = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand("DeleteProduct @id", id_product);
            return Ok(new { data = "Xóa thành công!" });
        }

        [HttpGet]
        [Route("api/admin/searchProduct")]

        public IHttpActionResult seatchProduct(string filter)
        {
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "0")
            {
                var result = db.PRODUCTs.ToList();
                return Ok(result);
            }
            else
            {
                var result = db.PRODUCTs.SqlQuery("FilterProduct @type", type).ToList();
                return Ok(result);
            }
            //  Lọc danh mục theo nhóm sản phẩm
          
        }

    }
}
