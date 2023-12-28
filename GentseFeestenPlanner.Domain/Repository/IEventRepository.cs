using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Repository
{
    public interface IEventRepository
    {
        List<Event> GetAllEventsByDate(DateTime date);

        List<Event> GetEventsForUserDayPlan(int userId, DateTime dayplanDate);

        List<DateTime> GetDatesWithNoDayplan(int userId);
        Event GetEventById(string eventId);
    }
}
