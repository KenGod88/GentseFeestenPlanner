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
            // DomainRule 1: Check if events are available on this date
            if (!Events.Any())
            {
                throw new InvalidOperationException("A day plan can only be created on a day when events are taking place.");
            }

            // DomainRule 2: Check if the same event has already been planned
            if (existingDayPlans.SelectMany(dp => dp.Events).Any(e => e.EventId == eventToAdd.EventId))
            {
                throw new InvalidOperationException("The same event cannot be planned more than once during the Gentse Feesten.");
            }

            // domainRule 3: Check for overlapping events
            if (Events.Any(e => e.StartTime < eventToAdd.EndTime && e.EndTime > eventToAdd.StartTime))
            {
                throw new InvalidOperationException("Events in a day plan must not overlap with each other.");
            }

            // DomainRule 4: Check if event takes place on the same date
            if (eventToAdd.StartTime.Date != this.Date)
            {
                throw new InvalidOperationException("An event must take place on the same date as the day plan.");
            }

            // DomainRule 5: Check if user already has a day plan for this date
            if (existingDayPlans.Any(dp => dp.Date == this.Date))
            {
                throw new InvalidOperationException("A user can only create one day plan per day.");
            }

            // DomainRule 6: Check if the cost exceeds the daily budget
            if (this.TotalCost + eventToAdd.Price > user.DailyBudget)
            {
                throw new InvalidOperationException("The total cost of all events per day must not exceed the user's available daily budget.");
            }

            //If all checks pass, add the event to the day plan
            Events.Add(eventToAdd);
            return true;

        }
        

        
    }
    
}
