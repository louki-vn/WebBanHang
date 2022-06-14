using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Models;

namespace WebShop.Common
{
    public class LoginIO
    {

        Shop db = new Shop();
        public List<string> GetListCredential(string userName)
        {
            var GroupId = db.MEMBERs.Where(p => p.username == userName).FirstOrDefault().GroupId;
            var credential = db.CREDENTIAL_S.Where(p => p.usergroup_id == GroupId).ToList();
            var data = credential.Select(p => p.role_id).ToList();
            return data;
        }
        public int Login(string username, string password, bool isLoginAdmin = false)
        {
            var result = db.MEMBERs.SingleOrDefault(x => x.username == username);
            if (result == null)
            {
                return 0;

            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupId == Constants.ADMIN_GROUP || result.GroupId==Constants.EMPLOYEES_GROUP)
                    {
                        if (result.password == password)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;
                        }
                    }
                    else
                    {
                        if (result.password == password)
                        {
                            return 2;
                        }
                        else
                        {
                            return -2;
                        }
                        
                    }
                }
                return -3;

            }
        }
    }
}