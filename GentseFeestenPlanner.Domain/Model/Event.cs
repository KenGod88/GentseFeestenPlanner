using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Model
{
    public class Event
    {
        public Event(string eventId, string title, string description, DateTime startTime, DateTime endTime, decimal price)
        {
            EventId = eventId;
            Title = title;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            Price = price;
        }

        public string EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
    }
}
