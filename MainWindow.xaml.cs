using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TimeZoneHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Task timeUpdateTask;
        private CancellationTokenSource cancelSource;
        public ObservableCollection<WorldClock> Clocks { get; set; }
        private bool isStartingUp = true;
        private bool loaded;
        private ClockSerializer serializer;
        private int red;
        private int green;
        private int blue;
        private int alpha;
        private Color setColor;
        private bool isDataRave;
        private bool isAlphaCycle;
        private bool isColorCycle;
        private bool backgroundSlidersVisible;
        private MediaPlayer player;
        List<string> availableSongs = new List<string>();


        public MainWindow()
        {
            this.DataContext = this;
            Clocks = new ObservableCollection<WorldClock>();
            player = new MediaPlayer();
            player.Open(new Uri(@"Music/What Is Love.mp3"
                ,UriKind.Relative));
            serializer = new ClockSerializer();
            Load(serializer.Deserialize());
            LoadSongs();
            InitializeComponent();
            ClocksControl.ItemsSource = Clocks;
            ContextMenu menu = new ContextMenu();
            MenuItem close = new MenuItem();
            close.Header = "Close";
            close.Click += CloseButton_OnClick;
            menu.Items.Add(close);
            ContextMenu = menu;

            if (!loaded)
            {
                red = 0;
                green = 0;
                blue = 0;
                alpha = 0;
                SetupClocks();
                StartClocks();
            }
        }


        private void Load(ClockList savedClocks)
        {
            if (savedClocks != null)
            {
                foreach (var savedClock in savedClocks.Clocks)
                {
                    Clocks.Add(savedClock);
                }

                if (Clocks.Any())
                {
                    Alpha = savedClocks.Alpha;
                    Red = savedClocks.Red;
                    Green = savedClocks.Green;
                    Blue = savedClocks.Blue;
                    loaded = true;
                    isStartingUp = false;
                    SetupClocks();
                    StartClocks();
                }
            }
        }

        private void SetupClocks()
        {
            cancelSource = new CancellationTokenSource();
            timeUpdateTask = new Task(UpdateClocks);
            if (isStartingUp)
                Clocks.Add(new WorldClock("Greensboro", TimeZoneInfo.Local.Id));
        }

        private  void LoadSongs()
        {
            String[] filenames = System.IO.Directory.GetFiles(@"Music");
            
            for (int i = 0; i < filenames.Count(); i++)
            {
                if (filenames[i].EndsWith(".mp3"))
                {
                    availableSongs.Add(filenames[i]);
                }
            }

        }

        private void StartClocks()
        {
            timeUpdateTask.Start();
        }

        private void UpdateClocks()
        {
            while (!cancelSource.Token.IsCancellationRequested)
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    foreach (var clock in Clocks)
                    {
                        clock.Update();
                    }
                }));

                GC.Collect();
                Thread.Sleep(5000);
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            cancelSource.Cancel();
            serializer.SetClocks(Clocks);
            serializer.SetColors(Alpha, Red, Green, Blue);
            serializer.Serialize();
            Close();
        }

        private void MainWindow_OnPreviewMouseLeftButtonDown(
            object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void AddNew_OnClick(object sender, RoutedEventArgs e)
        {
            cancelSource.Cancel();
            isStartingUp = false;
            var addWindow = new AddDialog();
            addWindow.Owner = this;
            var result = addWindow.ShowDialog();
            if ((bool)result)
            {
                var cityName = addWindow.Name;
                var timeZoneString = addWindow.TimeZoneString;

                var clock = new WorldClock(cityName, timeZoneString);
                clock.Update();
                Clocks.Add(clock);
            }
            SetupClocks();
            StartClocks();
        }

        private void ClocksControl_OnPreviewMouseLeftButtonDown(
            object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void RemoveButtonPreviewLeftMouseButton(
            object sender, MouseButtonEventArgs e)
        {
            cancelSource.Cancel();
            var button = sender as Button;
            var clockName = button.Tag.ToString();

            var worldClockItem = Clocks.FirstOrDefault
                (item => item.LocationName.Equals(clockName));

            if (worldClockItem != null)
            {
                Clocks.Remove(worldClockItem);
            }
            SetupClocks();
            StartClocks();
        }



        public SolidColorBrush SetColor
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(Convert.ToByte(Alpha),
                  Convert.ToByte(Red), Convert.ToByte(Green), Convert.ToByte(Blue)));
            }
        }

        public bool IsAlphaCycle
        {
            get { return isAlphaCycle; }
            set
            {
                isAlphaCycle = value;
                if (value)
                {
                    Task.Factory.StartNew(StartAlphaCycle);
                }
            }
        }

        public bool IsColorCycle
        {
            get { return isColorCycle; }
            set
            {
                isColorCycle = value;
                if (value)
                {
                    Task.Factory.StartNew(CycleRed);
                    Task.Factory.StartNew(CycleGreen);
                    Task.Factory.StartNew(CycleBlue);
                }
            }
        }

        public bool IsDataRave
        {
            get { return isDataRave; }
            set
            {
                isDataRave = value;
                if (value)
                {
                    Task.Factory.StartNew(StartDataRave);
                    player.Play();
                }
                else
                {
                    player.Pause();
                }
            }
        }

        public int Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                OnPropertyChanged();
                OnPropertyChanged("SetColor");
            }
        }

        public int Red
        {
            get { return red; }
            set
            {
                red = value;
                OnPropertyChanged();
                OnPropertyChanged("SetColor");
            }
        }

        public int Green
        {
            get { return green; }
            set
            {
                green = value;
                OnPropertyChanged();
                OnPropertyChanged("SetColor");
            }
        }

        public int Blue
        {
            get { return blue; }
            set
            {
                blue = value;
                OnPropertyChanged();
                OnPropertyChanged("SetColor");
            }
        }

        public bool BackgroundSlidersVisible
        {
            get
            {
                return backgroundSlidersVisible;
            }
            set
            {
                backgroundSlidersVisible = value;
                OnPropertyChanged("BackgroundSlidersVisible");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowSliders_OnClick(object sender, RoutedEventArgs e)
        {
            BackgroundSlidersVisible = !BackgroundSlidersVisible;
        }

        private void StartDataRave()
        {
            Random r = new Random();
            Alpha = 255;

            while (IsDataRave)
            {
                int slider = r.Next(3);

                switch (slider)
                {
                    //r
                    case 0:
                        Red = r.Next(255);
                        break;
                    //g
                    case 1:
                        Green = r.Next(255);
                        break;
                    //b
                    case 2:
                        Blue = r.Next(255);
                        break;
                }
            }
        }

        private void StartAlphaCycle()
        {
            var increment = true;
            var decrement = false;
            while (IsAlphaCycle)
            {
                while (increment && IsAlphaCycle)
                {
                    Alpha = (Alpha + 1)%256;
                    Thread.Sleep(100);
                    if (Alpha == 255)
                    {
                        increment = false;
                        decrement = true;
                    }
                }
                while (decrement && IsAlphaCycle)
                {
                    Alpha -= 1;
                    Thread.Sleep(100);
                    if (Alpha == 0)
                    {
                        decrement = false;
                        increment = true;
                    }
                }


            }
        }


        private void CycleRed()
        {
            var increment = true;
            var decrement = false;
            while (IsColorCycle)
            {
                while (increment && isColorCycle)
                {
                    Red = (Red + 1) % 256;
                    Thread.Sleep(100);
                    if (Red == 255)
                    {
                        increment = false;
                        decrement = true;
                    }
                }
                while (decrement && IsColorCycle)
                {
                    Red -= 1;
                    Thread.Sleep(100);
                    if (Red == 0)
                    {
                        decrement = false;
                        increment = true;
                    }
                }
            }
        }

        private void CycleGreen()
        {
            var increment = true;
            var decrement = false;
            while (IsColorCycle)
            {
                while (increment && IsColorCycle)
                {
                    Green = (Green + 1) % 256;
                    Thread.Sleep(100);
                    if (Green == 255)
                    {
                        increment = false;
                        decrement = true;
                    }
                }
                while (decrement && IsColorCycle)
                {
                    Green -= 1;
                    Thread.Sleep(100);
                    if (Green == 0)
                    {
                        decrement = false;
                        increment = true;
                    }
                }
            }
        }

        private void CycleBlue()
        {
            var increment = true;
            var decrement = false;
            while (IsColorCycle)
            {
                while (increment && IsColorCycle)
                {
                    Blue = (Blue + 1) % 256;
                    Thread.Sleep(100);
                    if (Blue == 255)
                    {
                        increment = false;
                        decrement = true;
                    }
                }
                while (decrement && IsColorCycle)
                {
                    Blue -= 1;
                    Thread.Sleep(100);
                    if (Blue == 0)
                    {
                        decrement = false;
                        increment = true;
                    }
                }
            }
        }

        private void ShuffleSong_OnClick(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            int nextSong = rng.Next(0, availableSongs.Count() - 1);
            var prepUri = @"Music/" + Path.GetFileName(availableSongs[nextSong]);
            player.Open(new Uri(prepUri
                , UriKind.Relative));

            if (isDataRave)
            {
                player.Play();
            }
        }
    }
}