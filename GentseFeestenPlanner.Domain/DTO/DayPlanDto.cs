using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.DTO
{
    public class DayPlanDTO
    {
        public DateTime Date { get; set; }
        public UserDTO User { get; set; }
        public List<EventDTO> Events { get; set; }
        public decimal TotalCost { get; set; }
    }
}
