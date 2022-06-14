using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/Order
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var result = db.TRANSACTIONs.ToList();
            return View(result);
        }

        public ActionResult Report(string id)
        {
            var tran_id = new SqlParameter("@tran_id", id);
            var tran_id_2 = new SqlParameter("@tran_id_2", id);
            var result = db.TRANSACTIONs.SqlQuery("GetTransaction @tran_id", tran_id).ToList();
            var product = db.Database.SqlQuery<Order_Products>("GetProductInTransaction @tran_id_2", tran_id_2).ToList();
            ViewBag.Product_List = product;
            var member_id = new SqlParameter("@member_id", result[0].member_id);
            var user = db.MEMBERs.SqlQuery("get_MEMBER_from_member_id @member_id", member_id).ToList();
            ViewBag.Email = user[0].username;
            return View(result);
        }


        [HttpGet]
        public ActionResult AddReport(string tran_id, string amount, string employee_id, string date, string status)
        {
            //  Báo cáo hoàn thành đơn hàng đồng thời chuyển trạng thái đơn hàng 
            var tran_id_var = new SqlParameter("@tran_id", int.Parse(tran_id));
            var tran_id1 = new SqlParameter("@tran_id1", int.Parse(tran_id));
            var employee_id_var = new SqlParameter("@employee_id", int.Parse(employee_id));
            var amount_var = new SqlParameter("@amount", int.Parse(amount));
            var date_var = new SqlParameter("@date", date);
            var status_var = new SqlParameter("@status", int.Parse(status));
            var status_var1 = new SqlParameter("@status1", int.Parse(status));
            var result = db.REPORTs.SqlQuery("CheckReport @tran_id1", tran_id1).ToList();
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var result1 = db.TRANSACTIONs.ToList();
            if (result.Count != 0)
            {
                db.Database.ExecuteSqlCommand("UpdateTransactionStatus @tran_id, @status1", tran_id_var, status_var1);
                return View("~/Areas/Admin/Views/Order/Index.cshtml", result1);
            }
            else
            {
                db.Database.ExecuteSqlCommand("UpdateTransactionStatus @tran_id, @status1", tran_id_var, status_var1);
                db.Database.ExecuteSqlCommand("AddReport @tran_id, @employee_id,  @amount, @date, @status",
                                                  tran_id_var, employee_id_var, amount_var, date_var, status_var);
            }           

            return View("~/Areas/Admin/Views/Order/Index.cshtml", result1);
        }

        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            //  Lọc đơn hàng theo tình trạng đơn hàng
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "3")
            {
                var result = db.TRANSACTIONs.ToList();
                return View("Index", result);
            }
            else
            {
                var result = db.TRANSACTIONs.SqlQuery("FilterTransaction @type", type).ToList();
                return View("Index", result);
            }
        }
    }
}