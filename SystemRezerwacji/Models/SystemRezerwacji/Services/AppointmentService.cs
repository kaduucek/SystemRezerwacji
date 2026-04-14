using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemRezerwacji.Models;

namespace SystemRezerwacji.Services
{
    public class AppointmentService
    {
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
