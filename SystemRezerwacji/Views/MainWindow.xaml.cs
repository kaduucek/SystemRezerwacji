using System.Text;
using System.Windows;
using SystemRezerwacji.Models;
using SystemRezerwacji.Services;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemRezerwacji.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppointmentService appointmentService = new AppointmentService();

        public MainWindow()
        {
            InitializeComponent();
            AppointmentsGrid.ItemsSource = appointmentService.Appointments;

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
            MessageBox.Show("Wizyta została dodana");


        }
        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }


        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentsGrid.SelectedItem is Appointment selected)
            {
                appointmentService.Appointments.Remove(selected);
                AppointmentsGrid.Items.Refresh();
            }
        }

        private void ClearForm()
        {
            PatientNameBox.Text = "";
            DoctorNameBox.Text = "";
            NotesBox.Text = "";
            DatePicker.SelectedDate = null;
        }



    }
}




