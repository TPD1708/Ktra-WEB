using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiemTraWeb.Models
{
    [Table("DangKy")]
    public class DangKy
    {
        [Key]
        public int MaDK { get; set; }

        [Required]
        [StringLength(10)]
        public string MaSV { get; set; } = null!;

        [Required]
        public DateTime NgayDK { get; set; }

        public bool DaXacNhan { get; set; }

        [ForeignKey("MaSV")]
        public virtual SinhVien SinhVien { get; set; } = null!;

        public virtual ICollection<ChiTietDangKy> ChiTietDangKys { get; set; } = new List<ChiTietDangKy>();
    }
}