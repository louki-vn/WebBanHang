using RestSharp;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Sales.Controllers
{
    public class HomeSalesController : Controller
    {
        // GET: Sales/Home
        Shop db = new Shop();
        private readonly RestClient _client;

        public HomeSalesController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        [HttpGet]
        public ActionResult Home()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
        
            var request = new RestRequest($"api/get_all_leather_products", Method.Get);
            var res = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            ViewBag.qty = res.Count();

            if (ViewBag.is_logined == 1)
            {
                var request1 = new RestRequest($"api/get_item_from_cart_by_username/{Session["user_logined"].ToString()}/", Method.Get);
                var response = _client.Execute<List<ItemInCart>>(request1).Data;                
                ViewBag.itemincart = response;
                ViewBag.number = response.Count();
            }
            return View("~/Areas/Sales/Views/HomeSales/Home.cshtml", res);
        }

        [HttpPost]
        public JsonResult HandleAdd_Cart(FormCollection data)
        {
            JsonResult Js = new JsonResult();
            string member_id = Session["member_id"].ToString();
            string product_id = data["product_id"];
            int _member_id = int.Parse(member_id);

            if (product_id == "")
            {
                Js.Data = new
                {
                    message = "Lỗi truy xuất!"
                };
            }
            else
            {
                int _product_id = int.Parse(product_id);
                var request = new RestRequest($"api/productsales/getproductbyid/{_product_id}", Method.Get);
                var res = _client.Execute<PRODUCT_Plus>(request).Data;
                //var x = db.PRODUCTs.Where(p => p.product_id == _product_id).FirstOrDefault();
                var itemincart = db.CART_ITEM.Where(p => p.product_id == _product_id).ToList();
                foreach (var it in itemincart)
                {
                    if (_member_id == it.cart_id && it.product_id == _product_id)
                    {
                        CART_ITEM i = new CART_ITEM();
                        i.qty = i.qty + 1;
                        i.amount = i.qty * i.price;
                        db.SaveChanges();
                        Js.Data = new
                        {
                            status = "OK",
                            no_add = 1
                        };
                        return Json(Js, JsonRequestBehavior.AllowGet);
                    }
                }

                CART_ITEM item = new CART_ITEM();
                item.cart_id = int.Parse(member_id);
                item.product_id = int.Parse(product_id);
                item.qty = 1;
                item.size = "S";
                item.price = res.price;
                item.amount = item.price * item.qty;

                db.CART_ITEM.Add(item);
                db.SaveChanges();

                Js.Data = new
                {
                    status = "OK",
                    image_link = res.image_link,
                    name = res.name,
                    size = item.size,
                    qty = item.qty,
                    amount = item.amount,
                    price = item.price,
                    no_add = 0
                };
            }

            return Json(Js, JsonRequestBehavior.AllowGet);
        }
    }
}