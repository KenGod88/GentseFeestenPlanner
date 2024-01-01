using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Model
{
    public class DayPlan
    {
        public DayPlan( DateTime date, User user)
        {
            
            Date = date;
            User = user;
            Events = new List<Event>();
            
        }

        public int DayPlanId { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public List<Event> Events { get; set; }
        public decimal TotalCost => Events.Sum(e => e.Price);


        public bool CheckEvent(Event eventToAdd, User user, List<DayPlan> existingDayPlans )
        {

            // Assuming 'this.Date' is the date of the current day plan you're trying to add events to
            DayPlan currentDayPlan = existingDayPlans.FirstOrDefault(dp => dp.Date == this.Date);

            // Perform all checks
            CheckIfEventsAvailableOnDate();
            CheckIfEventAlreadyPlanned(eventToAdd, currentDayPlan);
            CheckForOverlappingEvents(eventToAdd, currentDayPlan);
            CheckEventDateAlignment(eventToAdd);
            CheckIfDayPlanAlreadyExistsForDate(currentDayPlan, existingDayPlans);
            CheckBudgetConstraints(eventToAdd, user, currentDayPlan);

            //If all checks pass, add the event to the day plan
            if (currentDayPlan != null)
            {
                currentDayPlan.Events.Add(eventToAdd);
            }
            else
            {
                this.Events.Add(eventToAdd);
            }
            return true;

        }

        private void CheckIfEventsAvailableOnDate()
        {
            // Implement the logic to check if events are available on the date
            // Throw InvalidOperationException if no events are available
        }

        private void CheckIfEventAlreadyPlanned(Event eventToAdd, DayPlan currentDayPlan)
        {
            // Implement the logic to check if the same event has already been planned
            if (currentDayPlan != null && currentDayPlan.Events.Any(e => e.EventId == eventToAdd.EventId))
            {
                throw new InvalidOperationException("The same event cannot be planned more than once during the Gentse Feesten.");
            }
        }

        private void CheckForOverlappingEvents(Event eventToAdd, DayPlan currentDayPlan)
        {
            // Implement the logic to check for overlapping events
            if (currentDayPlan != null && currentDayPlan.Events.Any(e => e.StartTime < eventToAdd.EndTime && e.EndTime > eventToAdd.StartTime))
            {
                throw new InvalidOperationException("Events in a day plan must not overlap with each other.");
            }
        }

        private void CheckEventDateAlignment(Event eventToAdd)
        {
            // Implement the logic to check if event takes place on the same date as the day plan
            if (eventToAdd.StartTime.Date != this.Date)
            {
                throw new InvalidOperationException("An event must take place on the same date as the day plan.");
            }
        }

        private void CheckIfDayPlanAlreadyExistsForDate(DayPlan currentDayPlan, List<DayPlan> existingDayPlans)
        {
            if (currentDayPlan == null && existingDayPlans.Any(dp => dp.Date == this.Date && dp.User.UserId == this.User.UserId))
            {
                throw new InvalidOperationException("A user can only create one day plan per day.");
            }
        }

        private void CheckBudgetConstraints(Event eventToAdd, User user, DayPlan currentDayPlan)
        {
            // Implement the logic to check if the cost exceeds the daily budget
            decimal currentTotalCost = currentDayPlan?.Events.Sum(e => e.Price) ?? 0;
            if (currentTotalCost + eventToAdd.Price > user.DailyBudget)
            {
                throw new InvalidOperationException("The total cost of all events per day must not exceed the user's available daily budget.");
            }
        }



    }
    
}
