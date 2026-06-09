using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemRezerwacji.Models;

namespace SystemRezerwacji.Services
{
    public class SlotService
    {
        private readonly PrzychodniaContext _context;

        // Produkcja: w³asny kontekst (LocalDB)
        public SlotService()
        {
            _context = new PrzychodniaContext();
        }

        // Testy: wstrzykniêty kontekst (np. In-Memory)
        public SlotService(PrzychodniaContext context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var zajeteTerminy = await _context.Wizyty
                .AsNoTracking()
                .Where(w => w.IdLekarza == doctorId
                            && w.DataGodzinaRozpoczecia.Date == date.Date
                            && w.Status == WizytaStatus.Planned)
                .Select(w => w.DataGodzinaRozpoczecia)
                .ToListAsync();

            var sloty = new List<DateTime>();

            // Godziny pracy przychodni 7:00-20:00 (zgodnie z README), sloty co 30 min
            DateTime start = date.Date.AddHours(7);
            DateTime koniec = date.Date.AddHours(20);
            TimeSpan krok = TimeSpan.FromMinutes(30);

            for (DateTime slot = start; slot < koniec; slot = slot.Add(krok))
            {
                // pomijamy terminy zajête oraz godziny, które ju¿ minê³y
                if (!zajeteTerminy.Contains(slot) && slot > DateTime.Now)
                    sloty.Add(slot);
            }

            return sloty;
        }
    }
}
