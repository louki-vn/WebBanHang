namespace WebShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PRODUCT_Plus
    {
        public int product_id { get; set; }

        public int category_id { get; set; }

        public int? sale_id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public int price { get; set; }

        [StringLength(50)]
        public int brand_id { get; set; }

        public int sold { get; set; }

        [StringLength(5)]
        public string size { get; set; }

        [StringLength(500)]
        public string content { get; set; }

        [StringLength(50)]
        public string image_link { get; set; }

        [StringLength(50)]
        public string sale_name { get; set; }

        public int? percent { get; set; }

    }
}
