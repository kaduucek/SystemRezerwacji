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

        public SlotService()
        {
        }

        public SlotService(PrzychodniaContext context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var availableSlots = new List<DateTime>();

           
            var zajeteTerminy = await _context.Wizyty
                .AsNoTracking()
                .Where(w => w.IdLekarza == doctorId
                && w.DataGodzinaRozpoczecia.Date == date.Date
                && w.Status == WizytaStatus.Planned).Select(w => w.DataGodzinaRozpoczecia)
                .ToListAsync();

            DateTime startTime = date.Date.AddHours(8);
            DateTime endTime = date.Date.AddHours(16);
            TimeSpan slotDuration = TimeSpan.FromMinutes(30);

            DateTime currentSlot = startTime;
            while (currentSlot < endTime)
            {
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
