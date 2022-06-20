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
        private static List<ItemInCart> dl = new List<ItemInCart>();
        private readonly RestClient _client;

        public ProductSalesController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        //GET: Product
        public JsonResult GetPro()
        {
            List<PRODUCT> productlist = new List<PRODUCT>();
            var result_product = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();
            int qty = result_product.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result_product[i]);
            }
            return Json(productlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Product()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
        
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var request = new RestRequest($"api/productsales/getallproducts");
            var result_product = _client.Execute<List<PRODUCT>>(request).Data;
            int qty = result_product.Count();
            Mix_PRODUCT_And_PRODUCT_Plus(result_product, productpluslist);
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View(productpluslist);
        }

        public ActionResult Product_Detail(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
           
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var request = new RestRequest($"api/productsales/getallproducts");
            var result_product = _client.Execute<List<PRODUCT>>(request).Data;
            int qty = result_product.Count();
            
            Mix_PRODUCT_And_PRODUCT_Plus(result_product, productpluslist);
            ViewBag.qty = qty;
            ViewBag.product_id = id;
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product_Detail.cshtml", productpluslist);
        }

        public ActionResult Add_To_Cart1(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            List<PRODUCT> productlist = new List<PRODUCT>();
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
            
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var request = new RestRequest($"api/productsales/getallproducts");
            var result = _client.Execute<List<PRODUCT>>(request).Data;
            int qty = result.Count();
           
            ViewBag.qty = qty;
            Mix_PRODUCT_And_PRODUCT_Plus(result, productpluslist);
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
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
           
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var request = new RestRequest($"api/productsales/getallproducts");
            var result = _client.Execute<List<PRODUCT>>(request).Data;
            int qty = result.Count();
            
            ViewBag.qty = qty;
            Mix_PRODUCT_And_PRODUCT_Plus(result, productpluslist);
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
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
            int id_int = Int32.Parse(id);
            var id_var = new SqlParameter("@group_id", id_int);
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            //var request = new RestRequest($"api/productsales/getproductbyid/{id}");
            //var result = _client.Execute<List<PRODUCT>>(request).Data;
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_from_PRODUCT_GROUP @group_id", id_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productpluslist);
        }

        public ActionResult Get_Product_Base_On_Price(int id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var along_var = new SqlParameter("@along", id);
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_base_on_price @along", along_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productpluslist);
        }

        public ActionResult Get_Product_Base_On_Brand(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];         
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var request = new RestRequest($"api/productsales/getproductbybrand/{id}");
            var result = _client.Execute<List<PRODUCT>>(request).Data;
            int qty = result.Count();
           
            Mix_PRODUCT_And_PRODUCT_Plus(result, productpluslist);
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productpluslist);
        }

        public ActionResult Get_Product_Base_On_Category(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var name_var = new SqlParameter("@name", id);
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var request = new RestRequest($"api/productsales/getproductbycategory/{id}");
            var result = _client.Execute<List<PRODUCT>>(request).Data;
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(result, productpluslist);
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productpluslist);
        }

        public void Add_To_Cart(string user, int product_id, int item_qty, string size, List<PRODUCT> productlist)
        {
            //          Lấy tất cả sản phẩm cho vào 1 list, để sau khi thêm product vào item, quay lại view vẫn hiển thị được full product
            var result = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            ViewBag.qty = qty;
            ViewBag.product_id = product_id;
            //          Lấy thông tin member đang đăng nhập và product đã được chọn (để tạo item)
            var product_id_var = new SqlParameter("@product_id", product_id);
            var username = new SqlParameter("@username", System.Data.SqlDbType.NVarChar) { Value = user };
            var result_product = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_product_id @product_id", product_id_var).ToList();
            var result_member = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", username).ToList();
            string price = result_product[0].price.ToString();
            string cart_id = result_member[0].member_id.ToString();
            ViewBag.cart_id = cart_id;
            //          Kiểm tra xem đã tồn tại sản phẩm đó trong giỏ hàng chưa
            var product_id_var3 = new SqlParameter("@product_id", product_id);
            var cart_id_var2 = new SqlParameter("@cart_id", cart_id);
            var size_var1 = new SqlParameter("@size", size);
            var result_check = db.Database.SqlQuery<CART_ITEM>("exec CheckProductInCart @cart_id, @product_id, @size", cart_id_var2, product_id_var3, size_var1).ToList();
            if (result_check.Count() == 1)
            {
                var product_id_var2 = new SqlParameter("@product_id", product_id);
                var cart_id_var = new SqlParameter("@cart_id", cart_id);
                var size_var = new SqlParameter("@size", size);
                var qty_var = new SqlParameter("@qty", item_qty);
                db.Database.ExecuteSqlCommand("UpdateNumberProductInCartItem @cart_id, @product_id, @size,@qty", cart_id_var, product_id_var2, size_var, qty_var);
            }
            else
            {
                //          Tạo ITEM bằng thông tin của member(lấy cart_id) và product đó
                var product_id_var2 = new SqlParameter("@product_id", product_id);
                var price_var = new SqlParameter("@price", price);
                var cart_id_var = new SqlParameter("@cart_id", cart_id);
                var qty_var = new SqlParameter("@qty", item_qty);
                var size_var = new SqlParameter("@size", size);
                db.Database.ExecuteSqlCommand("exec create_CART_ITEM @cart_id, @product_id, @qty, @price, @size",
                                                            cart_id_var, product_id_var2, qty_var, price_var, size_var);
            }
        }

        [HttpGet]
        public ActionResult Add_To_CartAjax(string id)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            List<PRODUCT> productlist = new List<PRODUCT>();
            string user = ViewBag.user_logined;
            string size = "S";
            int item_qty = 1;
            if (ViewBag.is_logined == 1)
            {
                Add_To_Cart(user, Int32.Parse(id), item_qty, size, productlist);
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productlist);
        }

        public ActionResult Search(string key_word)
        {
            var key_word_var = new SqlParameter("@key_word", key_word);
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_key_word @key_word", key_word_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;
            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View("~/Areas/Sales/Views/ProductSales/Product.cshtml", productpluslist);
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

        public ActionResult Add_Review_Check(string product_id, string username)
        {
            bool check = false;
            var username_var = new SqlParameter("@username", username);
            var result_transaction = db.Database.SqlQuery<TRANSACTION>("exec get_TRANSACTION_from_username @username", username_var).ToList();
            int[] check_arr = new int[20];
            for (int k = 0; k < 20; k++)
            {
                check_arr[k] = 0;
            }
            int i = 0;
            foreach (TRANSACTION a in result_transaction)
            {
                var transaction_id_var = new SqlParameter("@transaction_id", a.transaction_id);
                var product_id_var = new SqlParameter("@product_id", product_id);
                check_arr[i] = db.Database.SqlQuery<int>("exec check_ITEM_in_TRANSACTION @transaction_id, @product_id", transaction_id_var, product_id_var).FirstOrDefault();
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
            var datetime_var = new SqlParameter("@date_post", a);
            var username_var = new SqlParameter("@username", username);
            var product_id_var = new SqlParameter("@product_id", product_id);
            var review_var = new SqlParameter("@content", review);
            var result = db.Database.ExecuteSqlCommand("exec create_REVIEW @content, @username, @product_id, @date_post", review_var, username_var, product_id_var, datetime_var);
            return Content("1");
        }
    }
}