using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemRezerwacji.Models
{
    public class HistoriaMedyczna
    {
        public int IdWpisu { get; set; }
        public int IdPacjeta { get; set; }
        public int IdWizyty {  get; set; }
        public DateTime DataWpisu { get; set; }
        public string Opis {  get; set; }
    }
}
