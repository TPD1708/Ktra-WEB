using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace KiemTraWeb.Models
{
    [Table("SinhVien")]
    public class SinhVien
    {
        [Key]
        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        [StringLength(10, ErrorMessage = "Mã sinh viên không được vượt quá 10 ký tự")]
        [Display(Name = "Mã sinh viên")]
        public string MaSV { get; set; } = null!;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Giới tính không được để trống")]
        [StringLength(5, ErrorMessage = "Giới tính không được vượt quá 5 ký tự")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; } = null!;

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [StringLength(200)]
        [Display(Name = "Hình")]
        public string? Hinh { get; set; }

        [Required(ErrorMessage = "Ngành học không được để trống")]
        [StringLength(4, ErrorMessage = "Mã ngành không được vượt quá 4 ký tự")]
        [Display(Name = "Ngành học")]
        public string MaNganh { get; set; } = null!;

        [ForeignKey("MaNganh")]
        public virtual NganhHoc? NganhHoc { get; set; }

        [NotMapped]
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ImageFile { get; set; }
    }
}
    