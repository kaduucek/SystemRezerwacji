using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemRezerwacji.Models
{
    public enum WizytaStatus
    {
        Planned,
        Cancelled,
        Completed
    }

    public class Wizyta
    {
        public int IdWizyty { get; set; }

        public int IdPacjenta { get; set; }
        public int IdLekarza { get; set; }

        public DateTime DataGodzinaRozpoczecia { get; set; }
        public int CzasTrwani { get; set; } = 30;

        public WizytaStatus Status { get; set; } = WizytaStatus.Planned;

    }
}

