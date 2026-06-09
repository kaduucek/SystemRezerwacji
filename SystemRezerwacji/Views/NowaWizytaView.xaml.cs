using System.Windows.Controls;
using SystemRezerwacji.ViewModels;

namespace SystemRezerwacji.Views
{
    public partial class NowaWizytaView : UserControl
    {
        public NowaWizytaView()
        {
            InitializeComponent();
            DataContext = new NowaWizytaViewModel();
        }
    }
}
