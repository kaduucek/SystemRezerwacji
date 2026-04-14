using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemRezerwacji.Models
{
    public class Dostepnosc
    {
        public int IdDostepnosci { get; set; }
        public int IdLekarza { get; set; }
        public int DzienTygodnia { get; set; }
        public TimeOnly GodzinaRozpoczecia { get; set; }
        public TimeOnly GodzinaZakonczenia { get; set; }
        public DateTime OkresWaznosciOd {  get; set; }
        public DateTime OkresWaznosciDo {  get; set; }
    }
}
