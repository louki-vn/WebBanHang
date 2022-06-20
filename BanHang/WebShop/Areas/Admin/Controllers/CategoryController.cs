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
            return View(result);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_CATEGORY_ADMIN")]
        public ActionResult AddCategory(FormCollection fc)
        {
            CATEGORY cate = new CATEGORY();
            cate.name = fc["category"];
            cate.group_id = int.Parse(fc["group_id"]);
            var request = new RestRequest($"api/admin/insertcategory", Method.Post).AddObject(cate);
            var res = _client.Execute(request);
            return RedirectToAction("Index");
        }
        [HasCredential(RoleID = "EDIT_CATEGORY_ADMIN")]
        [HttpGet]
        public ActionResult EditCategory(string category_id, string category_name)
        {
            CATEGORY cate = new CATEGORY();            
            cate.name = category_name;
            var request = new RestRequest($"api/admin/updatecategory?id={category_id}", Method.Put).AddObject(cate);
            var res = _client.Execute(request);                     
            return RedirectToAction("Index");
        }
        [HasCredential(RoleID = "DELETE_CATEGORY_ADMIN")]
        [HttpGet]
        public ActionResult DeleteCategory(string delete_id)
        {
            var request = new RestRequest($"api/admin/deletecategory?id={delete_id}", Method.Delete);
            _client.Execute(request);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            var request = new RestRequest($"api/admin/searchcategory?filter={filter}", Method.Get);
            var result = _client.Execute<List<CATEGORY>>(request).Data;      
            return View("Index", result);
        }
    }
}