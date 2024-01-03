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

            try
            {
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
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
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

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    
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
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return user;
        }

        public List<DayPlan> GetDayPlansForUser(int userId, User user)
        {
            List<DayPlan> dayPlans = new List<DayPlan>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    
                    SqlCommand selectDayPlansCommand = new SqlCommand("SELECT * FROM DayPlans WHERE UserId = @userId", connection);
                    selectDayPlansCommand.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader dayPlansReader = selectDayPlansCommand.ExecuteReader();

                    while (dayPlansReader.Read())
                    {
                        int dayPlanId = (int)dayPlansReader["DayPlanId"];
                        DateTime date = (DateTime)dayPlansReader["Date"];

                       
                        DayPlan dayPlan = new DayPlan(date, user);
                        dayPlans.Add(dayPlan);
                    }

                    dayPlansReader.Close();
                }

                
                foreach (var dayPlan in dayPlans)
                {
                    dayPlan.Events = GetEventsForDayPlan(dayPlan.DayPlanId);
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return dayPlans;
        }

        private List<Event> GetEventsForDayPlan(int dayPlanId)
        {
            List<Event> events = new List<Event>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    
                    SqlCommand selectEventsCommand = new SqlCommand("SELECT e.* FROM DayPlanEvents dpe INNER JOIN Events e ON dpe.EventId = e.EventId WHERE dpe.DayPlanId = @dayPlanId", connection);
                    selectEventsCommand.Parameters.AddWithValue("@dayPlanId", dayPlanId);

                    SqlDataReader eventsReader = selectEventsCommand.ExecuteReader();

                    while (eventsReader.Read())
                    {
                        Guid eventId = (Guid)eventsReader["EventId"];

                        string eventIdString = eventId.ToString();
                        string title = (string)eventsReader["Title"];
                        string description = (string)eventsReader["Description"];
                        DateTime startTime = (DateTime)eventsReader["StartTime"];
                        DateTime endTime = (DateTime)eventsReader["EndTime"];
                        decimal price = (decimal)eventsReader["Price"];

                        
                        Event dayPlanEvent = new Event(eventIdString, title, description, startTime, endTime, price);
                        events.Add(dayPlanEvent);
                    }

                    eventsReader.Close();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return events;
        }

        public List<DateTime> GetUserDayPlanDates(int userId)
        {
            List<DateTime> dates = new List<DateTime>();

            try
            {
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
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return dates;
        }

        public DayPlan GetDayPlanForUserOnDate(int userId, DateTime date)
        {
            DayPlan dayPlan = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    SqlCommand selectDayPlanCommand = new SqlCommand("SELECT * FROM DayPlans WHERE UserId = @userId AND Date = @date", connection);
                    selectDayPlanCommand.Parameters.AddWithValue("@userId", userId);
                    selectDayPlanCommand.Parameters.AddWithValue("@date", date.Date);

                    SqlDataReader reader = selectDayPlanCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        int dayPlanId = (int)reader["DayPlanId"];

                        dayPlan = new DayPlan(date, GetUserById(userId));
                        dayPlan.DayPlanId = dayPlanId;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return dayPlan;
        }
    }
}