using GentseFeestenPlanner.Domain.DTO;
using GentseFeestenPlanner.Domain.Model;
using System.Windows;
using System.Windows.Input;

namespace GentseFeestenPlanner.WPF
{
    /// <summary>
    /// Interaction logic for UserDetailWindow.xaml
    /// </summary>
    public partial class UserDetailWindow : Window
    {
        //EventHandlers
        public event EventHandler<DateTime> DayPlanSelected;

        public event EventHandler<EventDTO> EventSelected;

        public event EventHandler AddDayPlan;

        //Event properties
        private List<EventDTO> _events;

        public List<EventDTO> Events { get => _events; set => DayPlanDetailsListBox.ItemsSource = value; }

        //User properties
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => UserNameTitle.Text; set => UserNameTitle.Text = value; }
        public decimal DailyBudget { get; set; }

        //DayPlan properties
        public List<DayPlan> DayPlans { get; set; }

        private List<string> _DayPlanDates;
        public List<string> DayPlanDates { get => _DayPlanDates; set => ExistingPlansListBox.ItemsSource = value; }

        public UserDetailWindow()
        {
            InitializeComponent();
        }

        

        private void DayPlanDetailsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DayPlanDetailsListBox.SelectedItem is EventDTO selectedEvent)
            {
                EventSelected?.Invoke(this, selectedEvent);
            }
            else
            {
                MessageBox.Show("Please select a valid event.", "No Event Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddPlanButton_Click(object sender, RoutedEventArgs e)
        {
            AddDayPlan?.Invoke(this, EventArgs.Empty);
        }

        private void ExistingPlansListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ExistingPlansListBox.SelectedItem is string selectedDateString && DateTime.TryParse(selectedDateString, out DateTime selectedDate))
            {
                DayPlanSelected?.Invoke(this, selectedDate);
            }
            else
            {
                MessageBox.Show("Please select a valid day plan date.", "No Dayplan Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}