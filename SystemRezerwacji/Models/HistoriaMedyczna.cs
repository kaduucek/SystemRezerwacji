using System;

namespace SystemRezerwacji.Models
{
    public class HistoriaMedyczna
    {
        public int IdWpisu { get; set; }
        public int IdPacjenta { get; set; }
        public int? IdWizyty { get; set; }   // odniesienie do wizyty jest opcjonalne (wg README)
        public DateTime DataWpisu { get; set; }
        public string Opis { get; set; } = "";
    }
}
