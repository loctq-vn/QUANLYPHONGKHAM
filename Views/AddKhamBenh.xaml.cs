using System.Windows;
using QUANLYPHONGKHAM.ViewModels;

namespace QUANLYPHONGKHAM.Views
{
    /// <summary>
    /// Interaction logic for AddKhamBenh.xaml
    /// </summary>
    public partial class AddKhamBenh : Window
    {
        public AddKhamBenh()
        {
            InitializeComponent();
            this.DataContext = new AddKhamBenhViewModel();
        }
    }
}
