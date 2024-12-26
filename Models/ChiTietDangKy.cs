using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiemTraWeb.Models
{
    [Table("ChiTietDangKy")]
    public class ChiTietDangKy
    {
        [Key]
        public int MaCTDK { get; set; }

        [Required]
        public int MaDK { get; set; }

        [Required]
        [StringLength(6)]
        public string MaHP { get; set; } = null!;

        [ForeignKey("MaDK")]
        public virtual DangKy DangKy { get; set; } = null!;

        [ForeignKey("MaHP")]
        public virtual HocPhan HocPhan { get; set; } = null!;
    }
}

