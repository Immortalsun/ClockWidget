using System;
using System.Collections.ObjectModel;
using System.Windows;
namespace TimeZoneHelper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddDialog : Window
    {
        ObservableCollection<String> TimeZones
            = new ObservableCollection<string>();
        public string Name { get; private set; }
        public string TimeZoneString { get; private set; }

        public AddDialog()
        {
            var collect = TimeZoneInfo.GetSystemTimeZones();
            foreach (var timeZoneInfo in collect)
            {
                TimeZones.Add(timeZoneInfo.Id);
            }
            InitializeComponent();
            TimeZoneComboBox.ItemsSource = TimeZones;
            TimeZoneComboBox.SelectedItem = TimeZoneComboBox.Items[0];
        }

        private void ButtonBase_OkClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NameBox.Text))
            {
                Name = NameBox.Text;
                TimeZoneString = TimeZoneComboBox.SelectedItem as String;
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }

        }

        private void ButtonBase_CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
