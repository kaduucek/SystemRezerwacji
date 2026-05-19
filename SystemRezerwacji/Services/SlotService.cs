using System;
using System.Collections.Generic;
using System.Linq; // Wymagane dla LINQ
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Wymagane dla EF Core (AsNoTracking, ToListAsync)
using SystemRezerwacji.Models; // Wymagane do kontekstu

namespace SystemRezerwacji.Services
{
    public class SlotService
    {
        private readonly PrzychodniaContext _context;

        // Wstrzykujemy kontekst bazy danych, żeby móc z niej czytać
        public SlotService(PrzychodniaContext context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var availableSlots = new List<DateTime>();

            // OPTYMALIZACJA LINQ: 
            // 1. AsNoTracking() - tylko odczyt.
            // 2. Select() - pobieramy TYLKO daty wizyt, ignorujemy resztę tabeli!
            var zajeteTerminy = await _context.Wizyty
                .AsNoTracking()
                .Where(w => w.IdLekarza == doctorId && w.DataGodzinaRozpoczecia.Date == date.Date && w.Status == "Zaplanowana")
                .Select(w => w.DataGodzinaRozpoczecia)
                .ToListAsync();

            // Godziny pracy: 08:00 - 16:00, wizyty co 30 minut
            DateTime startTime = date.Date.AddHours(8);
            DateTime endTime = date.Date.AddHours(16);
            TimeSpan slotDuration = TimeSpan.FromMinutes(30);

            DateTime currentSlot = startTime;
            while (currentSlot < endTime)
            {
                // Sprawdzamy, czy wygenerowana godzina NIE znajduje się na liście zajętych terminów w bazie
                if (!zajeteTerminy.Contains(currentSlot))
                {
                    availableSlots.Add(currentSlot);
                }
                currentSlot = currentSlot.Add(slotDuration);
            }

            return availableSlots;
        }
    }
}
