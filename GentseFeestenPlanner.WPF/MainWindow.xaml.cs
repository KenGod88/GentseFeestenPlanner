using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler<int> UserSelected;

        private Dictionary<int, string> _users;

        public Dictionary<int, string> Users
        {
            get => _users;
            set
            {
                _users = value;
                UserListBox.ItemsSource = value.Values;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UserListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserSelected?.Invoke(this, GetSelectedUserId());
        }

        private int GetSelectedUserId()
        {
            if (_users != null && UserListBox.SelectedIndex >= 0 && UserListBox.SelectedIndex < _users.Count)
            {
                return _users.Keys.ToList()[UserListBox.SelectedIndex];
            }
            else
            {
                MessageBox.Show("Please select a user.", "No User Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return -1;
            }
        }
    }
}