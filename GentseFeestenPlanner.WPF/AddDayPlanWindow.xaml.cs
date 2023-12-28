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

        public List<string> DatesWithNoDayPlan { get => (List<string>)DropDownDateSelection.ItemsSource; set => DropDownDateSelection.ItemsSource = value; }

        public List<string> EventsOnDate { get => (List<string>)ShowEventsListBox.ItemsSource; set => ShowEventsListBox.ItemsSource = value; }

        public List<string> AddedEvents { get => (List<string>)AddedEventsListBox.ItemsSource; set => AddedEventsListBox.ItemsSource = value; }

        public AddDayPlanWindow()
        {
            InitializeComponent();
            AddedEvents = new List<string>();
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
            if (ShowEventsListBox.SelectedItem != null)
            {
                string selectedEvent = ShowEventsListBox.SelectedItem.ToString();
                AddedEvents.Add(selectedEvent); // Add to the underlying data source

                AddedEventsListBox.Items.Refresh(); // Refresh the ListBox to reflect changes
            }
            else
            {
                MessageBox.Show("Please select an event to add.", "No Event Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SavePlanButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure there are actually events to save
            if (AddedEvents != null && AddedEvents.Any())
            {
                // Create a new instance of EventsArgs with the added events
                EventsArgs args = new EventsArgs(AddedEvents);

                // Invoke the event with the custom EventArgs
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
            public List<string> Events { get; set; }

            public EventsArgs(List<string> events)
            {
                Events = events;
            }
        }


    }
}