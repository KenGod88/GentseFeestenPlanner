using CSVDataInitializer;
using Microsoft.Data.SqlClient;

string connectionString = "Data Source=Laptop_ken\\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True;Encrypt=False";

// Clear existing data in tables before loading
ClearDatabaseTables(connectionString);

// Load events from CSV
List<Event> events = LoadEventsFromCsv("gentse-feesten-evenementen-202324.csv");

// Load VIP users from CSV
List<User> Users = LoadUsersFromCsv("vip_users.csv");



// Insert data into the database
InsertDataIntoDatabase(connectionString, events, Users);

Console.WriteLine("Data loaded successfully.");


static void ClearDatabaseTables(string connectionString)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        SqlCommand deleteEventsCommand = new SqlCommand("DELETE FROM Events", connection);
        deleteEventsCommand.ExecuteNonQuery();

        SqlCommand deleteUsersCommand = new SqlCommand("DELETE FROM Users", connection);
        deleteUsersCommand.ExecuteNonQuery();

       
    }
}

static List<Event> LoadEventsFromCsv(string filePath)
{
    List<Event> events = new List<Event>();

    // Assuming your CSV has columns: Title, Description, StartTime, EndTime, Price
    using (StreamReader reader = new StreamReader(filePath))
    {
        reader.ReadLine(); // Skip the first line (header row)

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(';');

            Event newEvent = new Event
            {
                EventId = values[0],
                Title = values[1],
                Description = values[5],
                StartTime = DateTime.Parse(values[2]),
                EndTime = DateTime.Parse(values[3]),
                Price = decimal.Parse(values[4])
            };

            events.Add(newEvent);
        }
    }

    return events;
}

static List<User> LoadUsersFromCsv(string filePath)
{
    List<User> Users = new List<User>();

    // Assuming your CSV has columns: FirstName, LastName, DailyBudget
    using (StreamReader reader = new StreamReader(filePath))
    {
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(';');

            User newUser = new User
            {
                FirstName = values[0],
                LastName = values[1],
                DailyBudget = decimal.Parse(values[2])
            };

            Users.Add(newUser);
        }
    }

    return Users;
}

static void InsertDataIntoDatabase(string connectionString, List<Event> events, List<User> Users)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        // Insert events into the Events table
        foreach (Event e in events)
        {
            SqlCommand insertEventCommand = new SqlCommand(
                "INSERT INTO Events (Title, Description, StartTime, EndTime, Price) " +
                "VALUES (@Title, @Description, @StartTime, @EndTime, @Price)", connection);

            insertEventCommand.Parameters.AddWithValue("@Title", e.Title);
            insertEventCommand.Parameters.AddWithValue("@Description", e.Description);
            insertEventCommand.Parameters.AddWithValue("@StartTime", e.StartTime);
            insertEventCommand.Parameters.AddWithValue("@EndTime", e.EndTime);
            insertEventCommand.Parameters.AddWithValue("@Price", e.Price);

            insertEventCommand.ExecuteNonQuery();
        }

        // Insert VIP users into the VipUsers table
        foreach (User user in Users)
        {
            SqlCommand insertUserCommand = new SqlCommand(
                "INSERT INTO Users (FirstName, LastName, DailyBudget) " +
                "VALUES (@FirstName, @LastName, @DailyBudget)", connection);

            insertUserCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
            insertUserCommand.Parameters.AddWithValue("@LastName", user.LastName);
            insertUserCommand.Parameters.AddWithValue("@DailyBudget", user.DailyBudget);

            insertUserCommand.ExecuteNonQuery();
        }
    }
}