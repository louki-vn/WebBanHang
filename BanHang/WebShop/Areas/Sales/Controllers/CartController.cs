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
        private readonly RestClient _client;

        public CartController()
        {
            _client = new RestClient("https://localhost:44396/");
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

            var req = new RestRequest($"api/get_member_by_username/{Session["user_logined"].ToString()}/", Method.Get);
            var response = _client.Execute<List<MEMBER>>(req).Data;

            var request = new RestRequest($"api/cart/removeallitem/{response[0].cart_id}", Method.Post);
            var res = _client.Execute<int>(request).Data;
            List<ItemInCart> itemincartlist = new List<ItemInCart>();
            ViewBag.ItemInCart = itemincartlist;
            ViewBag.Number = itemincartlist.Count();         
            return View("~/Areas/Sales/Views/Cart/Cart.cshtml", itemincartlist);
        }

        [HasCredential(RoleID = "DELETE_PRODUCT_IN_CART_USER")]
        [HttpPost]
        public ActionResult Remove_Item0(FormCollection data)
        {
            JsonResult Js = new JsonResult();
            string product_id = data["product_id"];
            string size = data["size"];
            //SqlParameter[] pro_id = { new SqlParameter("@id", Int32.Parse(product_id)),
            //   new SqlParameter("@size", size)
            //   };
            //db.Database.ExecuteSqlCommand("delete CART_ITEM where product_id = @id and size= @size", pro_id);
            //Js.Data = new
            //{
            //    status = "OK"
            //};
            //db.SaveChanges();

            var request = new RestRequest($"api/cart/removeitem/{Session["user_logined"].ToString()}/{product_id}/{size}", Method.Put);
            var res = _client.Execute<List<ItemInCart>>(request).Data;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public void CheckOut(FormCollection form)
        {

        }
    }
}