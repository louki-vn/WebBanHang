using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;

namespace WebBanHang_API.Areas.Sales.Controllers
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
        [Route("api/productsales/getproductbybrand/{id}")]
        public IHttpActionResult GetProductByBrand(string id)
        {
            var u = new SqlParameter("@brand_name", id);
            var products_list = db.Database.SqlQuery<PRODUCT>("exec get_product_base_on_brand @brand_name", u).ToList();
            return Json(products_list);
        }

        [Route("api/productsales/getproductbycategory/{name}")]
        public IHttpActionResult GetProductByCategory(string name)
        {
            var u = new SqlParameter("@id", name);
            var products_list = db.Database.SqlQuery<PRODUCT>("exec get_product_from_CATEGORY @id", u).ToList();
            return Json(products_list);
        }

        [HttpGet]
        [Route("api/productsales/search/{keyword}")]
        public IHttpActionResult Search(string keyword)
        {
            var key_word_var = new SqlParameter("@key_word", keyword);           
            var result = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_key_word @key_word", key_word_var).ToList();
            return Json(result);
        }
    }
}
