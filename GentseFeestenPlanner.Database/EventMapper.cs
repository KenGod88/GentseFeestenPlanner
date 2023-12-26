using GentseFeestenPlanner.Domain.Model;
using GentseFeestenPlanner.Domain.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    

                    events.Add(new Event(EventId, Title, Description,StartTime, EndTime, Price));
                }
            }

            return events;
        }

        public List<DateTime> GetUniqueDates()
        {
            List<DateTime> dates = new List<DateTime>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand selectDatesCommand = new SqlCommand("SELECT DISTINCT CONVERT(DATE, StartTime) AS EventDate FROM Events", connection);

                SqlDataReader reader = selectDatesCommand.ExecuteReader();

                while (reader.Read())
                {
                    DateTime date = (DateTime)reader["EventDate"];

                    dates.Add(date);
                }
            }

            return dates;
        }
    }
}
