using GentseFeestenPlanner.Domain;
using GentseFeestenPlanner.Domain.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.WPF
{
    public class GentseFeestenApplication
    {
        private MainWindow _mainWindow;
        private DomainManager _domainManager;
        private UserDetailWindow _userDetailWindow;
        private EventDetailWindow _eventDetailWindow;
        private AddDayPlanWindow _addDayPlanWindow;

        public GentseFeestenApplication(DomainManager domainManager)
        {
            _domainManager = domainManager;

            _mainWindow = new MainWindow();
            _mainWindow.UserSelected += _mainWindow_UserSelected;
            _mainWindow.Show();

            _mainWindow.Users = _domainManager.GetAllUsers();
        }

        private void _mainWindow_UserSelected(object? sender, int id)
        {
            OpenUserDetailWindow();

            UserDTO user = _domainManager.GetUserById(id);
            List<string> DayPlanDates = _domainManager.GetUserDayPlanDates(id);

            _userDetailWindow.UserId = id;
            _userDetailWindow.FirstName = user.FirstName;
            _userDetailWindow.LastName = user.LastName;
            _userDetailWindow.DailyBudget = user.DailyBudget;
            _userDetailWindow.DayPlans = user.DayPlans;
            _userDetailWindow.FullName = user.FirstName + " " + user.LastName;
            _userDetailWindow.DayPlanDates = DayPlanDates;
        }

        private void OpenUserDetailWindow()
        {
            _mainWindow.Hide();

            _userDetailWindow = new UserDetailWindow();
            _userDetailWindow.DayPlanSelected += _userDetailWindow_DayPlanSelected;
            _userDetailWindow.EventSelected += _userDetailWindow_EventSelected;
            _userDetailWindow.AddDayPlan += _userDetailWindow_AddDayPlan;
            _userDetailWindow.Closing += _userDetailWindow_Closing;
            _userDetailWindow.Show();
        }

        private void _userDetailWindow_AddDayPlan(object? sender, EventArgs e)
        {
            OpenAddDayPlanWindow();
        }

        private void OpenAddDayPlanWindow()
        {
            _userDetailWindow.Hide();
            _addDayPlanWindow = new AddDayPlanWindow();
            _addDayPlanWindow.Closing += _addDayPlanWindow_Closing;
            _addDayPlanWindow.Show();
        }

        private void _addDayPlanWindow_Closing(object? sender, CancelEventArgs e)
        {
            _userDetailWindow.Show();
            
        }

        private void _userDetailWindow_EventSelected(object? sender, EventDTO e)
        {
            OpenEventDetailWindow();

            _eventDetailWindow.EventTitle = e.Title;
            _eventDetailWindow.EventDescription = e.Description;
            _eventDetailWindow.EventTimeSpan = e.StartTime.ToShortTimeString() + " - " + e.EndTime.ToShortTimeString();
            _eventDetailWindow.EventPrice = e.Price.ToString() + " Euro";
            _eventDetailWindow.EventId = e.EventId;
        }

        private void OpenEventDetailWindow()
        {
            _userDetailWindow.Hide();
            _eventDetailWindow = new EventDetailWindow();
            _eventDetailWindow.Closing += _eventDetailWindow_Closing;
            _eventDetailWindow.Show();
        }

        private void _eventDetailWindow_Closing(object? sender, CancelEventArgs e)
        {
            _userDetailWindow.Show();
        }

        private void _userDetailWindow_DayPlanSelected(object? sender, DateTime e)
        {
            List<EventDTO> events = _domainManager.GetEventsForUserDayplan(_userDetailWindow.UserId, e);

            _userDetailWindow.Events = events;
        }

        private void _userDetailWindow_Closing(object? sender, CancelEventArgs e)
        {
            _mainWindow.Show();
        }
    }
}