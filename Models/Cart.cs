using Nhom12_Website_Ban_Cua.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace Nhom12_Website_Ban_Cua.Models
{
    public class Cart
    {
    }

    public class CartItem
    {
        public int maSP { get; set; }
        public string tenSP { get; set; }
        public string tenLoai { get; set; }
        public string anhSP { get; set; }
        public int soLuong { get; set; }
        public double donGia { get; set; }
        public double giaGoc { get; set; } // Thêm giá gốc để hiển thị
        public int giamGia { get; set; } // Thêm % giảm giá để hiển thị

        public double ThanhTien
        {
            get { return soLuong * donGia; }
        }

        DBBanCuaEntities CSDL = new DBBanCuaEntities();

        public CartItem(int maSP)
        {
            SanPham sp = CSDL.SanPhams.Single(n => n.MaSP == maSP);
            if (sp != null)
            {
                this.maSP = sp.MaSP;
                tenSP = sp.TenSP;
                tenLoai = sp.Loai.TenLoai;
                anhSP = sp.AnhLon;

                // Lưu giá gốc
                giaGoc = double.Parse(sp.Gia.ToString());

                // Lưu % giảm giá
                giamGia = sp.GiamGia ?? 0;

                // Tính giá sau khi giảm
                donGia = TinhGiaSauGiam(giaGoc, giamGia);

                soLuong = 1;
            }
        }

        // Phương thức tính giá sau khi giảm
        private double TinhGiaSauGiam(double giaGoc, int giamGia)
        {
            if (giamGia > 0 && giamGia <= 100)
            {
                return giaGoc * (100 - giamGia) / 100.0;
            }
            return giaGoc;
        }
    }

    public class GioHang
    {
        public List<CartItem> lst;

        public GioHang()
        {
            lst = new List<CartItem>();
        }

        public GioHang(List<CartItem> lst)
        {
            this.lst = lst;
        }

        public int SoMatHang()
        {
            return lst.Sum(t => t.soLuong); // Sửa để tính tổng số lượng thay vì số mặt hàng
        }

        public double TongTien()
        {
            return lst.Sum(t => t.ThanhTien);
        }

        public int Them(int maSP)
        {
            try
            {
                // Khởi tạo danh sách nếu null
                if (lst == null)
                {
                    lst = new List<CartItem>();
                }

                // Tìm sản phẩm đã có trong giỏ hàng
                CartItem sp = lst.Find(t => t.maSP == maSP);

                if (sp == null)
                {
                    // Tạo CartItem mới với giá đã giảm
                    CartItem Cua = new CartItem(maSP);
                    if (Cua == null)
                        return -1;
                    lst.Add(Cua);
                }
                else
                {
                    // Nếu sản phẩm đã có, tăng số lượng
                    sp.soLuong++;
                }

                return 1; // Thành công
            }
            catch (Exception)
            {
                return -1; // Lỗi
            }
        }
    }
}