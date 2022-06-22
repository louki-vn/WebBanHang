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

    [Route("api/productsales")]
    public class ProductSalesController : ApiController
    {
        Shop db = new Shop();
        [HttpGet]
        [Route("api/productsales/getallproducts")]
        public IHttpActionResult GetAllProducts()
        {
            var products_list = db.PRODUCTs.ToList();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            Mix_PRODUCT_And_PRODUCT_Plus(products_list, productpluslist);
            return Json(productpluslist);
        }

        [Route("api/productsales/getproductbyid/{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            var products_list = db.PRODUCTs.ToList();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            Mix_PRODUCT_And_PRODUCT_Plus(products_list, productpluslist);
            PRODUCT_Plus p = new PRODUCT_Plus();
            foreach (var a in productpluslist)
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
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            Mix_PRODUCT_And_PRODUCT_Plus(products_list, productpluslist);
            return Json(productpluslist);
        }

        [Route("api/productsales/getproductbycategory/{name}")]
        public IHttpActionResult GetProductByCategory(string name)
        {
            var u = new SqlParameter("@id", name);
            var products_list = db.Database.SqlQuery<PRODUCT>("exec get_product_from_CATEGORY @id", u).ToList();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            Mix_PRODUCT_And_PRODUCT_Plus(products_list, productpluslist);
            return Json(productpluslist);
        }

        [HttpGet]
        [Route("api/productsales/search/{keyword}")]
        public IHttpActionResult Search(string keyword)
        {
            var key_word_var = new SqlParameter("@key_word", keyword);
            var result = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_key_word @key_word", key_word_var).ToList();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            Mix_PRODUCT_And_PRODUCT_Plus(result, productpluslist);
            return Json(productpluslist);
        }

        public void Mix_PRODUCT_And_PRODUCT_Plus(List<PRODUCT> productlist, List<PRODUCT_Plus> productpluslist)
        {
            var result_sale = db.Database.SqlQuery<SALE>("exec get_all_from_SALES").ToList();
            foreach (var a in productlist)
            {
                PRODUCT_Plus c = new PRODUCT_Plus();
                c.product_id = a.product_id;
                c.category_id = a.category_id;
                c.sale_id = a.sale_id;
                c.name = a.name;
                c.price = a.price;
                c.brand_id = (int)a.brand_id;
                c.sold = a.sold;
                c.size = a.size;
                c.content = a.content;
                c.image_link = a.image_link;
                foreach (var b in result_sale)
                {
                    if (b.sale_id == a.sale_id)
                    {
                        c.sale_name = b.sale_name;
                        c.percent = b.percent;
                    }
                }
                productpluslist.Add(c);
            }
        }

        [HttpGet]
        [Route("api/productsales/get_product_base_on_product_group/{id}")]
        public IHttpActionResult Get_Product_Base_On_Product_Group(string id)
        {
            int id_int = Int32.Parse(id);
            var id_var = new SqlParameter("@group_id", id_int);
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_from_PRODUCT_GROUP @group_id", id_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            return Json(productpluslist);
        }

        [HttpGet]
        [Route("api/productsales/get_product_base_on_price/{id}")]
        public IHttpActionResult Get_Product_Base_On_Price(int id)
        {
            var along_var = new SqlParameter("@along", id);
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_base_on_price @along", along_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            return Json(productpluslist);
        }

        [HttpGet]
        [Route("api/productsales/get_product_base_on_brand/{id}")]
        public IHttpActionResult Get_Product_Base_On_Brand(string id)
        {
            var brand_var = new SqlParameter("@brand", Int32.Parse(id));
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_base_on_brand @brand", brand_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            return Json(productpluslist);
        }
    }
}

