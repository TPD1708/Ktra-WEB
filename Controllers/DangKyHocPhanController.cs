using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KiemTraWeb.Data;
using KiemTraWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace KiemTraWeb.Controllers
{
    [Authorize]
    public class DangKyHocPhanController : Controller
    {
        private readonly KiemTraWebContext _context;

        public DangKyHocPhanController(KiemTraWebContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hocPhans = await _context.HocPhans.ToListAsync();
            return View(hocPhans);
        }

        [HttpPost]
        public async Task<IActionResult> DangKy(string maHP)
        {
            var hocPhan = await _context.HocPhans.FindAsync(maHP);
            if (hocPhan == null || hocPhan.SoLuongDuKien <= 0)
            {
                TempData["ErrorMessage"] = "Học phần không tồn tại hoặc đã hết chỗ";
                return RedirectToAction(nameof(Index));
            }

            var maSV = User.Claims.FirstOrDefault(c => c.Type == "MaSV")?.Value;
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("Login", "Account");
            }

            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .FirstOrDefaultAsync(d => d.MaSV == maSV && d.NgayDK.Date == DateTime.Today && !d.DaXacNhan);

            if (dangKy == null)
            {
                dangKy = new DangKy
                {
                    MaSV = maSV,
                    NgayDK = DateTime.Now,
                    DaXacNhan = false
                };
                _context.DangKys.Add(dangKy);
                await _context.SaveChangesAsync();
            }
            else
            {
                var daCoHocPhan = dangKy.ChiTietDangKys.Any(c => c.MaHP == maHP);
                if (daCoHocPhan)
                {
                    TempData["ErrorMessage"] = "Học phần này đã được đăng ký";
                    return RedirectToAction(nameof(GioHang));
                }
            }

            var chiTietDangKy = new ChiTietDangKy
            {
                MaDK = dangKy.MaDK,
                MaHP = maHP
            };

            _context.ChiTietDangKys.Add(chiTietDangKy);
            hocPhan.SoLuongDuKien--;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GioHang));
        }

        public async Task<IActionResult> GioHang()
        {
            var maSV = User.Claims.FirstOrDefault(c => c.Type == "MaSV")?.Value;
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("Login", "Account");
            }

            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaSV == maSV && d.NgayDK.Date == DateTime.Today && !d.DaXacNhan);

            if (dangKy == null)
            {
                dangKy = new DangKy
                {
                    MaSV = maSV,
                    NgayDK = DateTime.Now,
                    DaXacNhan = false,
                    ChiTietDangKys = new List<ChiTietDangKy>()
                };
            }

            return View(dangKy);
        }

        [HttpPost]
        public async Task<IActionResult> LuuDangKy(int maDK)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaDK == maDK);

            if (dangKy == null)
            {
                return NotFound();
            }

            if (dangKy.DaXacNhan)
            {
                TempData["ErrorMessage"] = "Đăng ký học phần này đã được xác nhận trước đó.";
                return RedirectToAction(nameof(GioHang));
            }

            return View("XacNhanDangKy", dangKy);
        }

        [HttpPost]
        public async Task<IActionResult> XacNhanDangKy(int maDK)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaDK == maDK);

            if (dangKy == null)
            {
                return NotFound();
            }

            if (!dangKy.DaXacNhan)
            {
                dangKy.DaXacNhan = true;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đăng ký học phần đã được xác nhận thành công.";
            }
            else
            {
                TempData["ErrorMessage"] = "Đăng ký học phần này đã được xác nhận trước đó.";
            }

            return RedirectToAction(nameof(ThongTinDaLuu), new { id = maDK });
        }

        public async Task<IActionResult> ThongTinDaLuu(int id)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaDK == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            return View(dangKy);
        }

        [HttpPost]
        public async Task<IActionResult> XoaHocPhan(int maDK, string maHP)
        {
            var dangKy = await _context.DangKys.FindAsync(maDK);
            if (dangKy == null || dangKy.DaXacNhan)
            {
                TempData["ErrorMessage"] = "Không thể xóa học phần từ đăng ký đã xác nhận.";
                return RedirectToAction(nameof(GioHang));
            }

            var chiTietDangKy = await _context.ChiTietDangKys
                .FirstOrDefaultAsync(c => c.MaDK == maDK && c.MaHP == maHP);

            if (chiTietDangKy == null)
            {
                return NotFound();
            }

            _context.ChiTietDangKys.Remove(chiTietDangKy);
            var hocPhan = await _context.HocPhans.FindAsync(maHP);
            if (hocPhan != null)
            {
                hocPhan.SoLuongDuKien++;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GioHang));
        }

        [HttpPost]
        public async Task<IActionResult> XoaTatCa(int maDK)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .FirstOrDefaultAsync(d => d.MaDK == maDK);

            if (dangKy == null || dangKy.DaXacNhan)
            {
                TempData["ErrorMessage"] = "Không thể xóa đăng ký đã xác nhận.";
                return RedirectToAction(nameof(GioHang));
            }

            foreach (var chiTiet in dangKy.ChiTietDangKys)
            {
                var hocPhan = await _context.HocPhans.FindAsync(chiTiet.MaHP);
                if (hocPhan != null)
                {
                    hocPhan.SoLuongDuKien++;
                }
            }

            _context.ChiTietDangKys.RemoveRange(dangKy.ChiTietDangKys);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã xóa tất cả các môn đã chọn.";
            return RedirectToAction(nameof(GioHang));
        }
    }
}