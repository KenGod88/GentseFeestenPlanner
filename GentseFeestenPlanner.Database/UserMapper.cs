using GentseFeestenPlanner.Domain.Model;
using GentseFeestenPlanner.Domain.Repository;
using Microsoft.Data.SqlClient;

namespace GentseFeestenPlanner.Database
{
    internal class UserMapper : IUserRepository
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
    }
}