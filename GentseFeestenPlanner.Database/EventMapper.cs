using GentseFeestenPlanner.Domain.DTO;
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

               
                string query = "SELECT * FROM Events WHERE CAST(StartTime AS DATE) = @date";

                SqlCommand selectEventsCommand = new SqlCommand(query, connection);
                selectEventsCommand.Parameters.AddWithValue("@date", date.Date);  

                SqlDataReader reader = selectEventsCommand.ExecuteReader();

                while (reader.Read())
                {
                    string EventId = reader["EventId"].ToString();
                    string Title = reader["Title"].ToString();
                    string Description = reader["Description"].ToString();
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

        public List<DateTime> GetDatesWithNoDayplan(int userId)
        {
            List<DateTime> datesWithNoDayplan = new List<DateTime>();

            string query = @"
                    SELECT DISTINCT CAST(e.StartTime AS DATE) AS EventDate FROM Events e
                    WHERE CAST(e.StartTime AS DATE) NOT IN (
                        SELECT dp.Date FROM DayPlans dp
                        WHERE dp.UserId = @UserId
                    )";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand selectEventsCommand = new SqlCommand(query, connection);
                selectEventsCommand.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader reader = selectEventsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = (DateTime)reader["EventDate"];
                        datesWithNoDayplan.Add(date);
                    }
                }
            }

            return datesWithNoDayplan;
        }

        public Event GetEventById(string eventId)
        {
            Event eventToReturn = null;

            string query = "SELECT * FROM Events WHERE EventId = @eventId";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand selectEventsCommand = new SqlCommand(query, connection);
                selectEventsCommand.Parameters.AddWithValue("@eventId", eventId);

                using (SqlDataReader reader = selectEventsCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string EventId = (string)reader["EventId"];
                        string Title = (string)reader["Title"];
                        string Description = (string)reader["Description"];
                        DateTime StartTime = (DateTime)reader["StartTime"];
                        DateTime EndTime = (DateTime)reader["EndTime"];
                        decimal Price = (decimal)reader["Price"];

                        eventToReturn = new Event(EventId, Title, Description, StartTime, EndTime, Price);
                    }
                }
            }

            return eventToReturn;
            
        }

        public void AddEventToDayPlan(int userId, Event eventToAdd)
        {

            // Ensure the connection is open
            _connection.Open();

            // Start a transaction
            SqlTransaction transaction = _connection.BeginTransaction();

            try
            {
                // Check if there's an existing DayPlan for the user and the event's date
                string checkDayPlanQuery = @"
            SELECT DayPlanId FROM DayPlans 
            WHERE UserId = @UserId AND Date = CAST(@EventDate AS DATE)";

                SqlCommand checkDayPlanCommand = new SqlCommand(checkDayPlanQuery, _connection, transaction);
                checkDayPlanCommand.Parameters.AddWithValue("@UserId", userId);
                checkDayPlanCommand.Parameters.AddWithValue("@EventDate", eventToAdd.StartTime);

                int? dayPlanId = (int?)checkDayPlanCommand.ExecuteScalar();

                // If there's no DayPlan for that date, create one
                if (!dayPlanId.HasValue)
                {
                    string insertDayPlanQuery = "INSERT INTO DayPlans (UserId, Date) OUTPUT INSERTED.DayPlanId VALUES (@UserId, CAST(@EventDate AS DATE))";

                    SqlCommand insertDayPlanCommand = new SqlCommand(insertDayPlanQuery, _connection, transaction);
                    insertDayPlanCommand.Parameters.AddWithValue("@UserId", userId);
                    insertDayPlanCommand.Parameters.AddWithValue("@EventDate", eventToAdd.StartTime);

                    // Retrieve the new DayPlanId
                    dayPlanId = (int)insertDayPlanCommand.ExecuteScalar();
                }

                // Now that we have a DayPlanId, we can add the event to the DayPlanEvents
                // First, ensure that the event isn't already added to the day plan
                string checkEventInDayPlanQuery = "SELECT COUNT(*) FROM DayPlanEvents WHERE DayPlanId = @DayPlanId AND EventId = @EventId";

                SqlCommand checkEventInDayPlanCommand = new SqlCommand(checkEventInDayPlanQuery, _connection, transaction);
                checkEventInDayPlanCommand.Parameters.AddWithValue("@DayPlanId", dayPlanId.Value);
                checkEventInDayPlanCommand.Parameters.AddWithValue("@EventId", new Guid(eventToAdd.EventId));

                int eventCount = (int)checkEventInDayPlanCommand.ExecuteScalar();

                if (eventCount == 0)
                {
                    string addEventToDayPlanQuery = @"
                INSERT INTO DayPlanEvents (DayPlanId, EventId) 
                VALUES (@DayPlanId, @EventId)";

                    SqlCommand addEventToDayPlanCommand = new SqlCommand(addEventToDayPlanQuery, _connection, transaction);
                    addEventToDayPlanCommand.Parameters.AddWithValue("@DayPlanId", dayPlanId.Value);
                    addEventToDayPlanCommand.Parameters.AddWithValue("@EventId", new Guid(eventToAdd.EventId));

                    addEventToDayPlanCommand.ExecuteNonQuery();
                }

                // Commit the transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback the transaction on error
                transaction.Rollback();
                throw; // Re-throw the exception to handle it in the calling code
            }
            finally
            {
                // Close the connection
                _connection.Close();
            }


        }
    }
}