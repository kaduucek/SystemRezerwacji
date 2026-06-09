using System.Windows.Controls;
using SystemRezerwacji.ViewModels;

namespace SystemRezerwacji.Views
{
    public partial class WizytyView : UserControl
    {
        public WizytyView()
        {
            InitializeComponent();
            DataContext = new WizytyViewModel();
        }
    }
}
