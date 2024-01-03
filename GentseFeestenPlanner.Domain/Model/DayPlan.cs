using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Model
{
    public class DayPlan
    {
        public DayPlan(DateTime date, User user)
        {
            Date = date;
            User = user;
            Events = new List<Event>();
        }

        public DayPlan()
        {
        }

        public int DayPlanId { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public List<Event> Events { get; set; }
        public decimal TotalCost => Events.Sum(e => e.Price);

        public bool CheckEvent(Event eventToAdd, User user, DayPlan existingDayPlan, List<Event> events)
        {
            
            DayPlan currentDayPlan = (existingDayPlan != null && existingDayPlan.Date == this.Date && existingDayPlan.User.UserId == user.UserId) ? existingDayPlan : null;
            currentDayPlan.Events = events;

            

            CheckIfEventAlreadyPlanned(eventToAdd, currentDayPlan);
            CheckForOverlappingEvents(eventToAdd, currentDayPlan);
            CheckBudgetConstraints(eventToAdd, user, currentDayPlan);

            
            if (currentDayPlan != null)
            {
                currentDayPlan.Events.Add(eventToAdd);
            }
            else
            {
                Events.Add(eventToAdd);
            }
            return true;
        }

        public void CheckIfEventAlreadyPlanned(Event eventToAdd, DayPlan currentDayPlan)
        {
            
            if (currentDayPlan != null && currentDayPlan.Events.Any(e => e.EventId == eventToAdd.EventId))
            {
                throw new InvalidOperationException("The same event cannot be planned more than once during the Gentse Feesten.");
            }
        }

        public void CheckForOverlappingEvents(Event eventToAdd, DayPlan currentDayPlan)
        {
            
            if (currentDayPlan != null && currentDayPlan.Events.Any(e => e.StartTime < eventToAdd.EndTime && e.EndTime > eventToAdd.StartTime))
            {
                throw new InvalidOperationException("Events in a day plan must not overlap with each other.");
            }
        }

        public void CheckIfDayPlanAlreadyExistsForDate(DayPlan currentDayPlan, DayPlan existingDayPlan)
        {
            if (currentDayPlan == null && existingDayPlan != null && existingDayPlan.Date == this.Date && existingDayPlan.User.UserId == this.User.UserId)
            {
                throw new InvalidOperationException("A user can only create one day plan per day.");
            }
        }

        public void CheckBudgetConstraints(Event eventToAdd, User user, DayPlan currentDayPlan)
        {
            
            decimal currentTotalCost = currentDayPlan?.Events.Sum(e => e.Price) ?? 0;
            if (currentTotalCost + eventToAdd.Price > user.DailyBudget)
            {
                throw new InvalidOperationException("The total cost of all events per day must not exceed the user's available daily budget.");
            }
        }
    }
}