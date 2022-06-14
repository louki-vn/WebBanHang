namespace WebShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CART_ITEM
    {
        [Key]
        public int cart_item_id { get; set; }

        public int? cart_id { get; set; }

        public int? product_id { get; set; }

        public int? qty { get; set; }

        public int? amount { get; set; }

        public int? price { get; set; }

        [StringLength(5)]
        public string size { get; set; }

        public virtual CART CART { get; set; }

        public virtual PRODUCT PRODUCT { get; set; }
    }
}
