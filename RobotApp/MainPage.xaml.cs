using System;
using System.IO;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Threading;
using System.ComponentModel;
using Windows.UI.Core;
using System.Threading.Tasks;

namespace RobotApp
{
    public sealed partial class MainPage : Page
    {
        private static String defaultHostName = "tak-hp-laptop";
        public static String serverHostName = defaultHostName; // read from config file
        public static bool isRobot = true; // determined by existence of hostName

        public static Stopwatch stopwatch;
        private BackgroundWorker _worker;
        private CoreDispatcher _dispatcher;
        private bool _finish;
        /// <summary>
        /// MainPage initialize all asynchronous functions
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            stopwatch = new Stopwatch();
            stopwatch.Start();

            GetModeAndStartup();

            Loaded += MainPage_Loaded;

            Unloaded += MainPage_Unloaded;
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        }

        private void MainPage_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
     

            _finish = true;

        }

        /// <summary>
        /// Show the current running mode
        /// </summary>
        public void ShowStartupStatus()
        {
            this.CurrentState.Text = "Robot-Kit Sample";
            this.Connection.Text = (isRobot ? ("Robot to " + serverHostName) : "Controller");
        }

        /// <summary>
        /// Switch and store the current running mode in local config file
        /// </summary>
        public async void SwitchRunningMode ()
        {
            try
            {
                if (serverHostName.Length > 0) serverHostName = "";
                else serverHostName = defaultHostName;

                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile configFile = await storageFolder.CreateFileAsync("config.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(configFile, serverHostName);

                isRobot = serverHostName.Length > 0;
                ShowStartupStatus();
           
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SetRunningMode() - " + ex.Message);
            }
        }

        /// <summary>
        /// Read the current running mode (controller host name) from local config file.
        /// Initialize accordingly
        /// </summary>
        public async void GetModeAndStartup()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile configFile = await storageFolder.GetFileAsync("config.txt");
                String fileContent = await FileIO.ReadTextAsync(configFile);

                serverHostName = fileContent;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("GetRunningMode() - configuration does not exist yet.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetRunningMode() - " + ex.Message);
            }

            isRobot = (serverHostName.Length > 0);
            ShowStartupStatus();

            Controllers.XboxJoystickInit();
      
            Controllers.SetRobotDirection(Controllers.CtrlCmds.Stop, (int)Controllers.CtrlSpeeds.Max);
        }



        private async Task WriteLog(string text)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Log.Text = $"{text} | " + Log.Text
                //Log.Text = $"{text}  ";
            });
        }
    }
}
