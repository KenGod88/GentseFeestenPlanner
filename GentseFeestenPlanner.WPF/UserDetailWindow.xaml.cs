using GentseFeestenPlanner.Domain;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GentseFeestenPlanner.WPF
{
    /// <summary>
    /// Interaction logic for UserDetailWindow.xaml
    /// </summary>
    public partial class UserDetailWindow : Window
    {
        //Event properties
        private List<EventDTO> _events;
        public List<EventDTO> Events { get => _events ; set => DayPlanDetailsListBox.ItemsSource = value; }
        public event EventHandler<DateTime> DayPlanSelected;
        public event EventHandler<EventDTO> EventSelected;

        //User properties
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => UserNameTitle.Text; set => UserNameTitle.Text = value; }
        public decimal DailyBudget { get; set; }

        //DayPlan properties
        public List<DayPlan> DayPlans { get; set; }
        private List<string> _DayPlanDates;  
        public List<string> DayPlanDates { get => _DayPlanDates ; set => ExistingPlansListBox.ItemsSource = value; }

        public UserDetailWindow()
        {
            InitializeComponent();


        }

        private void ExistingPlansListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
    }
}