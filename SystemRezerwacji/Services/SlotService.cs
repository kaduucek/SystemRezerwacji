using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SystemRezerwacji.Services
{
    public class SlotService
    {
        public async Task<List<DateTime>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var availableSlots = new List<DateTime>();

            // Godziny pracy: 08:00 - 16:00, wizyty co 30 minut
            DateTime startTime = date.Date.AddHours(8);
            DateTime endTime = date.Date.AddHours(16);
            TimeSpan slotDuration = TimeSpan.FromMinutes(30);

            DateTime currentSlot = startTime;
            while (currentSlot < endTime)
            {
                // Tutaj dojdzie warunek sprawdzający, czy wizyta nie jest już zajęta w bazie
                availableSlots.Add(currentSlot);
                currentSlot = currentSlot.Add(slotDuration);
            }

            return availableSlots;
        }
    }
}
