using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QUANLYPHONGKHAM.Models;

namespace QUANLYPHONGKHAM.ViewModels
{
    public partial class AddKhamBenhViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _hoTen = string.Empty;

        [ObservableProperty]
        private string _gioiTinh = string.Empty;

        [ObservableProperty]
        private int _namSinh;

        [ObservableProperty]
        private string _diaChi = string.Empty;

        [ObservableProperty]
        private DateTime _ngayKham = DateTime.Today;

        [ObservableProperty]
        private LoaiPhongKham? _selectedLoaiPhong;

        [ObservableProperty]
        private bool _isEditMode;

        public string TieuDe => IsEditMode ? "Sửa Thông Tin Bệnh Nhân" : "Thêm Bệnh Nhân Mới";

        public List<string> DanhSachGioiTinh { get; } = ["Nam", "Nữ"];

        public ObservableCollection<LoaiPhongKham> DanhSachLoaiPhong { get; } =
        [
            new LoaiPhongKham { MaLoaiPhongKham = 1, TenLoaiPhongKham = "Phòng khám Thường", SoLuongToiDa = 40 },
            new LoaiPhongKham { MaLoaiPhongKham = 2, TenLoaiPhongKham = "Phòng khám VIP", SoLuongToiDa = 20 }
        ];

        public AddKhamBenhViewModel()
        {
        }

        [RelayCommand]
        private void Luu(Window window)
        {
            // Validate họ tên
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate giới tính
            if (string.IsNullOrEmpty(GioiTinh))
            {
                MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate năm sinh
            int currentYear = DateTime.Now.Year;
            if (NamSinh <= 1900 || NamSinh > currentYear)
            {
                MessageBox.Show($"Năm sinh phải nằm trong khoảng 1900 - {currentYear}.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate địa chỉ
            if (string.IsNullOrWhiteSpace(DiaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate ngày khám
            if (NgayKham == default)
            {
                MessageBox.Show("Vui lòng chọn ngày khám.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate loại phòng khám
            if (SelectedLoaiPhong is null)
            {
                MessageBox.Show("Vui lòng chọn loại phòng khám.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            window.DialogResult = true;
            window.Close();
        }

        [RelayCommand]
        private void Huy(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }
}
