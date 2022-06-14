namespace WebShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("REPORT")]
    public partial class REPORT
    {
        [Key]
        public int report_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? report_date { get; set; }

        public int? employee_id { get; set; }

        public int? transaction_id { get; set; }

        public int amount { get; set; }

        public int? status { get; set; }

        public virtual MEMBER MEMBER { get; set; }
    }
}
