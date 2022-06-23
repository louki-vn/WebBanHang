using RestSharp;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Sales.Controllers
{
    public class LoginController : Controller
    {
        private readonly RestClient _client;

        public LoginController()
        {
            _client = new RestClient("https://localhost:44396/");
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            Session["is_logined"] = 0;
            ViewBag.fall_login = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string username = fc.Get("customer[email]").ToString();
            string pass = fc.Get("customer[password]").ToString();

            //----------------- TUYEN--------------------
            LoginIO lg = new LoginIO();

            var request = new RestRequest($"api/LoginCheck/{username}/{pass}");
            var response = _client.Execute<int>(request);
                 
            if (response.Data == 1)
            {
                var res = lg.Login(username, Data.MD5Hash(pass), true);
                if (res == -2)
                {
                    ViewBag.fall_login = "Mật khẩu không đúng!";
                    return View("~/Areas/Sales/Views/Login/Login.cshtml");

                }
                else
                {
                    Session["user_logined"] = username;
                    Session["is_logined"] = 1;
                    ViewBag.user_logined = Session["user_logined"];
                    ViewBag.is_logined = Session["is_logined"];

                    var req = new RestRequest($"api/get_member_by_username/{username}/", Method.Get);
                    var resp = _client.Execute<List<MEMBER>>(req).Data;
                    MEMBER member = resp[0];
                    var userSession = new UserLogin();
                    userSession.username = member.username;
                    userSession.member_id = member.member_id;
                    var listcredential = lg.GetListCredential(username);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listcredential);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    Session.Add(CommonConstants.SESSION_GROUPID, member.GroupId);
                    Session["member_id"] = member.member_id;
                    Session["group_id"] = member.GroupId;
                    Session["user_name"] = member.name;
                    Session["user_gmail"] = member.username;
                    if (res == 1)
                    {
                        var listCredentials = lg.GetListCredential(username);
                        //Session.Add(Constants.SS)
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                    }
                    else if (res == 2)
                    {
                        var listCredentials = lg.GetListCredential(username);
                        return RedirectToAction("Home", "HomeSales", new { area = "Sales" });
                    }
                }
            }
            else
            {
                ViewBag.fall_login = "Tài khoản không tồn tại!";
                return View("~/Areas/Sales/Views/Login/Login.cshtml");
            }      
            
            return View();
        }

        public ActionResult Logout()
        {
            Session["is_logined"] = 0;
            return View("~/Areas/Sales/Views/Login/Login.cshtml");
        }
    }
}