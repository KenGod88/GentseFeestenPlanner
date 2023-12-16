using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.DTO
{
    internal class DayPlanDto
    {
        public DateTime Date { get; set; }
        public UserDto User { get; set; }
        public List<EventDto> Events { get; set; }
        public decimal TotalCost { get; set; }
    }
}
