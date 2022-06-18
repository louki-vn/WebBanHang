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

namespace WebShop.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        Shop db = new Shop();
        private readonly RestClient _client;

        public CategoryController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        // GET: Admin/Category
        [HasCredential(RoleID = "VIEW_CATEGORY_ADMIN")]
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest("api/admin/getcategory", Method.Get);

            var result = _client.Execute<List<CATEGORY>>(request).Data;
            //var cate = JsonSerializer.Deserialize<CATEGORY>(result.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
         
            return View(result);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_CATEGORY_ADMIN")]
        public ActionResult AddCategory(FormCollection fc)
        {
            var cate = db.CATEGORies.ToList();
            var id = new SqlParameter("@id", cate.Last().category_id + 1);
            var name = new SqlParameter("@name", fc["category"]);
            var group_id = new SqlParameter("@group_id", fc["group_id"]);

            db.Database.ExecuteSqlCommand("AddCategory @id, @name, @group_id", id,  name, group_id);

            return RedirectToAction("Index");
        }
        [HasCredential(RoleID = "EDIT_CATEGORY_ADMIN")]
        [HttpGet]
        public ActionResult EditCategory(string category_id, string category_name)
        {

            var request = new RestRequest($"api/admin/updatecategory");
            
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var result = db.CATEGORies.ToList();
            return RedirectToAction("Index");
        }
        [HasCredential(RoleID = "DELETE_CATEGORY_ADMIN")]
        [HttpGet]
        public ActionResult DeleteCategory(string delete_id)
        {
            //var id = new SqlParameter("@id", delete_id);
            //db.Database.ExecuteSqlCommand("DeleteCategory @id", id);

            var request = new RestRequest($"api/admin/deletecategory?id={delete_id}", Method.Delete);
            var result = _client.Execute(request);
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            //  Lọc danh mục theo nhóm sản phẩm
            //var type = new SqlParameter("@type", filter);
            //if (type.Value.ToString() == "3")
            //{
            //    var result = db.CATEGORies.ToList();
            //    return View("Index", result);
            //}
            //else
            //{
            //    var result = db.CATEGORies.SqlQuery("FilterCategory @type", type).ToList();
            //    return View("Index", result);
            //}

            var request = new RestRequest($"api/admin/searchcategory?filter={filter}", Method.Get);
            //var result = _client.Execute<List<CATEGORY>>(request).Content;
            var result = _client.Execute(request);
            var model = JsonConvert.DeserializeObject<List<CATEGORY>>(result.Content);
            return View("Index", result);
        }
    }
}