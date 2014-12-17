using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TimeZoneHelper.DataClasses;
using TimeZoneHelper.MusicApiConnect;

namespace TimeZoneHelper.UIClasses
{
    /// <summary>
    /// Interaction logic for MusicView.xaml
    /// </summary>
    public partial class MusicView : UserControl, INotifyPropertyChanged
    {

        #region Fields

        private bool _loginVisible = true;
        private bool _registerVisible;
        private bool _mixSetVisible;
        private MusicDataHandler _dataHandler;
        private string _errorMessage;
        private ObservableCollection<SearchType> _searchOptions; 
        #endregion

        #region Properties

        public bool LoginVisible
        {
            get { return _loginVisible; }
            set { _loginVisible = value; OnPropertyChanged(); }
        }

        public bool RegisterVisible
        {
            get { return _registerVisible; }
            set { _registerVisible = value; OnPropertyChanged(); }
        }

        public bool MixSetVisible
        {
            get { return _mixSetVisible; }
            set { _mixSetVisible = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ObservableCollection<SearchType> SearchOptions
        {
            get
            {
                return _searchOptions ??
                       (_searchOptions = new ObservableCollection<SearchType>
                       {
                           new SearchType("Artists", SearchType.SearchOption.Artist),
                           new SearchType("Tag", SearchType.SearchOption.Tag),
                           new SearchType("Keyword", SearchType.SearchOption.Keyword),
                           new SearchType("Similar To Mix", SearchType.SearchOption.SimilarToMix)
                       });
            }
        }

        #endregion

        #region Constructors
        public MusicView()
        {
            InitializeComponent();
            _dataHandler = new MusicDataHandler();
            DataContext = this;
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            var success = false;
            ErrorMessage = "";
            loginButton.IsEnabled = false;
            if (!String.IsNullOrEmpty(UsernameBox.Text)
                && !String.IsNullOrEmpty(PasswordBox.Password))
            {
                    success =
                        await
                            _dataHandler.LoginUser(UsernameBox.Text,
                                PasswordBox.Password);
            }
            if (success)
            {
                LoginVisible = false;
                RegisterVisible = false;
                MixSetVisible = true;
            }
            else
            {
                ErrorMessage = _dataHandler.ErrorMessage;
                loginButton.IsEnabled = true;
            }
            
        }

        private async void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var success = false;
            ErrorMessage = "";
            registerButton.IsEnabled = false;
            if (!String.IsNullOrEmpty(RegUsernameBox.Text) 
                && !(String.IsNullOrEmpty(RegPasswordBox.Password))
                && !(String.IsNullOrEmpty(RegEmailBox.Text)))
            {
                success = await _dataHandler.RegisterNewUser(RegUsernameBox.Text,
                    RegPasswordBox.Password, RegEmailBox.Text);
            }

            if (success)
            {
                MixSetVisible = true;
                LoginVisible = false;
                RegisterVisible = false;
            }
            else
            {
                ErrorMessage = _dataHandler.ErrorMessage;
                registerButton.IsEnabled = true;
            }
        }

        private void SignUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegisterVisible = !RegisterVisible;
            LoginVisible = false;
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
