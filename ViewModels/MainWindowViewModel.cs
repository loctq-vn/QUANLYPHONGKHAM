using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QUANLYPHONGKHAM.Models;
using QUANLYPHONGKHAM.Views;

namespace QUANLYPHONGKHAM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime? _ngayKham = DateTime.Today;

        [ObservableProperty]
        private LoaiPhongKham? _selectedLoaiPhong;

        [ObservableProperty]
        private BenhNhan? _selectedBenhNhan;

        [ObservableProperty]
        private ObservableCollection<BenhNhan> _danhSachBenhNhan = [];

        // Danh sách gốc (giả lập dữ liệu, sau thay bằng DB)
        private readonly ObservableCollection<BenhNhan> _allBenhNhan = [];

        // Auto-increment cho MaKhamBenh
        private int _nextMaKhamBenh = 1;

        // Danh sách loại phòng khám (sau thay bằng DB)
        public ObservableCollection<LoaiPhongKham> DanhSachLoaiPhong { get; } =
        [
            new LoaiPhongKham { MaLoaiPhongKham = 1, TenLoaiPhongKham = "Phòng khám Thường", SoLuongToiDa = 40 },
            new LoaiPhongKham { MaLoaiPhongKham = 2, TenLoaiPhongKham = "Phòng khám VIP", SoLuongToiDa = 20 }
        ];

        [RelayCommand]
        private void TimKiem()
        {
            if (NgayKham is null || SelectedLoaiPhong is null)
            {
                MessageBox.Show("Vui lòng chọn ngày khám và loại phòng khám.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var filtered = _allBenhNhan
                .Where(b => b.NgayKham.Date == NgayKham.Value.Date
                         && b.MaLoaiPhongKham == SelectedLoaiPhong.MaLoaiPhongKham)
                .ToList();

            DanhSachBenhNhan = new ObservableCollection<BenhNhan>(filtered);
        }

        [RelayCommand]
        private void ThemBenhNhan()
        {
            if (NgayKham is null || SelectedLoaiPhong is null)
            {
                MessageBox.Show("Vui lòng chọn ngày khám và loại phòng khám trước khi thêm.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra quy định số bệnh nhân tối đa
            int currentCount = _allBenhNhan
                .Count(b => b.NgayKham.Date == NgayKham.Value.Date
                         && b.MaLoaiPhongKham == SelectedLoaiPhong.MaLoaiPhongKham);

            if (currentCount >= SelectedLoaiPhong.SoLuongToiDa)
            {
                MessageBox.Show(
                    $"Đã đạt số bệnh nhân tối đa ({SelectedLoaiPhong.SoLuongToiDa}) cho {SelectedLoaiPhong.TenLoaiPhongKham}.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var vm = new AddKhamBenhViewModel()
            {
                NgayKham = NgayKham.Value,
                SelectedLoaiPhong = SelectedLoaiPhong,
                IsEditMode = false
            };

            var addWindow = new AddKhamBenh { DataContext = vm };
            addWindow.Owner = Application.Current.MainWindow;

            if (addWindow.ShowDialog() == true)
            {
                var newBenhNhan = new BenhNhan
                {
                    MaKhamBenh = _nextMaKhamBenh++,
                    HoTen = vm.HoTen,
                    GioiTinh = vm.GioiTinh,
                    NamSinh = vm.NamSinh,
                    DiaChi = vm.DiaChi,
                    NgayKham = vm.NgayKham,
                    MaLoaiPhongKham = vm.SelectedLoaiPhong!.MaLoaiPhongKham
                };

                _allBenhNhan.Add(newBenhNhan);
                TimKiem();
            }
        }

        [RelayCommand]
        private void SuaBenhNhan()
        {
            if (SelectedBenhNhan is null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân cần sửa.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var loaiPhong = DanhSachLoaiPhong
                .FirstOrDefault(l => l.MaLoaiPhongKham == SelectedBenhNhan.MaLoaiPhongKham);

            var vm = new AddKhamBenhViewModel()
            {
                HoTen = SelectedBenhNhan.HoTen,
                GioiTinh = SelectedBenhNhan.GioiTinh,
                NamSinh = SelectedBenhNhan.NamSinh,
                DiaChi = SelectedBenhNhan.DiaChi,
                NgayKham = SelectedBenhNhan.NgayKham,
                SelectedLoaiPhong = loaiPhong,
                IsEditMode = true
            };

            var editWindow = new AddKhamBenh { DataContext = vm };
            editWindow.Owner = Application.Current.MainWindow;

            if (editWindow.ShowDialog() == true)
            {
                SelectedBenhNhan.HoTen = vm.HoTen;
                SelectedBenhNhan.GioiTinh = vm.GioiTinh;
                SelectedBenhNhan.NamSinh = vm.NamSinh;
                SelectedBenhNhan.DiaChi = vm.DiaChi;
                SelectedBenhNhan.NgayKham = vm.NgayKham;
                SelectedBenhNhan.MaLoaiPhongKham = vm.SelectedLoaiPhong!.MaLoaiPhongKham;

                TimKiem();
            }
        }

        [RelayCommand]
        private void XoaBenhNhan()
        {
            if (SelectedBenhNhan is null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân cần xóa.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa bệnh nhân \"{SelectedBenhNhan.HoTen}\"?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _allBenhNhan.Remove(SelectedBenhNhan);
                TimKiem();
            }
        }
    }
}
