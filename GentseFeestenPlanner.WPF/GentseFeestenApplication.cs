using GentseFeestenPlanner.Domain;
using GentseFeestenPlanner.Domain.DTO;
using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GentseFeestenPlanner.WPF.AddDayPlanWindow;
using System.Windows;

namespace GentseFeestenPlanner.WPF
{
    public class GentseFeestenApplication
    {
        private MainWindow _mainWindow;
        private DomainManager _domainManager;
        private UserDetailWindow _userDetailWindow;
        private EventDetailWindow _eventDetailWindow;
        private AddDayPlanWindow _addDayPlanWindow;
        private Window _lastOpenWindow;

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

            UpdateUserDetailWindow(id);
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

            List<string> DatesWithNoDayPlan = _domainManager.GetDatesWithNoDayplan(_userDetailWindow.UserId);
            _addDayPlanWindow.DailyBudget = _userDetailWindow.DailyBudget ;
            _addDayPlanWindow.DatesWithNoDayPlan = DatesWithNoDayPlan;
            _addDayPlanWindow.DateSelected += _addDayPlanWindow_DateSelected;
            _addDayPlanWindow.EventSelected += _addDayPlanWindow_EventSelected;
        }

        private void _addDayPlanWindow_EventSelected(object? sender, EventDTO e)
        {
            OpenEventDetailWindow();

            _eventDetailWindow.EventTitle = e.Title;
            _eventDetailWindow.EventDescription = e.Description;
            _eventDetailWindow.EventTimeSpan = e.StartTime.ToShortTimeString() + " - " + e.EndTime.ToShortTimeString();
            _eventDetailWindow.EventPrice = e.Price.ToString() + " Euro";
            _eventDetailWindow.EventId = e.EventId;
        }

        private void _addDayPlanWindow_DateSelected(object? sender, string e)
        {
            List<EventDTO> EventsOnDate = _domainManager.GetEventsOnDateAsEventDTO(e);

            _addDayPlanWindow.EventsOnDate = EventsOnDate;
            _addDayPlanWindow.SaveDayPlan += _addDayPlanWindow_SaveDayPlan;
            _addDayPlanWindow.AddEventToDayPlan += _addDayPlanWindow_AddEventToDayPlan;
        }

        private void _addDayPlanWindow_AddEventToDayPlan(object? sender, EventDTO e)
        {
            try 
            {
                _domainManager.AddEventToDayPlan(_userDetailWindow.UserId, e); 
            }catch(InvalidOperationException ex)
            {
              
                throw;
            }
            
            
        }

        private void _addDayPlanWindow_SaveDayPlan(object? sender, EventArgs e)
        {
            string date = _addDayPlanWindow.DropDownDateSelection.SelectedItem.ToString();

            if (e is EventsArgs customArgs)
            {
                List<EventDTO> events = customArgs.Events;

                _domainManager.MakeDayPlan(_userDetailWindow.UserId, date, events);
            }
            else
            {
                MessageBox.Show("Invalid event data provided.");
            }
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
            // Fetch updated details for the user
            UpdateUserDetailWindow(_userDetailWindow.UserId);

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

            _lastOpenWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            _userDetailWindow.Hide();
            _eventDetailWindow = new EventDetailWindow();
            _eventDetailWindow.Closing += _eventDetailWindow_Closing;
            _eventDetailWindow.Show();
        }

        private void _eventDetailWindow_Closing(object? sender, CancelEventArgs e)
        {
            _lastOpenWindow?.Show();
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

        private void UpdateUserDetailWindow(int userId)
        {
            UserDTO user = _domainManager.GetUserById(userId);
            List<string> DayPlanDates = _domainManager.GetUserDayPlanDates(userId);

            _userDetailWindow.UserId = userId;
            _userDetailWindow.FirstName = user.FirstName;
            _userDetailWindow.LastName = user.LastName;
            _userDetailWindow.DailyBudget = user.DailyBudget;
            _userDetailWindow.DayPlans = user.DayPlans;
            _userDetailWindow.FullName = user.FirstName + " " + user.LastName;
            _userDetailWindow.DayPlanDates = DayPlanDates;
        }
    }
}