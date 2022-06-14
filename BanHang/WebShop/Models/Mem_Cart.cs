using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class Mem_Cart
    {
        public int member_id { get; set; }

        public int cart_id { get; set; }

        public string name { get; set; }

        public string phone_number { get; set; }

        public string username { get; set; }

        public string address { get; set; }

        public int amount { get; set; }
    }
}