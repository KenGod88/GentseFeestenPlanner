using GentseFeestenPlanner.Domain.DTO;
using GentseFeestenPlanner.Domain.Model;
using GentseFeestenPlanner.Domain.Repository;

namespace GentseFeestenPlanner.Domain
{
    public class DomainManager
    {
        private IUserRepository _userRepository;
        private IEventRepository _eventRepository;
        

        public DomainManager(IUserRepository userRepository, IEventRepository eventRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            
        }

        public Dictionary<int, string> GetAllUsers()
        {
            return _userRepository.GetAllUsers().ToDictionary(u => u.UserId, u => u.FirstName + " " + u.LastName);
        }

        public UserDTO GetUserById(int userId)
        {
            User user = _userRepository.GetUserById(userId);

            return new UserDTO(user.UserId, user.FirstName, user.LastName, user.DailyBudget, user.DayPlans);
        }

        public List<string> GetUserDayPlanDates(int userId)
        {
            List<string> UniqueDates = new List<string>();
            List<DateTime> userDayPlanDates = _userRepository.GetUserDayPlanDates(userId);

            userDayPlanDates = userDayPlanDates.OrderBy(d => d).ToList();

            foreach (DateTime date in userDayPlanDates)
            {
                string formattedDate = date.ToString("dd/MM/yyyy");
                UniqueDates.Add(formattedDate);
            }

            return UniqueDates;
        }

        public List<EventDTO> GetEventsForUserDayplan(int userId, DateTime dayplanDate)
        {
            List<Event> events = _eventRepository.GetEventsForUserDayPlan(userId, dayplanDate);

            List<EventDTO> eventDTOs = new List<EventDTO>();

            foreach (Event e in events)
            {
                eventDTOs.Add(new EventDTO(e.EventId, e.Title, e.Price, e.StartTime, e.EndTime, e.Description));
            }

            return eventDTOs;
        }

        public List<string> GetEventsOnDate(string date)
        {
            List<string> UniqueEvents = new List<string>();
            DateTime dateTime = DateTime.Parse(date);
            List<Event> events = _eventRepository.GetAllEventsByDate(dateTime);

            events = events.OrderBy(e => e.StartTime).ToList();

            foreach (Event e in events)
            {
                string formattedEvent = e.ToString();
                UniqueEvents.Add(formattedEvent);
            }

            return UniqueEvents;
        }

        public List<EventDTO> GetEventsOnDateAsEventDTO(string date)
        {
            List<EventDTO> events = new List<EventDTO>();
            DateTime dateTime = DateTime.Parse(date);
            List<Event> eventsToConvert = _eventRepository.GetAllEventsByDate(dateTime);

            eventsToConvert = eventsToConvert.OrderBy(e => e.StartTime).ToList();

            foreach (Event e in eventsToConvert)
            {
                events.Add(new EventDTO(e.EventId, e.Title, e.Price, e.StartTime, e.EndTime, e.Description));
            }

            return events;
        }

        public EventDTO GetEventById(string eventId)
        {
            Event eventToReturn = _eventRepository.GetEventById(eventId);

            return new EventDTO(eventToReturn.EventId, eventToReturn.Title, eventToReturn.Price, eventToReturn.StartTime, eventToReturn.EndTime, eventToReturn.Description);
        }

        public List<string> GetDatesWithNoDayplan(int userId)
        {
            List<string> UniqueDates = new List<string>();
            List<DateTime> userDayPlanDates = _eventRepository.GetDatesWithNoDayplan(userId);

            userDayPlanDates = userDayPlanDates.OrderBy(d => d).ToList();

            foreach (DateTime date in userDayPlanDates)
            {
                string formattedDate = date.ToString("dd/MM/yyyy");
                UniqueDates.Add(formattedDate);
            }

            return UniqueDates;
        }

        public void MakeDayPlan(int userId, string date, List<EventDTO> events)
        {
        }

        public DayPlan GetDayplanForUserOnDate(int userId, DateTime date)
        {
            return _userRepository.GetDayPlanForUserOnDate(userId, date);
        }

        public void AddEventToDayPlan(int userId, EventDTO e)
        {
            DateTime eventDate = DateTime.Parse(e.StartTime.ToShortDateString());
            User user = _userRepository.GetUserById(userId);
            Event eventToCheck = ConvertToEvent(e);
            DayPlan dayPlan = _userRepository.GetDayPlanForUserOnDate(userId, eventDate);
            List<Event> eventsForDayPlan = _eventRepository.GetEventsForUserDayPlan(userId, eventDate);

            if (dayPlan == null)
            {
                dayPlan = new DayPlan(eventDate, user);
            }

            if (dayPlan.CheckEvent(eventToCheck, user, dayPlan, eventsForDayPlan))
            {
                _eventRepository.AddEventToDayPlan(userId, eventToCheck);
                dayPlan.Events.Add(eventToCheck);
            }
        }

        public Event ConvertToEvent(EventDTO eventDTO)
        {
            return new Event(
        eventDTO.EventId,
        eventDTO.Title,
        eventDTO.Description,
        eventDTO.StartTime,
        eventDTO.EndTime,
        eventDTO.Price
             );
        }
    }
}