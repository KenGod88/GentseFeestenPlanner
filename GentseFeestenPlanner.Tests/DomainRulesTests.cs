using GentseFeestenPlanner.Domain;
using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Tests
{
    public class DomainRulesTests
    {

        [Theory]
        [InlineData("123", true)]
        [InlineData("456", false)]
        public void CheckIfEventAlreadyPlanned_Throws_WhenEventAlreadyPlanned(string eventIdToAdd, bool shouldThrow)
        {
            // Arrange
            var eventToAdd = new Event { EventId = eventIdToAdd };
            var currentDayPlan = new DayPlan();
            currentDayPlan.Events.Add(new Event { EventId = "123" }); // Add an event with Id "123"

            // Act & Assert
            if (shouldThrow)
            {
                Assert.Throws<InvalidOperationException>(() => currentDayPlan.CheckIfEventAlreadyPlanned(eventToAdd, currentDayPlan));
            }
            else
            {
                currentDayPlan.CheckIfEventAlreadyPlanned(eventToAdd, currentDayPlan); // Should not throw
            }
        }



    }
}
