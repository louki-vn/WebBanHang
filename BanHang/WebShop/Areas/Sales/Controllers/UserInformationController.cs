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
        Shop db = new Shop();
        // GET: UserInfomation

        [HasCredential(RoleID = "VIEW_INFORMATION_USER")]
        public ActionResult UserInformation()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            var user_var = new SqlParameter("@username", ViewBag.user_logined);
            var result = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user_var).ToList();
            MEMBER user = new MEMBER();
            user = result[0];

            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist,Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }

            return View(user);
        }
        [HasCredential(RoleID = "UPDATE_INFORMATION_USER")]
        public ActionResult update_information(string member_id, string name, string phone, string address)
        {
            var member_id_var = new SqlParameter("@member_id", member_id);
            var name_var = new SqlParameter("@name", name);
            var phone_number_var = new SqlParameter("@phone_number", phone);
            var address_var = new SqlParameter("@address", address);
            db.Database.ExecuteSqlCommand("exec update_MEMBER_information @member_id, @name, @phone_number, @address", 
                                                                                member_id_var, name_var, phone_number_var, address_var);

            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }

            return Content("1");
        }
        [HasCredential(RoleID = "CHANGE_PASSWORD_USER")]
        public ActionResult Change_Password()
        {

            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            var user_var = new SqlParameter("@username", ViewBag.user_logined);
            var result = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user_var).ToList();
            MEMBER user = new MEMBER();
            user = result[0];

            if (ViewBag.is_logined == 1)
            {
                Models.Data data = new Models.Data();
                List<ItemInCart> itemincartlist = new List<ItemInCart>();
                data.GetItemInCart(itemincartlist, Session["user_logined"].ToString());
                ViewBag.ItemInCart = itemincartlist;
                ViewBag.Number = itemincartlist.Count();
            }
            return View(user);
        }
        [HasCredential(RoleID = "CHANGE_PASSWORD_USER")]
        [HttpPost]

        public JsonResult Change_Password(FormCollection form)
        {
            Shop my = new Shop();

            string old_pass = form["old_pass"];
            string new_pass = form["new_pass"];
            string confirm_password = form["confirm_password"];
            var username = Session["user_logined"].ToString();
            var user = my.MEMBERs.Where(x => x.username == username).FirstOrDefault();
            JsonResult js = new JsonResult();
            if (user.password!= Data.MD5Hash(old_pass))
            {
                js.Data = new
                {
                    status = "Fall_oldpass",
                };
            }
            else if(new_pass != confirm_password)
            {
                js.Data = new
                {
                    status = "Fall_confirm",
                };
            }
            else
            {
                user.password = Data.MD5Hash(new_pass);

                my.SaveChanges();
                js.Data = new
                {
                    status = "OK",
                };
            }
            
            return Json(js, JsonRequestBehavior.AllowGet);
        }
    }
}