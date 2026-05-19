using System;
using System.Windows.Controls;
using SystemRezerwacji.Services;

namespace SystemRezerwacji.Views
{
    public partial class CalendarView : UserControl
    {
        private readonly SlotService _slotService;

        public CalendarView()
        {
            InitializeComponent();
            _slotService = new SlotService();
        }

        private async void AppointmentCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = AppointmentCalendar.SelectedDate.Value;
                
                // Pobieramy sloty dla przykładowego ID lekarza = 1
                var slots = await _slotService.GetAvailableSlotsAsync(1, selectedDate);
                
                // Odświeżamy listę w UI
                SlotsList.ItemsSource = slots;
            }
        }
    }
}
