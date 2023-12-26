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
    /// Interaction logic for UserDetailWindow.xaml
    /// </summary>
    public partial class UserDetailWindow : Window
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get => UserNameTitle.Text; set => UserNameTitle.Text = value; }
        

        public decimal DailyBudget { get; set; }
        public List<DayPlan> DayPlans { get; set; }

        private List<string> _uniqueDates;  
        public List<string> UniqueDates { get => _uniqueDates ; set => GentseFeestenDates.ItemsSource = value; }

        public UserDetailWindow()
        {
            InitializeComponent();


        }
    }
}