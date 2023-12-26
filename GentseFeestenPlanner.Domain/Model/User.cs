using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Model
{
    public class User
    {
        public User(int userId, string firstName, string lastName, decimal dailyBudget)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            DailyBudget = dailyBudget;
            DayPlans = new List<DayPlan>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<DayPlan> DayPlans { get; set; }
        public decimal DailyBudget { get; set; }


       

    }


}
