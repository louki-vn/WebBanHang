using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/Product
        [HasCredential(RoleID = "VIEW_PRODUCT_ADMIN")]
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            var product = db.PRODUCTs.ToList();
            var category = db.CATEGORies.ToArray();
            var brand = db.BRANDs.ToList();
            Dictionary<int, string> p1 = new Dictionary<int, string>();
            foreach (var item in brand)
            {
                p1.Add(item.brand_id, item.brand_name);
            }
            Dictionary<int, string> p = new Dictionary<int, string>();
            foreach(var item in category)
            {
                p.Add(item.category_id, item.name);
            }
            ViewBag.Brand = p1;
            ViewBag.Category = p;            
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
            var category_id = new SqlParameter("@category_id", fc["category_id"]);
            var name = new SqlParameter("@name", fc["productname"]);
            var price = new SqlParameter("@price", int.Parse(fc["price"]));
            var content = new SqlParameter("@content", fc["content"]);
            var brand = new SqlParameter("@brand", fc["brand"]);
            var size = new SqlParameter("@size", fc["size"]);
            var sale_id = new SqlParameter("@sale_id", int.Parse(fc["sale_id"]));
            var sold = new SqlParameter("@sold", int.Parse("0"));           
            var image_list = new SqlParameter("@image_list", "");
            string path = GetUrl(file);
            var image_link = new SqlParameter("@image_link", path.ToString());

            db.Database.ExecuteSqlCommand("AddProduct @name, @category_id, @sale_id, @price, @brand, @sold, @size, @content, @image_link, @image_list",
                                                        name, category_id, sale_id, price, brand, sold, size, content, image_link, image_list);
            return RedirectToAction("Index");
        }

        [HasCredential(RoleID = "DELETE_PRODUCT_ADMIN")]
        [HttpGet]
        public ActionResult DeleteProduct(string product_id)
        {
            var id = new SqlParameter("@id", product_id);
            db.Database.ExecuteSqlCommand("DeleteProduct @id", id);
            return RedirectToAction("Index");
        }
      
        [HttpGet]
        [HasCredential(RoleID = "EDIT_PRODUCT_ADMIN")]
        public ActionResult EditProduct(string product_id, string name, string size, string price, string content, string sale)
        {
            var id = new SqlParameter("@id", product_id);
            var name_var = new SqlParameter("@name", name);
            var size_var = new SqlParameter("@size", size);
            var price_var = new SqlParameter("@price", price);
            var content_var = new SqlParameter("@content", content);
            var sale_var = new SqlParameter("@sale", sale);

            db.Database.ExecuteSqlCommand("exec EditProduct @id, @name, @size, @price, @content, @sale",
                                                        id, name_var, size_var, price_var, content_var, sale_var);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            //  Lọc theo danh mục sản phẩm
            var category = db.CATEGORies.ToArray();
            var brand = db.BRANDs.ToList();
            List<string> p1 = new List<string>();
            foreach (var item in brand)
            {
                p1.Add(item.brand_name);
            }
            Dictionary<int, string> p = new Dictionary<int, string>();
            foreach (var item in category)
            {
                p.Add(item.category_id, item.name);
            }
            ViewBag.Brand = p1;
            ViewBag.Category = p;
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "0")
            {
                var result = db.PRODUCTs.ToList();
                return View("Index", result);
            }
            else
            {
                var result = db.PRODUCTs.SqlQuery("FilterProduct @type", type).ToList();
                return View("Index", result);
            }
        }
    }
}