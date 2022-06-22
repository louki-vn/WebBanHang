using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;
using WebShop.Models;

namespace WebBanHang_API.Areas.Sales.Controllers
{
    [Route("api/cart")]
    public class CartController : ApiController
    {
        Shop db = new Shop();
        // Get_data là hàm chức năng phục vụ cho API CART nhé ae, anh em call vào cái CART ấy

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
        
        // API create cart_item
        [HttpPost]
        [Route("api/add_cart_item/{cart_id}/{product_id}/{qty}/{amount}/{price}/{size}")]
        public IHttpActionResult Add_Cart_Item(int cart_id, int product_id, int qty, int amount,int price, string size)
        {
            var cart_id_var = new SqlParameter("@cart_id", cart_id);
            var product_id_var = new SqlParameter("@product_id", product_id);
            var qty_var = new SqlParameter("@qty", qty);
            var amount_var = new SqlParameter("@amount", amount);
            var price_var = new SqlParameter("@price", price);
            var size_var = new SqlParameter("@size", size);
            db.Database.ExecuteSqlCommand("exec create_CART_ITEM @cart_id @product_id @qty @amount @price @size", cart_id_var, product_id_var, qty_var, amount_var, price_var, size_var);
            return Json(1);
        }
        
        // Api này thêm tham số username vì trong bản gốc có sử dụng username được lấy từ session.
        
        [HttpPut]
        [Route("api/cart/removeitem/{username}/{id}")]
        public IHttpActionResult Remove_Item(string username, string id, string size)
        {
            SqlParameter[] pro_id = { new SqlParameter("@id", Int32.Parse(id)),
               new SqlParameter("@size", size)
               };
            db.Database.ExecuteSqlCommand("delete CART_ITEM where product_id = @id and size= @size", pro_id);
            List<ItemInCart> itemincartlist = new List<ItemInCart>();
            itemincartlist = Get_Data(username, itemincartlist);
            return Json(itemincartlist);
        }
        
    }
}

