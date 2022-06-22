using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Models;
namespace WebShop.Areas.Sales.Controllers
{
    public class ProductSalesController : Controller
    {
        // GET: Sales/Product
        Shop db = new Shop();
        //private static List<ItemInCart> dl = new List<ItemInCart>();
        private readonly RestClient _client;

        public ProductSalesController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        //GET: Product
        public JsonResult GetPro()
        {
            //List<PRODUCT> productlist = new List<PRODUCT>();
            //var result_product = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();
            //int qty = result_product.Count();
            //for (int i = 0; i < qty; i++)
            //{
            //    productlist.Add(result_product[i]);
            //}

            var request = new RestRequest($"api/productsales/getallproducts");
            var result_product = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            return Json(result_product, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Product()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];      
            var request = new RestRequest($"api/productsales/getallproducts");
            var result_product = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            int qty = result_product.Count();
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View(result_product);
        }

        public ActionResult Product_Detail(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/productsales/getallproducts");
            var result_product = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            int qty = result_product.Count();
            ViewBag.qty = qty;
            ViewBag.product_id = id;
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product_Detail.cshtml", result_product);
        }

        public ActionResult Add_To_Cart1(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            List<PRODUCT_Plus> productlist = new List<PRODUCT_Plus>();
            string user = ViewBag.user_logined;
            string size = "S";
            int item_qty = 1;
            Add_To_Cart(user, Int32.Parse(id), item_qty, size, productlist);
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productlist);
        }

        [HttpPost]
        public ActionResult Add_To_Cart2(FormCollection form, string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/productsales/getallproducts");
            var result = _client.Execute<List<PRODUCT_Plus>>(request).Data;            
            ViewBag.qty = result.Count();           
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            if (ViewBag.is_logined == 1)
            {
                string user = ViewBag.user_logined;
                string size = form.Get("option-1");
                if (size == null)
                {
                    size = "S";
                }
                int item_qty = Int32.Parse(form.Get("quantity").ToString());
                Add_To_Cart(user, Int32.Parse(id), item_qty, size, result);
            }
            else
            {
            }
            ViewBag.product_id = id;
            return RedirectToAction("Product_Detail/" + id);
        }

