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

                if (user != null)
                {
                    // Retrieve day plans for the user
                    SqlCommand selectDayPlansCommand = new SqlCommand("SELECT * FROM DayPlans WHERE UserId = @userId", connection);
                    selectDayPlansCommand.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader dayPlansReader = selectDayPlansCommand.ExecuteReader();

                    while (dayPlansReader.Read())
                    {
                        int dayPlanId = (int)dayPlansReader["DayPlanId"];
                        DateTime date = (DateTime)dayPlansReader["Date"];

                        // Create a DayPlan object and add it to the user's list of day plans
                        DayPlan dayPlan = new DayPlan(dayPlanId,date, user);
                        user.DayPlans.Add(dayPlan);
                    }

                    dayPlansReader.Close();
                }
            }

            return user;
        }
    }
}