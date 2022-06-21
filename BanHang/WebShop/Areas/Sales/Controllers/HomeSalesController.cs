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
            string username = "";
            //if (ViewBag.user_logined != null)
            //{
            //    username = Session["user_logined"].ToString();
            //}

            //var request = new RestRequest($"api/homesales/home/{username}");
            //var res = _client.Execute<List<ItemInCart>>(request).Data;


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
            ViewBag.qty = qty;
            Mix_PRODUCT_And_PRODUCT_Plus(product1list, productpluslist);

            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, ViewBag.user_logined);
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }

            //ViewBag.ItemInCart = res;
            //ViewBag.Number = res.Count();


            return View("~/Areas/Sales/Views/HomeSales/Home.cshtml", productpluslist);
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
                var x = db.PRODUCTs.Where(p => p.product_id == _product_id).FirstOrDefault();
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
                item.price = x.price;
                item.amount = item.price * item.qty;

                db.CART_ITEM.Add(item);
                db.SaveChanges();

                Js.Data = new
                {
                    status = "OK",
                    image_link = x.image_link,
                    name = x.name,
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