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
            // Assuming the updated repository method now accepts a DateTime parameter to filter events.
            List<Event> events = _eventRepository.GetEventsForUserDayPlan(userId, dayplanDate);

            List<EventDTO> eventDTOs = new List<EventDTO>();

            foreach (Event e in events)
            {
                // Create and add the EventDTO to the list
                eventDTOs.Add(new EventDTO(e.EventId, e.Title, e.Price, e.StartTime, e.EndTime, e.Description));
            }

            return eventDTOs;
        }



    }
}
