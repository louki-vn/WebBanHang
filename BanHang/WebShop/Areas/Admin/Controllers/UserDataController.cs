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
        // GET: Admin/UserData
        [HasCredential(RoleID = "DATA_MEMBER_ADMIN")]
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var result = db.MEMBERs.ToList();
            return View(result);
        }

        [HttpPost]
        public ActionResult DeleteMember()
        {
            var id = new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = TempData["delete_id"] };
            db.Database.ExecuteSqlCommand("DeleteMember @id", id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            //  Lọc danh sách thành viên theo vai trò
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "3")
            {
                var result = db.MEMBERs.ToList();
                return View("Index", result);
            }
            else
            {
                var result = db.MEMBERs.SqlQuery("FilterMember @type", type).ToList();
                return View("Index", result);
            }
        }
    }
}