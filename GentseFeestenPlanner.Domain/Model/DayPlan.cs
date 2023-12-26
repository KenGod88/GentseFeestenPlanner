using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Model
{
    public class DayPlan
    {
        public DayPlan(int dayPlanId, DateTime date, User user)
        {
            DayPlanId = dayPlanId;
            Date = date;
            User = user;
            Events = new List<Event>();
            
        }

        public int DayPlanId { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public List<Event> Events { get; set; }
        

        
    }
    
}
