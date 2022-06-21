using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;

namespace WebBanHang_API.Areas.Sales.Controllers
{
    public class UserInformationController : ApiController
    {
        Shop db = new Shop();

        [HttpGet]
        [Route("api/userinformation/getuserinfor/{username}/")]
        public IHttpActionResult GetUserInfor(string username)
        {
            var user_var = new SqlParameter("@username", username);
            var result = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user_var).ToList();
            return Json(result);
        }

        [HttpPut]
        [Route("api/userinformation/getuserinfor/{member_id}/{name}/{phone}/{address}")]
        public IHttpActionResult UpdateInfor(string member_id, string name, string phone, string address)
        {
            var member_id_var = new SqlParameter("@member_id", member_id);
            var name_var = new SqlParameter("@name", name);
            var phone_number_var = new SqlParameter("@phone_number", phone);
            var address_var = new SqlParameter("@address", address);
            db.Database.ExecuteSqlCommand("exec update_MEMBER_information @member_id, @name, @phone_number, @address",
                                                                                member_id_var, name_var, phone_number_var, address_var);
            return Json(1);
        }

        [HttpPut]
        [Route("api/userinformation/changepassword/{username}/{old_pass}/{new_pass}/{confirm_password}")]
        public IHttpActionResult ChangePassword(string username, string old_pass, string new_pass, string confirm_password)
        {
            Shop my = new Shop();
            var user = my.MEMBERs.Where(x => x.username == username).FirstOrDefault();
           
            if (user.password != Data.MD5Hash(old_pass))
            {
                return Ok("Fall_oldpass");
            }
            else if (new_pass != confirm_password)
            {
                return Ok("Fall_confirm");
            }
            else
            {
                user.password = Data.MD5Hash(new_pass);
                my.SaveChanges();                
                return Ok("OK");
            }
        }
    }
}
