namespace WebBanHang_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MEMBER")]
    public partial class MEMBER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEMBER()
        {
            CARTs = new HashSet<CART>();
            TRANSACTIONs = new HashSet<TRANSACTION>();
        }

        [Key]
        public int member_id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        [Required]
        [StringLength(20)]
        public string phone_number { get; set; }

        [Required]
        [StringLength(100)]
        public string address { get; set; }

        public int? role { get; set; }

        public int? cart_id { get; set; }

        [StringLength(20)]
        public string GroupId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CART> CARTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRANSACTION> TRANSACTIONs { get; set; }

        public virtual USER_GROUP_S USER_GROUP_S { get; set; }
    }
}
