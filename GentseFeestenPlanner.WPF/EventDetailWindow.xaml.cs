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
    /// Interaction logic for EventDetailWindow.xaml
    /// </summary>
    public partial class EventDetailWindow : Window
    {

        public string EventTitle { get => EventTitleTextBlock.Text; set => EventTitleTextBlock.Text = value ; }
        public string EventDescription { get => EventDescriptionTextBlock.Text; set => EventDescriptionTextBlock.Text = value; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public string EventTimeSpan { get => EventTimeTextBlock.Text; set => EventTimeTextBlock.Text = value; }
        public string EventPrice { get => EventPriceTextBlock.Text; set => EventPriceTextBlock.Text = value; }
        public string EventId { get; set; }

        public EventDetailWindow()
        {
            InitializeComponent();
        }
    }
}
