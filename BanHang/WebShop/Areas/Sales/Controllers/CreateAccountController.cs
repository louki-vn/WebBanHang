using RestSharp;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Areas.Sales.Controllers
{
    public class CreateAccountController : Controller
    {
        //Shop db = new Shop();
        private readonly RestClient _client;

        public CreateAccountController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        // GET: CreateAccount
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(FormCollection form)
        {
            string username = form.Get("customer[username]").ToString();
            string pass = form.Get("customer[password]").ToString();
            string address = form.Get("customer[address]").ToString();
            string phone = form.Get("customer[phone]").ToString();
            string name = form.Get("customer[name]").ToString();

            var req = new RestRequest($"api/get_member_by_username/{username}/", Method.Get);
            var response = _client.Execute<List<MEMBER>>(req).Data;

            if (response.Count() == 0)
            {
                var request = new RestRequest($"api/add_member/{username}/{name}/{pass}/{phone}/{address}", Method.Post);
                var res = _client.Execute<int>(request);
                if (res.Data == 1)
                {
                    Session["user_logined"] = username;
                    Session["is_logined"] = 1;
                    ViewBag.user_logined = Session["user_logined"];
                    ViewBag.is_logined = Session["is_logined"];
                    return RedirectToAction("Home", "HomeSales", new { area = "Sales" });
                }
                return View();
            }
            else
            {
                return View();                
            }
        }       
    }
}