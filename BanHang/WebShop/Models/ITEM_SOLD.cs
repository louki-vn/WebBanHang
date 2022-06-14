namespace WebShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ITEM_SOLD
    {
        [Key]
        public int item_sold_id { get; set; }

        public int? product_id { get; set; }

        public int qty { get; set; }

        public int? price { get; set; }

        [StringLength(5)]
        public string size { get; set; }

        public int? transaction_id { get; set; }

        public virtual TRANSACTION TRANSACTION { get; set; }
    }
}
