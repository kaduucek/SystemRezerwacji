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
            _slotService = new SlotService(new PrzychodniaContext());
        }

        private async void AppointmentCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = AppointmentCalendar.SelectedDate.Value;
                
                var slots = await _slotService.GetAvailableSlotsAsync(1, selectedDate);
                
                SlotsList.ItemsSource = slots;
            }
        }
    }
}