        [HttpPost]
        public JsonResult Add_To_Cart3(FormCollection fc)
        {
            JsonResult js = new JsonResult();
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            int id = Int32.Parse(fc["id"]);
            int quantity = Int32.Parse(fc["quantity"]);
            string size = fc["size"];
            var request = new RestRequest($"api/productsales/getallproducts");
            var result = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            int qty = result.Count();
            
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            if (ViewBag.is_logined == 1)
            {
                string user = ViewBag.user_logined;
                Add_To_Cart(user, id, quantity, size, result);
                js.Data = new
                {
                    status = "OK",
                };
            }
            else
            {
            }
            ViewBag.product_id = id;
            return Json(js, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveInfomationUser(FormCollection data)
        {
            JsonResult Js = new JsonResult();
            string usename = data["usename"];
            string numberphone = data["numberphone"];
            string address = data["address"];
            string amount = data["amount"];
            if (usename == "" || numberphone == "" || address == "")
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
                info.status = 0;
                info.delivery = address;
                info.payment = true;
                info.amount = Int32.Parse(amount);
                db.TRANSACTIONs.Add(info);
                db.SaveChanges();
                Js.Data = new
                {
                    status = "OK"
                };
            }
            return Json(Js, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveInfoPayment(FormCollection data)
        {
            JsonResult Js = new JsonResult();
            var member_id = Session["member_id"].ToString();
            var id_var = new SqlParameter("@id", member_id);
            var id_var2 = new SqlParameter("@id", member_id);
            List<CART_ITEM> list_cart_item = new List<CART_ITEM>();
            var result = db.Database.SqlQuery<CART_ITEM>("select * from CART_ITEM where cart_id = @id", id_var).ToList();
            List<ITEM_SOLD> list_item_sold = new List<ITEM_SOLD>();

            int mem_id2 = Int32.Parse(member_id);
            var id_mem = new SqlParameter("@id", Int32.Parse(member_id));
            var tran_id = db.TRANSACTIONs.OrderByDescending(p => p.transaction_id).ToList();
            for (int i = 0; i < result.Count(); i++)
            {
                var t = new ITEM_SOLD();
                t.product_id = result[i].product_id;
                t.price = result[i].price;
                t.qty = (int)result[i].qty;
                t.size = result[i].size;
                t.transaction_id = tran_id[0].transaction_id;
                db.ITEM_SOLD.Add(t);
            }

            db.Database.ExecuteSqlCommand("delete CART_ITEM where cart_id = @id", id_var2);
            db.SaveChanges();
            return Json(Js, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_Product_Base_On_Product_Group(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];           
            var request = new RestRequest($"api/productsales/get_product_base_on_product_group/{id}", Method.Get);
            var result = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            ViewBag.qty = result.Count();
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", result);
        }

        public ActionResult Get_Product_Base_On_Price(int id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/productsales/get_product_base_on_price/{id}", Method.Get);
            var result = _client.Execute<List<PRODUCT_Plus>>(request).Data;

            ViewBag.qty = result.Count();
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", result);
        }

        public ActionResult Get_Product_Base_On_Brand(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/productsales/get_product_base_on_brand/{id}", Method.Get);
            var result = _client.Execute<List<PRODUCT_Plus>>(request).Data;

            ViewBag.qty = result.Count();
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", result);
        }

        public ActionResult Get_Product_Base_On_Category(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/productsales/getproductbycategory/{id}");
            var result = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            int qty = result.Count();       
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", result);
        }

        public void Add_To_Cart(string user, int product_id, int item_qty, string size, List<PRODUCT_Plus> productlist)
        {
            //          Lấy tất cả sản phẩm cho vào 1 list, để sau khi thêm product vào item, quay lại view vẫn hiển thị được full product
            var request = new RestRequest($"api/productsales/getallproducts");
            var result = _client.Execute<List<PRODUCT_Plus>>(request).Data;
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            ViewBag.qty = qty;
            ViewBag.product_id = product_id;

            var request1 = new RestRequest($"api/productsales/getproductbyid/{product_id}", Method.Get);
            var res1 = _client.Execute<PRODUCT_Plus>(request1).Data;

            var request2 = new RestRequest($"api/get_member_by_username/{user}/", Method.Get);
            var res2 = _client.Execute<List<MEMBER>>(request2).Data;
            //          Lấy thông tin member đang đăng nhập và product đã được chọn (để tạo item)
            string price = res1.price.ToString();
            string cart_id = res2[0].member_id.ToString();
            ViewBag.cart_id = cart_id;
            //          Kiểm tra xem đã tồn tại sản phẩm đó trong giỏ hàng chưa
            var request3 = new RestRequest($"api/productsales/check_product_in_cart/{cart_id}/{product_id}/{size}", Method.Get);
            var res3 = _client.Execute<List<CART_ITEM>>(request3).Data;
            if (res3.Count() == 1)
            {
                var request4 = new RestRequest($"api/productsales/update_number_product_in_cart/{cart_id}/{product_id}/{size}/{qty}", Method.Get);
                var res4 = _client.Execute<List<CART_ITEM>>(request4).Data;
            }
            else
            {
                float amount = float.Parse(price) * item_qty;
                var request5 = new RestRequest($"api/add_cart_item/{cart_id}/{product_id}/{qty}/{amount}/{price}/{size}", Method.Post);
                var response = _client.Execute(request5);
            }
        }

        [HttpGet]
        public ActionResult Add_To_CartAjax(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            List<PRODUCT_Plus> productlist = new List<PRODUCT_Plus>();
            string user = ViewBag.user_logined;
            string size = "S";
            int item_qty = 1;
            if (ViewBag.is_logined == 1)
            {
                Add_To_Cart(user, Int32.Parse(id), item_qty, size, productlist);
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productlist);
        }

        public ActionResult Search(string keyword)
        {
            var request = new RestRequest($"api/productsales/search/{keyword}/", Method.Get);
            var result = _client.Execute<List<PRODUCT>>(request).Data;
            int qty = result.Count();
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", result);
        }

        public ActionResult Add_Review_Check(string product_id, string username)
        {
            bool check = false;
            var request = new RestRequest($"api/productsales/get_transaction_by_username/{username}/", Method.Get);
            var result = _client.Execute<List<TRANSACTION>>(request).Data;
            //var username_var = new SqlParameter("@username", username);
            //var result_transaction = db.Database.SqlQuery<TRANSACTION>("exec get_TRANSACTION_from_username @username", username_var).ToList();
            int[] check_arr = new int[20];
            for (int k = 0; k < 20; k++)
            {
                check_arr[k] = 0;
            }
            int i = 0;
            foreach (TRANSACTION a in result)
            {
                var request1 = new RestRequest($"api/productsales/get_item_from_transaction_by_id/{a.transaction_id}/{product_id}", Method.Get);
                var result1 = _client.Execute<int>(request1).Data;
                check_arr[i] = result1;
                //var transaction_id_var = new SqlParameter("@transaction_id", a.transaction_id);
                //var product_id_var = new SqlParameter("@product_id", product_id);
                //check_arr[i] = db.Database.SqlQuery<int>("exec check_ITEM_in_TRANSACTION @transaction_id, @product_id", transaction_id_var, product_id_var).FirstOrDefault();
                if (check_arr[i] == 1)
                {
                    check = true;
                    break;
                }
            }
            if (check == true)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        public ActionResult Add_Review(string username, string product_id, string review)
        {
            DateTime date = DateTime.Now;
            string a = date.ToString("dd/MM/yyyy");
            var request = new RestRequest($"api/productsales/add_review/{review}/{username}/{product_id}/{a}", Method.Get);
            var result = _client.Execute<int>(request).Data;
            //var datetime_var = new SqlParameter("@date_post", a);
            //var username_var = new SqlParameter("@username", username);
            //var product_id_var = new SqlParameter("@product_id", product_id);
            //var review_var = new SqlParameter("@content", review);
            //var result = db.Database.ExecuteSqlCommand("exec create_REVIEW @content, @username, @product_id, @date_post", review_var, username_var, product_id_var, datetime_var);
            return Content("1");
        }
    }
}