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
    [Route("api/createaccount")]
    public class CreateAccountController : ApiController
    {
        Shop db = new Shop();
        
/*        [HttpPost]
        [Route("api/createaccount/addmember/{username}/{name}/{password}/{phonenumber}/{address}")]
        public IHttpActionResult Add_Member(string username, string name, string password, string phonenumber, string address)
        {
            int success = 0;
            var u = new SqlParameter("@username", username);
            var result = db.Database.SqlQuery<MEMBER>("exec getMEMBERfromusername @username", u).ToList();
            int check = result.Count();
            if(check == 0)
            {
                var username2var = new SqlParameter("@username", username);
                var passvar = new SqlParameter("@password", Data.MD5Hash(password));
                var namevar = new SqlParameter("@name", name);
                var phonevar = new SqlParameter("@phone", phonenumber);
                var addressvar = new SqlParameter("@address", address);
                db.Database.ExecuteSqlCommand("exec AddMember @name, @username, @password, @phone, @address",
                                                            namevar, username2var, passvar, phonevar, addressvar);
                success = 1;
            }
            return Json(success);*/
    
        [HttpPost]
        [Route("api/add_member/{username}/{name}/{password}/{phonenumber}/{address}")]
        public IHttpActionResult Add_Member(string username, string name, string password, string phonenumber, string address)
        {
            int success = 1;
            var username2var = new SqlParameter("@username", username);
            var passvar = new SqlParameter("@password", Data.MD5Hash(password));
            var namevar = new SqlParameter("@name", name);
            var phonevar = new SqlParameter("@phone", phonenumber);
            var addressvar = new SqlParameter("@address", address);
            db.Database.ExecuteSqlCommand("exec AddMember @name, @username, @password, @phone, @address",
                                                        namevar, username2var, passvar, phonevar, addressvar);
            return Json(success); 
        }

        // lấy thông tin member từ username của member đó
        [HttpGet]
        [Route("api/get_member_by_username/{username}")]
        public IHttpActionResult Get_Member_By_Username(string username)
        {
            var u = new SqlParameter("@username", username);
            var result = db.Database.SqlQuery<MEMBER>("exec getMEMBERfromusername @username", u).ToList();
            return Json(result);
        }
    }
}
