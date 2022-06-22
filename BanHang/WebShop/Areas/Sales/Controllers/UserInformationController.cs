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

        //[HasCredential(RoleID = "CHANGE_PASSWORD_USER")]
        //public ActionResult Change_Password()
        //{
        //    ViewBag.user_logined = Session["user_logined"];
        //    ViewBag.is_logined = Session["is_logined"];

        //    var user_var = new SqlParameter("@username", ViewBag.user_logined);
        //    var result = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user_var).ToList();
        //    MEMBER user = new MEMBER();
        //    user = result[0];

        //    if (ViewBag.is_logined == 1)
        //    {
        //        Models.Data data = new Models.Data();
        //        List<ItemInCart> itemincartlist = new List<ItemInCart>();
        //        data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
        //        ViewBag.ItemInCart = itemincartlist;
        //        ViewBag.Number = itemincartlist.Count();
        //    }
        //    return View(user);
        //}

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

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}