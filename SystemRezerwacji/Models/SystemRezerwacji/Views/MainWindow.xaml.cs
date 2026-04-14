using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SystemRezerwacji.Models;
using SystemRezerwacji.Services;

namespace SystemRezerwacji.Views
{
    public partial class MainWindow : Window
    {
        private AppointmentService appointmentService = new AppointmentService();

        public MainWindow()
        {
            InitializeComponent();
            AppointmentsGrid.ItemsSource = appointmentService.Appointments;
            DatePicker.SelectedDate = DateTime.Today;
            UpdateTitle();
        }

        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PatientNameBox.Text))
            {
                MessageBox.Show("Podaj imię pacjenta");
                return;
            }

            if (DatePicker.SelectedDate == null || DatePicker.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Proszę wybrać poprawną datę (dzisiejszą lub przyszłą).");
                return;
            }
            if (string.IsNullOrWhiteSpace(DoctorNameBox.Text))
            {
                MessageBox.Show("Proszę wpisać imię i nazwisko lekarza.");
                return;
            }

            var appointment = new Appointment
            {
                PatientName = PatientNameBox.Text,
                DoctorName = DoctorNameBox.Text,
                AppointmentDate = DatePicker.SelectedDate.Value,
                Notes = NotesBox.Text
            };

            appointmentService.Appointments.Add(appointment);
            AppointmentsGrid.Items.Refresh();
            ClearForm();
            UpdateTitle();
            MessageBox.Show("Wizyta została dodana");
        }

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentsGrid.SelectedItem is Appointment selected)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Czy na pewno chcesz usunąć wizytę pacjenta {selected.PatientName}?", 
                    "Potwierdzenie usunięcia", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    appointmentService.Appointments.Remove(selected);
                    AppointmentsGrid.Items.Refresh();
                    UpdateTitle();
                }
            }
        }

        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            PatientNameBox.Text = "";
            DoctorNameBox.Text = "";
            NotesBox.Text = "";
            DatePicker.SelectedDate = null;
        }

        private void UpdateTitle()
        {
            this.Title = $"System Rezerwacji - Liczba wizyt: {appointmentService.Appointments.Count}";
        }
    }
}
