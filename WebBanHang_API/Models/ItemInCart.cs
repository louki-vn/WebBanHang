using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class ItemInCart
    {
        public int product_id { get; set; }

        public int qty { get; set; }

        public string name { get; set; }

        public int price { get; set; }

        public string size { get; set; }

        public string image_link { get; set; }
    }
}