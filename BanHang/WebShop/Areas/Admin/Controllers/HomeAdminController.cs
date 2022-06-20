using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        Shop db = new Shop();
        private readonly RestClient _client;

        public HomeAdminController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        // GET: Admin/Home
        [HasCredential(RoleID = "VIEW_HOME_ADMIN")]
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/admin/get_count_HomeAdmin");
            var response = _client.Get(request).Content;
            JObject json = JObject.Parse(response);
            var mem = json.GetValue("count_mem");
            var order = json.GetValue("count_order");
            var item = db.ITEM_SOLD.ToList();
            //  Số lượng thành viên
            ViewBag.Member_count = mem;
            //  Tổng số đơn hàng
            ViewBag.Order_count = order;
         
            //  Số sản phẩm đã bán và tổng doanh thu
            ViewBag.Amount = json.GetValue("amount");
            ViewBag.Total = json.GetValue("total");

            var topproduct = db.PRODUCTs.SqlQuery("exec SelectTopProduct").ToList();
            ViewBag.TopProduct = topproduct;

            var category = db.CATEGORies.ToArray();
            Dictionary<int, string> p = new Dictionary<int, string>();
            foreach (var c in category)
            {
                p.Add(c.category_id, c.name);
            }
            ViewBag.Category = p;

            var brand = db.BRANDs.ToList();
            Dictionary<int, string> p1 = new Dictionary<int, string>();
            foreach (var i in brand)
            {
                p1.Add(i.brand_id, i.brand_name);
            }
            ViewBag.Brand = p1;

            var topmem = db.Database.SqlQuery<Mem_Cart>("exec SelectTopMember").ToList();
            ViewBag.TopMem = topmem;
            return View();
        }

    }
}