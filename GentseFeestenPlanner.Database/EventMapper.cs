using GentseFeestenPlanner.Domain.Model;
using GentseFeestenPlanner.Domain.Repository;
using Microsoft.Data.SqlClient;

namespace GentseFeestenPlanner.Database
{
    public class EventMapper : IEventRepository
    {
        private const string ConnectionString = "Data Source=Laptop_ken\\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True;Encrypt=False";
        private SqlConnection _connection;

        public EventMapper()
        {
            _connection = new SqlConnection(ConnectionString);
        }

        public List<Event> GetAllEventsByDate(DateTime date)
        {
            List<Event> events = new List<Event>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand selectEventsCommand = new SqlCommand("SELECT * FROM Events WHERE Date = @date", connection);
                selectEventsCommand.Parameters.AddWithValue("@date", date);

                SqlDataReader reader = selectEventsCommand.ExecuteReader();

                while (reader.Read())
                {
                    string EventId = (string)reader["EventId"];
                    string Title = (string)reader["Title"];
                    string Description = (string)reader["Description"];
                    DateTime StartTime = (DateTime)reader["StartTime"];
                    DateTime EndTime = (DateTime)reader["EndTime"];
                    decimal Price = (decimal)reader["Price"];

                    events.Add(new Event(EventId, Title, Description, StartTime, EndTime, Price));
                }
            }

            return events;
        }

        public List<Event> GetEventsForUserDayPlan(int userId, DateTime dayplanDate)
        {
            List<Event> events = new List<Event>();

            string query = @"
                            SELECT e.* FROM Events e
                            INNER JOIN DayPlanEvents dpe ON e.EventId = dpe.EventId
                            INNER JOIN DayPlans dp ON dp.DayPlanId = dpe.DayPlanId
                            WHERE dp.UserId = @UserId AND dp.Date = @DayplanDate";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand selectEventsCommand = new SqlCommand(query, connection);
                selectEventsCommand.Parameters.AddWithValue("@UserId", userId);
                selectEventsCommand.Parameters.AddWithValue("@DayplanDate", dayplanDate.Date); // Ensure only the date part is considered

                using (SqlDataReader reader = selectEventsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid eventId = (Guid)reader["EventId"];
                        string eventIdString = eventId.ToString();
                        string title = (string)reader["Title"];
                        string description = (string)reader["Description"];
                        DateTime startTime = (DateTime)reader["StartTime"];
                        DateTime endTime = (DateTime)reader["EndTime"];
                        decimal price = (decimal)reader["Price"];

                        // Assuming Event constructor matches these parameters
                        events.Add(new Event(eventIdString, title, description, startTime, endTime, price));
                    }
                }
            }

            return events;
        }
    }
}