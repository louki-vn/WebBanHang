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
    [Route("api/cart")]
    public class CartController : ApiController
    {
        Shop db = new Shop();

        [HttpGet]
        [Route("api/cart/get/{id}/{itemincartlist}")]
        public List<ItemInCart> Get_Data(string id, List<ItemInCart> itemincartlist)
        {
            var user = new SqlParameter("@username", id);
            var result_member = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user).ToList();
            int cart_id = result_member[0].member_id;

            // Lấy cart của member đó và từ đó lấy ra những Item nằm trong cart đó.
            var cart_id_var = new SqlParameter("@cart_id", cart_id);
            var result_cart_item = db.Database.SqlQuery<CART_ITEM>("exec get_CART_ITEM_from_cart_id @cart_id", cart_id_var).ToList();

            // Thêm những item đó vào 1 list rồi gửi sang View
            List<CART_ITEM> cart_itemlist = new List<CART_ITEM>();
            for (int i = 0; i < result_cart_item.Count(); i++)
            {
                cart_itemlist.Add(result_cart_item[i]);
            }

            //          Tạo list product tương ứng với list cart_item
            List<PRODUCT> productlist = new List<PRODUCT>();
            foreach (var a in cart_itemlist)
            {
                var p = new SqlParameter("@product_id", a.product_id);
                var result_product = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_product_id @product_id", p).ToList();
                productlist.Add(result_product[0]);
            }
            //          Tạo list ItemInCart để hiển thị trong cart     
            for (int i = 0; i < cart_itemlist.Count(); i++)
            {
                ItemInCart a = new ItemInCart();
                a.product_id = Int32.Parse(cart_itemlist[i].product_id.ToString());
                a.price = productlist[i].price;
                a.name = productlist[i].name;
                a.qty = Int32.Parse(cart_itemlist[i].qty.ToString());
                a.size = cart_itemlist[i].size;
                a.image_link = productlist[i].image_link;
                itemincartlist.Add(a);
            }
            return itemincartlist;
        }

        [HttpGet]
        [Route("api/cart/getdata/{username}")]
        public IHttpActionResult Cart(string username)
        {
            List<ItemInCart> itemincartlist = new List<ItemInCart>();
            itemincartlist = Get_Data(username, itemincartlist);
            return Json(itemincartlist);

        }

    }
}
