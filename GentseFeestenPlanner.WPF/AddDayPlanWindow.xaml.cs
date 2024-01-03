using GentseFeestenPlanner.Domain.DTO;
using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GentseFeestenPlanner.WPF
{
    /// <summary>
    /// Interaction logic for AddDayPlanWindow.xaml
    /// </summary>
    public partial class AddDayPlanWindow : Window
    {
        public event EventHandler<string> DateSelected;

        public event EventHandler<EventArgs> SaveDayPlan;

        public event EventHandler<EventDTO> AddEventToDayPlan;

        public event EventHandler<EventDTO> EventSelected;



        public List<string> DatesWithNoDayPlan { get => (List<string>)DropDownDateSelection.ItemsSource; set => DropDownDateSelection.ItemsSource = value; }

        public List<EventDTO> EventsOnDate { get => ShowEventsListBox.ItemsSource as List<EventDTO>; set => ShowEventsListBox.ItemsSource = value; }

        public List<EventDTO> AddedEvents { get => AddedEventsListBox.ItemsSource as List<EventDTO>; set => AddedEventsListBox.ItemsSource = value; }

        public decimal DailyBudget { 
            get 
            { 
                return decimal.Parse(BudgetTextBox.Text);
            } 
            set 
            { 
                BudgetTextBox.Text = value.ToString() + " Euro"; 
            } 
        }

        public AddDayPlanWindow()
        {
            InitializeComponent();
            AddedEvents = new List<EventDTO>();
            AddedEventsListBox.ItemsSource = AddedEvents;
        }

        private void DropDownDateSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddedEvents.Clear();
            AddedEventsListBox.Items.Refresh();

            DateSelected?.Invoke(this, DropDownDateSelection.SelectedItem.ToString());
        }

        private void AddToPlanButton_Click(object sender, RoutedEventArgs e)
        {
            EventDTO selectedEvent = ShowEventsListBox.SelectedItem as EventDTO;
            if (selectedEvent != null)
            {
                try
                {
                    // Attempt to add the event to the day plan.
                    AddEventToDayPlan?.Invoke(this, selectedEvent);

                    // If no exception is thrown, add the event to the AddedEvents list.
                    AddedEvents.Add(selectedEvent);
                    AddedEventsListBox.Items.Refresh();
                }
                catch (InvalidOperationException ex)
                {
                    // Show a message box if the event cannot be added due to overlapping.
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SavePlanButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddedEvents != null && AddedEvents.Any())
            {
                EventsArgs args = new EventsArgs(AddedEvents);

                SaveDayPlan?.Invoke(this, args);
            }
            else
            {
                MessageBox.Show("There are no events to save.", "No Events", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Close();
        }

        public class EventsArgs : EventArgs
        {
            public List<EventDTO> Events { get; set; }

            public EventsArgs(List<EventDTO> events)
            {
                Events = events;
            }
        }

        private void ShowEventsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ShowEventsListBox.SelectedItem is EventDTO selectedEvent)
            {
                EventSelected?.Invoke(this, selectedEvent);
            }
            else
            {
                MessageBox.Show("Please select a valid event.", "No Event Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}