using Microsoft.EntityFrameworkCore;
using KiemTraWeb.Models;

namespace KiemTraWeb.Data
{
    public class KiemTraWebContext : DbContext
    {
        public KiemTraWebContext(DbContextOptions<KiemTraWebContext> options)
            : base(options)
        {
        }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<NganhHoc> NganhHocs { get; set; }
        public DbSet<HocPhan> HocPhans { get; set; }
        public DbSet<DangKy> DangKys { get; set; }
        public DbSet<ChiTietDangKy> ChiTietDangKys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NganhHoc>().HasData(
                new NganhHoc { MaNganh = "CNTT", TenNganh = "Công nghệ thông tin" },
                new NganhHoc { MaNganh = "KTPM", TenNganh = "Kỹ thuật phần mềm" },
                new NganhHoc { MaNganh = "HTTT", TenNganh = "Hệ thống thông tin" },
                new NganhHoc { MaNganh = "KHMT", TenNganh = "Khoa học máy tính" },
                new NganhHoc { MaNganh = "TMDT", TenNganh = "Thương mại điện tử" }
            );
        }
    }
}

