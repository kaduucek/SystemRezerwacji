using System;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SystemRezerwacji.Models;
using SystemRezerwacji.Services;

namespace SystemRezerwacji.Views
{
    public partial class CalendarView : UserControl
    {
        private readonly SlotService _slotService = new();

        public CalendarView()
        {
            InitializeComponent();
            AppointmentCalendar.SelectedDate = DateTime.Today;
            _ = WczytajLekarzy();
        }

        private async System.Threading.Tasks.Task WczytajLekarzy()
        {
            using var ctx = new PrzychodniaContext();
            DoctorBox.ItemsSource = await ctx.Lekarze.AsNoTracking().ToListAsync();
            if (DoctorBox.Items.Count > 0)
                DoctorBox.SelectedIndex = 0;
        }

        private async void AppointmentCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
            => await OdswiezSloty();

        private async void DoctorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => await OdswiezSloty();

        private async System.Threading.Tasks.Task OdswiezSloty()
        {
            if (DoctorBox.SelectedItem is not Lekarz lekarz) return;
            if (!AppointmentCalendar.SelectedDate.HasValue) return;

            var slots = await _slotService.GetAvailableSlotsAsync(
                lekarz.IdLekarza, AppointmentCalendar.SelectedDate.Value);

            SlotsList.ItemsSource = slots;
        }
    }
}
