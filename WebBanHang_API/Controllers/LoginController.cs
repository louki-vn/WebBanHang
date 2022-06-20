using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using WebBanHang_API.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Collections;

namespace WebBanHang_API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ApiController
    {
        Shop db = new Shop();

        [HttpGet]
        [Route("api/LoginCheck/{username}/{password}")]
        public IHttpActionResult LoginCheck(string username, string password)
        {
            var user_list = db.MEMBERs.ToList();
            int check = 0;
            password = Data.MD5Hash(password).ToString().Trim();
            foreach (var a in user_list)
            {
                if(a.username == username && (a.password.ToString().Contains(password) == true || password.Contains(a.password.ToString()) == true) )
                {
                    check = 1;
                }
            }
            return Ok(check);
        }

        public string encryption(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }
    }
}
