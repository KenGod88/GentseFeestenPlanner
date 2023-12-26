using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Model
{
    public class DayPlan
    {

        public int DayPlanId { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public List<Event> Events { get; set; }
        public decimal TotalCost => Events.Sum(e => e.Price);

        public void AddEvent(Event newEvent)
        {
            // Business rule: Cannot add events that overlap with existing events
            if (Events.Any(existingEvent =>
                (newEvent.StartTime >= existingEvent.StartTime && newEvent.StartTime <= existingEvent.EndTime) ||
                (newEvent.EndTime >= existingEvent.StartTime && newEvent.EndTime <= existingEvent.EndTime)))
            {
                throw new InvalidOperationException("Cannot add overlapping events.");
            }

            // Business rule: Cannot exceed the available budget
            decimal totalCostAfterAddingEvent = TotalCost + newEvent.Price;
            if (totalCostAfterAddingEvent > User.DailyBudget)
            {
                throw new InvalidOperationException("Adding this event exceeds the daily budget.");
            }

            // Business rule: An event can only be added once to a day plan
            if (Events.Any(existingEvent => existingEvent.EventId == newEvent.EventId))
            {
                throw new InvalidOperationException("This event is already added to the day plan.");
            }

            // Business rule: An event must occur on the same date as the day plan
            if (newEvent.StartTime.Date != Date.Date)
            {
                throw new InvalidOperationException("Event date does not match the day plan date.");
            }

            Events.Add(newEvent);
        }
    }
    
}
