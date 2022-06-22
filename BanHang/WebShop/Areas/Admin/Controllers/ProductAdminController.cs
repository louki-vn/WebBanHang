using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;
using RestSharp;

namespace WebShop.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        Shop db = new Shop();
        private readonly RestClient _client;

        public ProductAdminController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        // GET: Admin/Product
        [HasCredential(RoleID = "VIEW_PRODUCT_ADMIN")]
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            var request = new RestRequest($"api/admin/getproduct");
     
            var product = _client.Execute<List<PRODUCT>>(request).Data;

            request = new RestRequest($"api/admin/get_listcategory_product");
            var category = _client.Execute<Dictionary<int, string>>(request).Data;

            request = new RestRequest($"api/admin/get_listbrand_product");
            
            ViewBag.Brand = _client.Execute<Dictionary<int, string>>(request).Data;
            ViewBag.Category = category;
            return View(product);
        }

        //  Lấy link ảnh
        public string GetUrl(HttpPostedFileBase file)
        {
            string path = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        path = Path.Combine(Server.MapPath("~/Asset/Images"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                    }
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
            }
            path = path.Replace('\\', '/');
            int i = path.IndexOf("Asset");
            path = path.Substring(i - 1);
            return path;
        }
        [HasCredential(RoleID = "ADD_PRODUCT_ADMIN")]
        [HttpPost]
        public ActionResult AddProduct(FormCollection fc, HttpPostedFileBase file)
        {
            PRODUCT pro = new PRODUCT();

            pro.category_id = int.Parse(fc["category_id"]);
            pro.name = fc["productname"];
            pro.price = int.Parse(fc["price"]);
            pro.content = fc["content"];
            pro.brand_id = int.Parse(fc["brand"]);
            pro.size = fc["size"];
            pro.sale_id = int.Parse(fc["sale_id"]);
            pro.sold = int.Parse("0");
            pro.image_list = "";
            string path = GetUrl(file);
            pro.image_link = path.ToString();
            var request = new RestRequest($"api/admin/insertProduct", Method.Post).AddObject(pro);
            _client.Execute(request);   
            return RedirectToAction("Index");
        }

        [HasCredential(RoleID = "DELETE_PRODUCT_ADMIN")]
        [HttpGet]
        public ActionResult DeleteProduct(string product_id)
        {
            var request = new RestRequest($"api/admin/DeleteProduct?id={product_id}", Method.Delete);
            _client.Execute(request);
            return RedirectToAction("Index");
        }
      
        [HttpGet]
        [HasCredential(RoleID = "EDIT_PRODUCT_ADMIN")]
        public ActionResult EditProduct(string product_id, string name, string size, string price, string content, string sale)
        {
            PRODUCT pro = new PRODUCT();
            pro.name = name;
            pro.size = size;
            pro.price = int.Parse(price);
            pro.content = content;
            pro.sale_id = int.Parse(sale);
            var request = new RestRequest($"api/admin/updateProduct?id={product_id}", Method.Put).AddObject(pro);
            _client.Execute(request);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            //  Lọc theo danh mục sản phẩm
            
            var request = new RestRequest($"api/admin/get_listbrand_product");
            ViewBag.Brand = _client.Execute<Dictionary<int, string>>(request).Data;
            request = new RestRequest($"api/admin/get_listcategory_product");
            ViewBag.Category = _client.Execute<Dictionary<int, string>>(request).Data;
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "0")
            {
                request = new RestRequest($"api/admin/getproduct");
                var result = _client.Execute<List<PRODUCT>>(request).Data;
                return View("Index", result);
            }
            else
            {
                request = new RestRequest($"api/admin/searchProduct?filter={filter}");
                var result = _client.Execute<List<PRODUCT>>(request).Data;
                return View("Index", result);
            }
        }
    }
}