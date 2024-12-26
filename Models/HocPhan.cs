using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiemTraWeb.Models
{
    [Table("HocPhan")]
    public class HocPhan
    {
        [Key]
        [StringLength(6)]
        public string MaHP { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string TenHP { get; set; } = null!;

        public int SoTinChi { get; set; }

        public int SoLuongDuKien { get; set; }

        public virtual ICollection<ChiTietDangKy> ChiTietDangKys { get; set; } = new List<ChiTietDangKy>();
    }
}
    