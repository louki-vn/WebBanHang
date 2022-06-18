using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebBanHang_API.Areas.Sales.Controllers
{
    public class ProductSalesController : ApiController
    {
        [Route("api/{controller}")]
        public class ProductSalesController : ApiController
        {
            Shop db = new Shop();
            [HttpGet]
            [Route("api/productsales/getallproducts")]
            public IHttpActionResult GetAllProducts()
            {
                var products_list = db.PRODUCTs.ToList();
                return Json(products_list);
            }
            [Route("api/productsales/getproductbyid/{id}")]
            public IHttpActionResult GetProductById(int id)
            {
                var products_list = db.PRODUCTs.ToList();
                PRODUCT p = new PRODUCT();
                foreach (var a in products_list)
                {
                    if (a.product_id == id)
                    {
                        p = a;
                    }
                }
                return Json(p);
            }
            [Route("api/productsales/getproductbybrand/{brand_name}")]
            public IHttpActionResult GetProductByBrand(string brand_name)
            {
                var u = new SqlParameter("@brand_name", brand_name);
                var products_list = db.Database.SqlQuery<PRODUCT>("exec get_product_from_brand_name @brand_name", u).ToList();
                return Json(products_list);
            }

            [Route("api/productsales/getproductbycategory/{id}")]
            public IHttpActionResult GetProductByCategory(int id)
            {
                var u = new SqlParameter("@id", id);
                var products_list = db.Database.SqlQuery<PRODUCT>("exec get_product_from_category_id @id", u).ToList();
                return Json(products_list);
            }
        }
    }
}
