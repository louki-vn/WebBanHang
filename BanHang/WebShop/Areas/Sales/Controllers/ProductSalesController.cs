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
                var request = new RestRequest($"api/productsales/add_transaction/{0}/{Session["member_id"].ToString()}/{usename}/{1}/{address}/{numberphone}/{amount}", Method.Post);
                var response = _client.Execute(request);
        
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
            var request2 = new RestRequest($"api/productsales/get_cart_item_by_cart_id/{member_id}", Method.Get);
            var response2 = _client.Execute<List<CART_ITEM>>(request2).Data;
            var request1 = new RestRequest($"api/productsales/get_all_transaction", Method.Get);
            var tran_id = _client.Execute<List<TRANSACTION>>(request1).Data;

            for (int i = 0; i < response2.Count(); i++)
            {               
                var request3 = new RestRequest($"api/productsales/add_item_sold/{response2[i].product_id}/{(int)response2[i].qty}/" +
                    $"{response2[i].price}/{response2[i].size}/{tran_id[0].transaction_id}", Method.Get);
                var response3 = _client.Execute(request3);
            }

            var request4 = new RestRequest($"api/productsales/deletecartitem/{member_id}", Method.Delete);
            var response4 = _client.Execute(request4);

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
                var request4 = new RestRequest($"api/productsales/update_number_product_in_cart/{cart_id}/{product_id}/{size}/{item_qty}", Method.Get);
                var res4 = _client.Execute<List<CART_ITEM>>(request4).Data;
            }
            else
            {
                float amount = float.Parse(price) * item_qty;
                var request5 = new RestRequest($"api/cart/add_cart_item/{cart_id}/{product_id}/{item_qty}/{amount}/{price}/{size}", Method.Post);
                var response = _client.Execute<int>(request5);
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
          
            return Content("1");
        }
    }
}