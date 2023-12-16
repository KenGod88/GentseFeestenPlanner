using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVDataInitializer
{
    internal class Event
    {

        public string EventId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public decimal Price { get; init; }
    }
}
