namespace QUANLYPHONGKHAM.Models
{
    public class BenhNhan
    {
        public int MaKhamBenh { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string GioiTinh { get; set; } = string.Empty;
        public int NamSinh { get; set; }
        public string DiaChi { get; set; } = string.Empty;
        public DateTime NgayKham { get; set; }
        public int MaLoaiPhongKham { get; set; }
    }
}