using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.DTO
{
    public class UserDTO
    {
        public UserDTO(int userId, string firstName, string lastName, decimal dailyBudget,  List<DayPlan> dayPlans)
        {
            FirstName = firstName;
            LastName = lastName;
            DailyBudget = dailyBudget;
            UserId = userId;
            DayPlans = dayPlans;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal DailyBudget { get; set; }

        public int UserId { get; set; }

        public List<DayPlan> DayPlans { get; set; }


    }
}
