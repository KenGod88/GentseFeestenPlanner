using GentseFeestenPlanner.Domain.Model;
using GentseFeestenPlanner.Domain.Repository;
using Microsoft.Data.SqlClient;

namespace GentseFeestenPlanner.Database
{
    public class UserMapper : IUserRepository
    {
        private const string ConnectionString = "Data Source=Laptop_ken\\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True;Encrypt=False";
        private SqlConnection _connection;

        public UserMapper()
        {
            _connection = new SqlConnection(ConnectionString);
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand selectUsersCommand = new SqlCommand("SELECT * FROM Users", connection);

                SqlDataReader reader = selectUsersCommand.ExecuteReader();

                while (reader.Read())
                {
                    int UserId = (int)reader["UserId"];
                    string FirstName = (string)reader["FirstName"];
                    string LastName = (string)reader["LastName"];
                    decimal DailyBudget = (decimal)reader["DailyBudget"];

                    users.Add(new User(UserId, FirstName, LastName, DailyBudget));
                }
            }

            return users;
        }

        public User GetUserById(int userId)
        {
            User user = GetUserInformation(userId);

            if (user != null)
            {
                user.DayPlans = GetDayPlansForUser(userId, user);
            }

            return user;
        }

        private User GetUserInformation(int userId)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Retrieve user information
                SqlCommand selectUserCommand = new SqlCommand("SELECT * FROM Users WHERE UserId = @userId", connection);
                selectUserCommand.Parameters.AddWithValue("@userId", userId);

                SqlDataReader userReader = selectUserCommand.ExecuteReader();

                while (userReader.Read())
                {
                    int UserId = (int)userReader["UserId"];
                    string FirstName = (string)userReader["FirstName"];
                    string LastName = (string)userReader["LastName"];
                    decimal DailyBudget = (decimal)userReader["DailyBudget"];

                    user = new User(UserId, FirstName, LastName, DailyBudget);
                }

                userReader.Close();
            }

            return user;
        }

        private List<DayPlan> GetDayPlansForUser(int userId, User user)
        {
            List<DayPlan> dayPlans = new List<DayPlan>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Retrieve day plans for the user
                SqlCommand selectDayPlansCommand = new SqlCommand("SELECT * FROM DayPlans WHERE UserId = @userId", connection);
                selectDayPlansCommand.Parameters.AddWithValue("@userId", userId);

                SqlDataReader dayPlansReader = selectDayPlansCommand.ExecuteReader();

                while (dayPlansReader.Read())
                {
                    int dayPlanId = (int)dayPlansReader["DayPlanId"];
                    DateTime date = (DateTime)dayPlansReader["Date"];

                    // Create a DayPlan object and add it to the user's list of day plans
                    DayPlan dayPlan = new DayPlan(dayPlanId, date, user);
                    dayPlans.Add(dayPlan);
                }

                dayPlansReader.Close();
            }

            // Populate events for each day plan
            foreach (var dayPlan in dayPlans)
            {
                dayPlan.Events = GetEventsForDayPlan(dayPlan.DayPlanId);
            }

            return dayPlans;
        }

        private List<Event> GetEventsForDayPlan(int dayPlanId)
        {
            List<Event> events = new List<Event>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Retrieve events for the day plan
                SqlCommand selectEventsCommand = new SqlCommand("SELECT e.* FROM DayPlanEvents dpe INNER JOIN Events e ON dpe.EventId = e.EventId WHERE dpe.DayPlanId = @dayPlanId", connection);
                selectEventsCommand.Parameters.AddWithValue("@dayPlanId", dayPlanId);

                SqlDataReader eventsReader = selectEventsCommand.ExecuteReader();

                while (eventsReader.Read())
                {
                    Guid eventId = (Guid)eventsReader["EventId"];

                    // Convert the GUID to a string if necessary
                    string eventIdString = eventId.ToString();
                    string title = (string)eventsReader["Title"];
                    string description = (string)eventsReader["Description"];
                    DateTime startTime = (DateTime)eventsReader["StartTime"];
                    DateTime endTime = (DateTime)eventsReader["EndTime"];
                    decimal price = (decimal)eventsReader["Price"];

                    // Create an Event object and add it to the day plan
                    Event dayPlanEvent = new Event(eventIdString, title, description, startTime, endTime, price);
                    events.Add(dayPlanEvent);
                }

                eventsReader.Close();
            }

            return events;
        }


        public List<DateTime> GetUserDayPlanDates(int userId)
        {
            List<DateTime> dates = new List<DateTime>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand selectDatesCommand = new SqlCommand("SELECT DISTINCT CONVERT(DATE, Date) AS DayPlanDate FROM DayPlans WHERE UserId = @userId", connection);
                selectDatesCommand.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = selectDatesCommand.ExecuteReader();

                while (reader.Read())
                {
                    DateTime date = (DateTime)reader["DayPlanDate"];

                    dates.Add(date);
                }
            }

            return dates;   
            
        }
    }
}