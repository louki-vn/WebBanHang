using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    public class CartShopController : Controller
    {
        // Khai báo database
        Shop db = new Shop();
        private static List<ItemInCart> dl = new List<ItemInCart>();
        private readonly RestClient _client;

        public CartShopController()
        {
            _client = new RestClient("https://localhost:44396/");
        }
        // GET: Admin/CartShop
        [HasCredential(RoleID = "SELL_OFLINE_ADMIN")]
        public ActionResult Index()
        {
            List<ItemInCart> ds = new List<ItemInCart>();

            if (Session["list"] != null)
            {
                foreach (var item in (List<ItemInCart>)Session["list"])
                {
                    ds.Add(item);
                }
            }
            else
            {
                return View(ds);
            }

            return View(ds);
        }

        //Sử lý lấy sản phẩm theo Id sp
        [HttpPost]
        public ActionResult GetProduct(string id)
        {
            int _id = int.Parse(id);
            var product_id = new SqlParameter("@product_id", _id);
            var request = new RestRequest($"api/admin/GetProduct_CartShop?id={id}");
            var dt = _client.Execute<PRODUCT>(request).Data;

            if (dt != null) 
            {
                foreach (var item in dl)
                {
                    if (item.product_id == dt.product_id)
                    {
                        item.qty++;
                        Session["list"] = dl;
                        return RedirectToAction("Index");
                    }
                }

                ItemInCart it = new ItemInCart();
                it.product_id = dt.product_id;
                it.name = dt.name;
                it.price = dt.price;
                it.size = dt.size;
                it.image_link = dt.image_link;
                it.qty = 1;

                dl.Add(it);

                //Lưu dữ liệu vào trong session
                Session["list"] = dl;

            }
            return RedirectToAction("Index");
        }

        // xử lý sự kiện tăng số lượng
        [HttpPost]
        public JsonResult Handle_qty(FormCollection data)
        {
            JsonResult Js = new JsonResult();

            string id = data["product_id"];
            string qty = data["qty"];

            int _qty = int.Parse(qty);
            int _id = int.Parse(id);

            foreach (var item in dl)
            {
                if (item.product_id == _id)
                {
                    item.qty = _qty;

                    Js.Data = new
                    {
                        status = "OK"
                    };
                }
            }

            Session["list"] = dl;
            return Json(Js, JsonRequestBehavior.AllowGet);
        }


        // Xoá tất cả sản phẩm trong giỏ
        public ActionResult RemoveAllItem()
        {
            Session["list"] = new List<ItemInCart>();
            dl = new List<ItemInCart>();
            return RedirectToAction("Index");
        }

        //Xoá từng sản phẩm một trong giỏ hàng
        public ActionResult RemoveItem(string id)
        {
            int _id = int.Parse(id);
            for (int i = 0; i < dl.Count; i++)
            {
                if (dl[i].product_id == _id)
                {
                    dl.RemoveAt(i);
                }
            }

            Session["list"] = dl;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult SaveInfomationUser(FormCollection data)
        {
            JsonResult Js = new JsonResult();

            string usename = data["usename"];
            string numberphone = data["numberphone"];
            if (usename == "" || numberphone == "")
            {
                Js.Data = new
                {
                    message = "Chưa đầy đủ thông tin người mua!"
                };
            }
            else
            {
                TRANSACTION info = new TRANSACTION();
                var member_id = Session["member_id"].ToString();
                info.member_name = usename;
                info.member_id = int.Parse(member_id);
                info.member_phone_number = numberphone;
                info.status = 1;
                info.payment = true;
                info.amount = 0;
                //db.TRANSACTIONs.Add(info);
                //db.SaveChanges();
                var request = new RestRequest($"api/admin/insertTransaction_CartShop", Method.Post).AddObject(info);
                _client.Execute(request);
                Js.Data = new
                {
                    status = "OK"
                };
            }

            return Json(Js, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveInfoPayment(FormCollection data)
        {
            JsonResult Js = new JsonResult();

            string amount = data["amount"];
            if (amount == "")
            {
                Js.Data = new
                {
                    message = "Lỗi nhập liệu!"
                };
            }
            else
            {
                //var item_sold = db.TRANSACTIONs.OrderByDescending(p => p.transaction_id).FirstOrDefault();

                TRANSACTION item_sold = new TRANSACTION();

                item_sold.status = 2;
                item_sold.amount = int.Parse(amount);
                //db.SaveChanges();
                var request = new RestRequest($"api/admin/SaveInfoPayment_CartShop", Method.Post).AddObject(item_sold);
                item_sold = _client.Execute<TRANSACTION>(request).Data;
                Js.Data = new
                {
                    status = "OK"
                };


                // lưu các sản phẩm vào trong db
                ITEM_SOLD item = new ITEM_SOLD();
                foreach (var i in dl)
                {
                    item.product_id = i.product_id;
                    item.qty = i.qty;
                    item.price = i.price;
                    item.size = i.size;
                    item.transaction_id = item_sold.transaction_id;

                    //db.ITEM_SOLD.Add(item);
                    //db.SaveChanges();
                    request = new RestRequest($"api/admin/SaveItemSold_CartShop", Method.Post).AddObject(item);
                    _client.Execute(request);
                }

            }

            dl = new List<ItemInCart>();
            Session["list"] = dl;

            return Json(Js, JsonRequestBehavior.AllowGet);
        }
    }
}