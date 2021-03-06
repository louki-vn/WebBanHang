using RestSharp;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Sales.Controllers
{
    public class UserInformationController : Controller
    {       
        private readonly RestClient _client;

        public UserInformationController()
        {
            _client = new RestClient("https://localhost:44396/");
        }
        // GET: UserInfomation

        [HasCredential(RoleID = "VIEW_INFORMATION_USER")]
        public ActionResult UserInformation()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];
            string username = Session["user_logined"].ToString();
            var request = new RestRequest($"api/userinformation/getuserinfor/{username}/", Method.Get);
            var response = _client.Execute<List<MEMBER>>(request).Data;
            MEMBER user = new MEMBER();
            user = response[0];

            if (ViewBag.is_logined == 1)
            {                
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }

            return View(user);
        }

        [HasCredential(RoleID = "UPDATE_INFORMATION_USER")]
        public ActionResult update_information(string member_id, string name, string phone, string address)
        {
            var request = new RestRequest($"api/userinformation/getuserinfor/{member_id}/{name}/{phone}/{address}", Method.Put);
            var response = _client.Execute<int>(request).Data;
            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }

            return Content("1");
        }

        [HasCredential(RoleID = "CHANGE_PASSWORD_USER")]
        public ActionResult change_password()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            var req = new RestRequest($"api/get_member_by_username/{Session["user_logined"].ToString()}/", Method.Get);
            var response = _client.Execute<List<MEMBER>>(req).Data;

            MEMBER user = new MEMBER();
            user = response[0];

            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }
            return View(user);
        }

        [HasCredential(RoleID = "CHANGE_PASSWORD_USER")]
        [HttpPost]
        public JsonResult Change_Password(FormCollection form)
        {
            string old_pass = form["old_pass"];
            string new_pass = form["new_pass"];
            string confirm_password = form["confirm_password"];
            var username = Session["user_logined"].ToString();
            var request = new RestRequest($"api/userinformation/changepassword/{username}/{old_pass}/{new_pass}/{confirm_password}", Method.Put);
            var response = _client.Execute(request).Content;
            JsonResult js = new JsonResult();
            if(response== "\"OK\"")
            {
                js.Data = new
                {
                    status = "OK",
                };
            }
            else if (response == "\"Fall_oldpass\"")
            {
                js.Data = new
                {
                    status = "Fall_oldpass",
                };

            }
            else if (response == "\"Fall_confirm\"")
            {
                js.Data = new
                {
                    status = "Fall_confirm",
                };

            }
            return Json(js, JsonRequestBehavior.AllowGet);
        }
    }
}