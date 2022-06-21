using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Sales.Controllers
{
    public class CartController : Controller
    {
        // GET: Sales/Cart
        Shop db = new Shop();
        private readonly RestClient _client;

        public CartController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        // GET: Cart

        public List<ItemInCart> Get_Data(string id, List<ItemInCart> itemincartlist)
        {
            var request = new RestRequest($"api/cart/get/{id}/{itemincartlist}", Method.Get).AddBody(itemincartlist);
            var res = _client.Execute<List<ItemInCart>>(request).Data;

            //var user = new SqlParameter("@username", id);
            //var result_member = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user).ToList();
            //int cart_id = result_member[0].member_id;

            //// Lấy cart của member đó và từ đó lấy ra những Item nằm trong cart đó.
            //var cart_id_var = new SqlParameter("@cart_id", cart_id);
            //var result_cart_item = db.Database.SqlQuery<CART_ITEM>("exec get_CART_ITEM_from_cart_id @cart_id", cart_id_var).ToList();

            //// Thêm những item đó vào 1 list rồi gửi sang View
            //List<CART_ITEM> cart_itemlist = new List<CART_ITEM>();
            //for (int i = 0; i < result_cart_item.Count(); i++)
            //{
            //    cart_itemlist.Add(result_cart_item[i]);
            //}

            ////          Tạo list product tương ứng với list cart_item
            //List<PRODUCT> productlist = new List<PRODUCT>();
            //foreach (var a in cart_itemlist)
            //{
            //    var p = new SqlParameter("@product_id", a.product_id);
            //    var result_product = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_product_id @product_id", p).ToList();
            //    productlist.Add(result_product[0]);
            //}
            ////          Tạo list ItemInCart để hiển thị trong cart     
            //for (int i = 0; i < cart_itemlist.Count(); i++)
            //{
            //    ItemInCart a = new ItemInCart();
            //    a.product_id = Int32.Parse(cart_itemlist[i].product_id.ToString());
            //    a.price = productlist[i].price;
            //    a.name = productlist[i].name;
            //    a.qty = Int32.Parse(cart_itemlist[i].qty.ToString());
            //    a.size = cart_itemlist[i].size;
            //    a.image_link = productlist[i].image_link;
            //    itemincartlist.Add(a);
            //}
            ViewBag.ItemInCart = res;
            ViewBag.Number = res.Count();
            return itemincartlist;
        }

        [HasCredential(RoleID = "VIEW_CART_USER")]
        public ActionResult Cart(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            string username = Session["user_logined"].ToString();

            var request = new RestRequest($"api/cart/getdata/{username}/", Method.Get);
            var res = _client.Execute(request);
            var response = System.Text.Json.JsonSerializer.Deserialize<List<ItemInCart>>(res.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.ItemInCart = response;
            ViewBag.Number = response.Count();
            return View(response);
        }

        [HasCredential(RoleID = "DELETE_PRODUCT_IN_CART_USER")]
        public ActionResult Remove_Item(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            var username = Session["user_logined"].ToString();
            var request = new RestRequest($"api/cart/removeitem/{username}/{id}", Method.Put);
            var res = _client.Execute<List<ItemInCart>>(request).Data;
            ViewBag.ItemInCart = res;
            ViewBag.Number = res.Count();
            return View("~/Areas/Sales/Views/Cart/Cart.cshtml", res);
        }

        [HasCredential(RoleID = "DELETE_ALL_PRODUCT_IN_CART_USER")]
        public ActionResult Remove_All_Item()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            db.Database.ExecuteSqlCommand("exec remove_all_CART_ITEM");

            var username = Session["user_logined"].ToString();
            List<ItemInCart> itemincartlist = new List<ItemInCart>();
            Get_Data(username, itemincartlist);
            return View("~/Areas/Sales/Views/Cart/Cart.cshtml", itemincartlist);
        }

        [HasCredential(RoleID = "DELETE_PRODUCT_IN_CART_USER")]
        [HttpPost]
        public ActionResult Remove_Item0(FormCollection data)
        {
            JsonResult Js = new JsonResult();
            string product_id = data["product_id"];
            string size = data["size"];
            SqlParameter[] pro_id = { new SqlParameter("@id", Int32.Parse(product_id)),
               new SqlParameter("@size", size)
               };
            db.Database.ExecuteSqlCommand("delete CART_ITEM where product_id = @id and size= @size", pro_id);
            Js.Data = new
            {
                status = "OK"
            };
            db.SaveChanges();

            return Json(Js, JsonRequestBehavior.AllowGet);
        }
        public void CheckOut(FormCollection form)
        {

        }
    }
}