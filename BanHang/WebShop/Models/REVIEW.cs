namespace WebShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("REVIEW")]
    public partial class REVIEW
    {
        [Key]
        [Column(Order = 0)]
        public int review_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string username { get; set; }

        public string review_text { get; set; }

        public int? product_id { get; set; }

        [StringLength(10)]
        public string date_post { get; set; }

        public virtual PRODUCT PRODUCT { get; set; }
    }
}
