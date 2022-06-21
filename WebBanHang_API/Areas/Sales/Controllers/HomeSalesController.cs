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
    [Route("api/homesales")]
    public class HomeSalesController : ApiController
    {
        Shop db = new Shop();

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

        // API lấy tất cả các sản phẩm đồ da, chuyển thành list dạng PRODUCT_Plus 
        [HttpGet]
        [Route("api/get_all_leather_products")]
        public IHttpActionResult Get_All_Leather_Products()
        {
            int doda = 2;
            var id_var = new SqlParameter("@group_id", doda);
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_from_PRODUCT_GROUP @group_id", id_var).ToList();
            int qty = result.Count();
            List<PRODUCT> product1list = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            for (int i = 0; i < qty; i++)
            {
                product1list.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(product1list, productpluslist);
            return Json(productpluslist);
        }
        

        // API Lấy các sản phẩm trong giỏ hàng của người dùng từ username
        [HttpGet]
        [Route("api/get_item_from_cart_by_username/{username}")]
        public IHttpActionResult Get_Item_From_Cart_By_Username(string username)
        {
            Models.Data data = new Models.Data();
            List<ItemInCart> itemincartlist = new List<ItemInCart>();
            data.GetItemInCart(itemincartlist, username);
            return Json(itemincartlist);
        }
    }
}
