namespace WebBanHang_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BRAND")]
    public partial class BRAND
    {
  /*      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BRAND()
        {
            PRODUCTs = new HashSet<PRODUCT>();
        }
*/
        [Key]
        public int brand_id { get; set; }

        [StringLength(50)]
        public string brand_name { get; set; }

        [StringLength(100)]
        public string description { get; set; }

  /*      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCT> PRODUCTs { get; set; }*/
    }
}
