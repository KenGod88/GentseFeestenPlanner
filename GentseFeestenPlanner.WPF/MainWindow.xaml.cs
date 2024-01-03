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

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            var filteredUsers = _users.Where(kv => kv.Value.ToLower().Contains(searchText)).ToDictionary(kv => kv.Key, kv => kv.Value);

            UserListBox.ItemsSource = filteredUsers.Values;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Seach by name")
            {
                SearchBox.Text = string.Empty;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Seach by name";
            }
        }
    }
}