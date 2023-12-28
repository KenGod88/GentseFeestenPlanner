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
            DateTime dayPlanDate = DateTime.Parse(date);
            User user = _userRepository.GetUserById(userId); // Retrieve the user
            DayPlan dayPlan = new DayPlan(dayPlanDate, user); // Create a new DayPlan instance
            List<DayPlan> existingDayPlans = _userRepository.GetDayPlansForUser(userId, user); // Retrieve all existing day plans for the user

            //SqlTransaction transaction = _userRepository.BeginTransaction();
            try
                {
                    // Add the day plan to the database
                    _userRepository.AddDayPlan(dayPlan);

                    // For each EventDTO, convert it to Event, validate, and add to the day plan
                    foreach (EventDTO eventDto in events)
                    {
                        Event eventToAdd = _eventRepository.GetEventById(eventDto.EventId);
                        // Validation and adding events to the day plan should go here
                        if (dayPlan.CheckEvent(eventToAdd, user, existingDayPlans))
                            {
                            // Once validated, add the event to the DayPlanEvents table
                            _userRepository.AddEventToDayPlan(user.UserId, dayPlanDate, eventToAdd.EventId);
                        }
                    }

                    // If everything is successful, commit the transaction
                    //transaction.Commit();
                }
                catch
                {
                throw;
                    // If there's an error, roll back the transaction
                    //transaction.Rollback();
                     // Rethrow the exception to handle it at a higher level
                }
            
        }
    }
}