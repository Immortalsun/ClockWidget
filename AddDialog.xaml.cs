using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using TimeZoneHelper.WeatherApiConnect;

namespace TimeZoneHelper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddDialog : Window
    {
        ObservableCollection<Location> TimeZones
            = new ObservableCollection<Location>();
        LocationSearcher searcher = new LocationSearcher();
        public string Name { get; private set; }
        public string TimeZoneString { get; private set; }
        private Location SelectedLocation;
        public AddDialog()
        {
           //set up requesters, etc
            InitializeComponent();
            TimeZoneResults.ItemsSource = TimeZones;
        }

        private void ButtonBase_OkClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NameBox.Text))
            {
                Name = SelectedLocation.CityName;
                TimeZoneString = SelectedLocation.TimeZone.Id;
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


        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            TimeZones.Clear();
            NameBox.IsEnabled = false;
            SearchButton.IsEnabled = false;
            var cityName = NameBox.Text;
            searcher.GenerateSearchQuery(cityName);

            var json =
                await WeatherRequester.RequestJsonFromWeb(searcher.QueryString);

            if (!String.IsNullOrEmpty(json))
            {
                var list = searcher.ParseLocationJson(json);
                foreach (var location in list)
                {
                    TimeZones.Add(location);
                }
            }
            NameBox.IsEnabled = true;
            SearchButton.IsEnabled = true;
        }

        private void SelectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            var location = (sender as ToggleButton).Tag;
            SelectedLocation = (Location)location;

            foreach (var loc in TimeZones)
            {
                if (!loc.Equals(SelectedLocation))
                {
                    loc.IsChecked = false;
                }
            }
        }
    }
}
