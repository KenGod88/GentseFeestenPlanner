namespace GentseFeestenPlanner.Domain.DTO
{
    public class EventDTO
    {
        public EventDTO(string eventId, string name, decimal price, DateTime startDate, DateTime endDate, string description)
        {
            EventId = eventId;
            Title = name;
            Price = price;
            StartTime = startDate;
            EndTime = endDate;
            Description = description;
            EventId = eventId;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
        public string EventId { get; }

        public override string ToString()
        {
            return $"{StartTime.ToShortTimeString()} to {EndTime.ToShortTimeString()} - {Title}  - € {Price}";
        }
    }
}