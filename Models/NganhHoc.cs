using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiemTraWeb.Models
{
    [Table("NganhHoc")]
    public class NganhHoc
    {
        [Key]
        [StringLength(4)]
        public string MaNganh { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string TenNganh { get; set; } = null!;

        public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
    }
}
