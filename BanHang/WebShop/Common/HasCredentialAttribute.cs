using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShop.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserLogin)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                return false;

            }
            string privilegeLevels = string.Join(";", this.getCredentialByLoggedInUser(session.username));
            if (privilegeLevels.Contains(this.RoleID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filerContext)
        {
            filerContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Sales/Views/Shared/Not_Permission.cshtml"
            };


        }
        private List<string> getCredentialByLoggedInUser(string userName)
        {
            var credential = (List<string>)HttpContext.Current.Session[CommonConstants.SESSION_CREDENTIALS];
            return credential;
        }
    }
}