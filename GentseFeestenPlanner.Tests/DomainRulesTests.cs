using GentseFeestenPlanner.Domain;
using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Tests
{
    public class DomainRulesTests
    {
        private User CreateTestUser(int userId, decimal dailyBudget) => new User(userId,"","", dailyBudget);
        private Event CreateTestEvent(string eventId, DateTime startTime, DateTime endTime, decimal price) => new Event(eventId, "", "", startTime, endTime, price);
        private DayPlan CreateTestDayPlan(DateTime date, User user) => new DayPlan(date, user);

        [Theory]
        [InlineData("E001", "E001")] // Same Event ID
        [InlineData("E001", "E002")] // Different Event ID
        public void CheckIfEventAlreadyPlanned_ThrowsWhenAlreadyPlanned(string firstEventId, string secondEventId)
        {
            // Arrange
            User user = CreateTestUser(1, 100);
            DateTime date = DateTime.Now;
            DayPlan dayPlan = CreateTestDayPlan(date, user);
            Event event1 = CreateTestEvent(firstEventId, date, date.AddHours(2), 20);
            Event event2 = CreateTestEvent(secondEventId, date, date.AddHours(2), 20);
            dayPlan.Events.Add(event1);

            // Act & Assert
            if (firstEventId == secondEventId)
            {
                InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => dayPlan.CheckIfEventAlreadyPlanned(event2, dayPlan));
                Assert.Equal("The same event cannot be planned more than once during the Gentse Feesten.", exception.Message);
            }
            else
            {
                Exception exception = Record.Exception(() => dayPlan.CheckIfEventAlreadyPlanned(event2, dayPlan));
                Assert.Null(exception);
            }
        }

        [Theory]
        [InlineData(1, 3, 2, 4)] // Overlapping
        [InlineData(1, 2, 3, 4)] // Not overlapping
        public void CheckForOverlappingEvents_ThrowsWhenEventsOverlap(int startHour1, int endHour1, int startHour2, int endHour2)
        {
            // Arrange
            User user = CreateTestUser(1, 100);
            DateTime date = DateTime.Now;
            DayPlan dayPlan = CreateTestDayPlan(date, user);
            Event event1 = CreateTestEvent("E001", date.AddHours(startHour1), date.AddHours(endHour1), 20);
            Event event2 = CreateTestEvent("E002", date.AddHours(startHour2), date.AddHours(endHour2), 30);
            dayPlan.Events.Add(event1);

            // Act & Assert
            if (endHour1 > startHour2 && startHour1 < endHour2)
            {
                InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => dayPlan.CheckForOverlappingEvents(event2, dayPlan));
                Assert.Equal("Events in a day plan must not overlap with each other.", exception.Message);
            }
            else
            {
                Exception exception = Record.Exception(() => dayPlan.CheckForOverlappingEvents(event2, dayPlan));
                Assert.Null(exception); // No exception expected if events don't overlap
            }
        }

        [Theory]
        [InlineData(100, 50, 60)] // Exceeding budget
        [InlineData(100, 30, 40)] // Within budget
        public void CheckBudgetConstraints_ThrowsWhenBudgetExceeded(decimal userBudget, decimal price1, decimal price2)
        {
            // Arrange
            User user = CreateTestUser(1, userBudget);
            DateTime date = DateTime.Now;
            DayPlan dayPlan = CreateTestDayPlan(date, user);
            Event event1 = CreateTestEvent("E001", date, date.AddHours(2), price1);
            Event event2 = CreateTestEvent("E002", date, date.AddHours(3), price2);
            dayPlan.Events.Add(event1);

            // Act & Assert
            if (price1 + price2 > userBudget)
            {
                InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => dayPlan.CheckBudgetConstraints(event2, user, dayPlan));
                Assert.Equal("The total cost of all events per day must not exceed the user's available daily budget.", exception.Message);
            }
            else
            {
                Exception exception = Record.Exception(() => dayPlan.CheckBudgetConstraints(event2, user, dayPlan));
                Assert.Null(exception); // No exception expected if within budget
            }
        }

    }
}
