using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        Shop db = new Shop();
        private readonly RestClient _client;

        public OrderController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        // GET: Admin/Order
        public ActionResult Index()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/admin/getTransactions_admin");
            var result = _client.Execute<List<TRANSACTION>>(request).Data;
            return View(result);
        }

        public ActionResult Report(string id)
        {
            string url = $"api/admin/getTransactionById_admin?id=" + id;
            var request = new RestRequest(url);
            var result = _client.Execute<List<TRANSACTION>>(request).Data;

            url = $"api/admin/getProductInTransaction_admin?id=" + id;
            request = new RestRequest(url);
            ViewBag.Product_List = _client.Execute<List<Order_Products>>(request).Data;

            url = $"api/admin/getMemberInTransaction_admin?id=" + id;
            request = new RestRequest(url);
            ViewBag.Email = _client.Get(request).Content;
            return View(result);
        }


        [HttpGet]
        public ActionResult AddReport(string tran_id, string amount, string employee_id, string date, string status)
        {
            //  Báo cáo hoàn thành đơn hàng đồng thời chuyển trạng thái đơn hàng 
            REPORT rep = new REPORT();
            //var tran_id_var = new SqlParameter("@tran_id", int.Parse(tran_id));
            //var tran_id1 = new SqlParameter("@tran_id1", int.Parse(tran_id));
            //var employee_id_var = new SqlParameter("@employee_id", int.Parse(employee_id));
            //var amount_var = new SqlParameter("@amount", int.Parse(amount));
            //var date_var = new SqlParameter("@date", date);
            //var status_var = new SqlParameter("@status", int.Parse(status));
            //var status_var1 = new SqlParameter("@status1", int.Parse(status));
            rep.amount = int.Parse(amount);
            rep.employee_id = int.Parse(employee_id);
            rep.report_date = DateTime.Parse(date);
            rep.status = int.Parse(status);
            rep.transaction_id = int.Parse(tran_id);
            var request = new RestRequest($"api/admin/getCheckReport_admin?tran_id={tran_id}");
            //var result = db.REPORTs.SqlQuery("CheckReport @tran_id1", tran_id1).ToList();
            var result = _client.Execute<List<REPORT>>(request).Data;
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            request = new RestRequest($" api/admin/getTransactions_admin");
            var result1 = _client.Execute<List<TRANSACTION>>(request).Data;
            if (result.Count != 0)
            {
                request = new RestRequest($"api/admin/UpdateTransactionStatus", Method.Put).AddObject(rep);
                _client.Execute(request);
                //db.Database.ExecuteSqlCommand("UpdateTransactionStatus @tran_id, @status1", tran_id_var, status_var1);
                return View("~/Areas/Admin/Views/Order/Index.cshtml", result1);
            }
            else
            {
                request = new RestRequest($"api/admin/UpdateTransactionStatus", Method.Put).AddObject(rep);
                _client.Execute(request);
                //db.Database.ExecuteSqlCommand("UpdateTransactionStatus @tran_id, @status1", tran_id_var, status_var1);
                request = new RestRequest($"api/admin/insertReport",Method.Post).AddObject(rep);
                //db.Database.ExecuteSqlCommand("AddReport @tran_id, @employee_id,  @amount, @date, @status",
                //                                  tran_id_var, employee_id_var, amount_var, date_var, status_var);
                return View("~/Areas/Admin/Views/Order/Index.cshtml", result1);
            }           
        }

        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            var request = new RestRequest($"api/admin/searchTransaction?filter={filter}");
            //  Lọc đơn hàng theo tình trạng đơn hàng
            var type = new SqlParameter("@type", filter);
            var result = _client.Execute<List<TRANSACTION>>(request).Data;
            return View("Index", result);
        }
    }
}