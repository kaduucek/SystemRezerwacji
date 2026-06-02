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
        public List<Wizyta> Wizyty { get; set; } = new List<Wizyta>();
    }
}
