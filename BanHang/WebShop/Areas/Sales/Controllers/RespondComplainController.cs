using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using WebShop.Common;
using WebShop.Models;

namespace WebShop.Areas.Sales.Controllers
{
    public class RespondComplainController : Controller
    {
        private readonly RestClient _client;

        public RespondComplainController()
        {
            _client = new RestClient("https://localhost:44396/");
        }
        // GET: Sales/RespondComplain
        [HasCredential(RoleID = "RESPOND_USER")]
        public ActionResult RespondComplain()
        {
            ViewBag.user_logined = Session["user_logined"];
            ViewBag.is_logined = Session["is_logined"];

            if (ViewBag.is_logined == 1)
            {
                string username = Session["user_logined"].ToString();
                var request6 = new RestRequest($"api/cart/getdata/{username}/");
                var response6 = _client.Execute<List<ItemInCart>>(request6).Data;
                ViewBag.ItemInCart = response6;
                ViewBag.Number = response6.Count();
            }

            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "RESPOND_USER")]
        public ActionResult SendEmail()
        {
            send();
            return View("RespondComplain");
        }

          
        static string emailFromAddress = "hieu7apro@gmail.com"; //Sender Email Address  
        static string password = "hieu72vn"; //Sender Password  
        static string emailToAddress = "Lethenhatt@gmail.com"; //Receiver Email Address  
        static string subject = "Hello";
        static string body = "Hello, This is Email sending test using gmail.";
        public  void send()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

    }
}