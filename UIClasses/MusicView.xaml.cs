using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        private MusicPlayer _player;
        private string _errorMessage;
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
        #endregion

        #region Constructors
        public MusicView()
        {
            InitializeComponent();
            _player = new MusicPlayer();
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
                            _player.LoginUser(UsernameBox.Text,
                                PasswordBox.Password);
            }
            LoginVisible = !success;
            if (LoginVisible)
            {
                ErrorMessage = _player.ErrorMessage;
                loginButton.IsEnabled = true;
            }
            
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegisterVisible = !RegisterVisible;
        }

        private void MixSetButton_OnClick(object sender, RoutedEventArgs e)
        {
            MixSetVisible = !MixSetVisible;
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
