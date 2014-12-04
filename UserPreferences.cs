namespace TimeZoneHelper
{
    class UserPreferences
    {
        public double WindowTop { get; set; }
        public double WindowLeft { get; set; }

        public UserPreferences()
        {
            //Load the settings
            Load();

            //Move the window at least partially into view
            MoveIntoView();
        }

        private void Load()
        {
            WindowTop = Properties.Settings.Default.WindowTop;
            WindowLeft = Properties.Settings.Default.WindowLeft;
        }

        public void Save()
        {
            Properties.Settings.Default.WindowTop = WindowTop;
            Properties.Settings.Default.WindowLeft = WindowLeft;

            Properties.Settings.Default.Save();          
        }

        public void MoveIntoView()
        {
            if (WindowTop < 0)
            {
                WindowTop = 0;
            }

            if (WindowLeft < 0)
            {
                WindowLeft = 0;
            }
        }
    }
}
