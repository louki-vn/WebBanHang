using RestSharp;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    public class UserDataController : Controller
    {
        Shop db = new Shop();
        private readonly RestClient _client;

        public UserDataController()
        {
            _client = new RestClient("https://localhost:44396/");
        }
        // GET: Admin/UserData
        [HasCredential(RoleID = "DATA_MEMBER_ADMIN")]
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/admin/getMember_UserData");
            var result = _client.Execute<List<MEMBER>>(request).Data;
            return View(result);
        }

        [HttpPost]
        public ActionResult DeleteMember(string id)
        {
          
            var request = new RestRequest($"api/admin/deleteMember_UserData?id=" + id, Method.Delete);
            _client.Execute(request);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            //  Lọc danh sách thành viên theo vai trò
            //var type = new SqlParameter("@type", filter);
            //if (type.Value.ToString() == "3")
            //{
            //    var result = db.MEMBERs.ToList();
            //    return View("Index", result);
            //}
            //else
            //{
            //    var result = db.MEMBERs.SqlQuery("FilterMember @type", type).ToList();
            //    return View("Index", result);
            //}
            var request = new RestRequest($"api/admin/searchMember_UserData?filter={filter}");
            var result = _client.Execute<List<MEMBER>>(request).Data;
            return View("Index", result);
        }
    }
}