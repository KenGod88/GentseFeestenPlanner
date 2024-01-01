using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Model
{
    public class DayPlanEvent
    {
        public DayPlanEvent(int dayPlanId, string eventId)
        {
            DayPlanId = dayPlanId;
            EventId = eventId;
            
        }

        public int DayPlanId { get; set; }
        public string EventId { get; set; }

       
    }
}
