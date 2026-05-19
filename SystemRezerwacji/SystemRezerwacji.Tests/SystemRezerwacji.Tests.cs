using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using Xunit;
using SystemRezerwacji.Services;

namespace SystemRezerwacji.Tests
{
    public class SlotServiceTests
    {
        [Fact]
        public async Task GetAvailableSlotsAsync_ShouldReturnSlots()
        {
            // Arrange
            var service = new SlotService();
            var testDate = new DateTime(2026, 10, 10);

            // Act
            var result = await service.GetAvailableSlotsAsync(1, testDate);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
