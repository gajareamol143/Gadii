using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UraniumUI;
using Gadii.Model;
using Gadii.View;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using System.Timers;
using Timer = System.Timers.Timer;
using HeathCare;
using System.Runtime.Intrinsics.X86;
using System;

namespace Gadii.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        int cnt = 0;
        private static Timer _timer;
        private DateTime starttime;
        private DateTime lastBreakTime;
        private Timer idleTimer;
        private DateTime firstActivityTime;
        private DateTime lastActivityTime;
        private DateTime lastActivityTimePosture;
        private DateTime lastKeyboardActivity;
        private int screenTimeInSeconds;
        private MouseHook _mouseHook;
        private GlobalKeyboardHook _globalKeyboardHook;
        private TodoItem postureNotification;
        private float postureAvg;
        public ObservableCollection<TodoItem> Items { get; protected set; } = new ObservableCollection<TodoItem>();

        public ObservableCollection<TodoItem> SelectedItems { get; set; } = new ObservableCollection<TodoItem>();
        private TodoItem newItem = new();
        public TodoItem NewItem
        {
            get => newItem;
            set
            {
                newItem = value;
                OnPropertyChanged(nameof(newItem));
            }
        }
        private string _config;

        public string Config
        {
            get { return _config; }
            set { _config = value; }
        }

        private TimeSpan _pickTime;

        public TimeSpan PickTime
        {
            get { return _pickTime; }
            set { _pickTime = value; }
        }
        private int _intervalTime;

        public int IntervalTime
        {
            get { return _intervalTime; }
            set { _intervalTime = value; }
        }

        public ICommand AddNewItemCommand { get; protected set; }

        public ICommand RemoveSelectedItemsCommand { get; protected set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand GPTCommand { get; set; }

        public MainPageViewModel()
        {
            if (Items.Count == 0)
            {
                Items.Add(new TodoItem { Content = "Bad Posture Detection", Type = TodoItem.TodoItemType.Work, IsInterval = true, IntervalTime = 2, Time = "" });
                //Items.Add(new TodoItem { Content = "Eye Stain Alert", Type = TodoItem.TodoItemType.Work, IsInterval = true, IntervalTime = 15, Time = "" });
                Items.Add(new TodoItem { Content = "Healthy Break Alert", Type = TodoItem.TodoItemType.Personal, IsInterval = true, IntervalTime = 2, Time = "" });
            }

            AddNewItemCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(Config))
                {
                    NewItem.Content = Config;
                    NewItem.Type = TodoItem.TodoItemType.Personal;
                    NewItem.IntervalTime = IntervalTime;
                    NewItem.Time = IntervalTime > 0 ? "" : string.Format("{0:D2}:{1:D2}", (int)PickTime.TotalHours, PickTime.Minutes);

                    Items.Insert(0, NewItem);
                    NewItem = new();
                }

            });


            RemoveSelectedItemsCommand = new Command(() =>
            {
                foreach (var item in SelectedItems)
                {
                    Items.Remove(item);
                }
            });
            Suggestions = new ObservableCollection<string>();
            LogoutCommand = new Command(LogoutCommandReceiver);
            GPTCommand = new Command(GPTCommandReceiver);
            //

            lastActivityTime = DateTime.Now;
            lastActivityTimePosture = DateTime.Now;
            screenTimeInSeconds = 0;
            firstActivityTime = MauiProgram.SystemStartTime;
            _mouseHook = new MouseHook();
            _mouseHook.Hook();
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyPressed += OnMouseMoveAndKeyPressed;
            _globalKeyboardHook.Hook();
            _timer = new Timer(30000);
            _timer.Elapsed += PushNotification;
            _timer.AutoReset = true; // Repeat the timer
            _timer.Enabled = true; // Start the timer
        }

        private async void GPTCommandReceiver(object obj)
        {
            //bool result = await this.ConfirmAsync("Confirmation", "Do you want to Log out your account?");
        }

        private void OnMouseMoveAndKeyPressed(int obj)
        {
            lastKeyboardActivity = DateTime.Now;
        }

        private void PushNotification(object? sender, ElapsedEventArgs e)
        {
            #region Movement
            var newactivity = firstActivityTime.AddMinutes(10);
            lastActivityTime = MouseHook.LastMouseMove > lastKeyboardActivity ? MouseHook.LastMouseMove : lastKeyboardActivity;
            if (newactivity > lastActivityTime && lastActivityTime > firstActivityTime)
            {
                cnt = cnt + 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt >= 2)
            {
                var data = Items.Where(x => x.Content == "Healthy Break Alert").FirstOrDefault();
                if (data != null)
                {
                    var contentdata = new ToastContentBuilder().AddText(data.Content).AddText("Hey Buddy, Take Break for some time !!").GetToastContent();
                    var notificationdata = new ToastNotification(contentdata.GetXml());
                    ToastNotificationManager.CreateToastNotifier().Show(notificationdata);
                }

                cnt = 0;
            }
            #endregion
            #region Posture

            postureNotification = Items.FirstOrDefault(x => x.Content == "Bad Posture Detection");
            if(DateTime.Now>lastActivityTimePosture)
            {
                PostureNotification();
                lastActivityTimePosture = lastActivityTimePosture.AddMinutes(2);
            }
            
            //_timer = new Timer(postureNotification.IntervalTime * 60 * 1000);
            //_timer.Elapsed += PostureNotification;
            //_timer.AutoReset = true; // Repeat the timer
            //_timer.Enabled = true; // Start the timer



            #endregion


        }

        private void PostureNotification()
        {
            string filePath = "F:\\TEAM\\Output.txt";

            // Read all lines and store them in an array
            string[] lines = File.ReadAllLines(filePath);
            //List<string> lines = new List<string>();
            //using (StreamReader reader = new StreamReader(filePath))
            //{
            //    string line;
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        lines.Add(line);
            //    }
            //}
            //// Convert List<string> to string[]
            //string[] linesArray = lines.ToArray();

            int count = lines.Count(line => line.Contains("Bad"));
            int Goodcount = lines.Count(line => line.Contains("Good"));


            postureAvg = Goodcount * 100 / lines.Count();

            var content = new ToastContentBuilder().AddText(postureNotification.Content).AddText("Hey Buddy, Good Posture Avrage is " + postureAvg + " %").GetToastContent();
            var notification = new ToastNotification(content.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        private async void LogoutCommandReceiver()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }

        private ObservableCollection<string> suggestions;
        private string _destinationPoint;

        public ObservableCollection<string> Suggestions
        {
            get => suggestions;
            set
            {
                suggestions = value;
                OnPropertyChanged(nameof(suggestions));
            }
        }

        public string DestinationPoint
        {
            get => _destinationPoint;
            set
            {
                _destinationPoint = value;
                OnPropertyChanged(nameof(_destinationPoint));
                FetchSuggestionsAsync(_destinationPoint);
            }
        }
        private string _pickupPoint;

        public string PickupPoint
        {
            get { return _pickupPoint; }
            set
            {
                _pickupPoint = value;
                OnPropertyChanged(nameof(_pickupPoint));
                FetchSuggestionsAsync(_pickupPoint);
            }
        }

        public int UserActivityDetected { get; private set; }

        private void FetchSuggestionsAsync(string query)
        {
            try
            {

                if (string.IsNullOrEmpty(query))
                {
                    Suggestions.Clear();
                    return;
                }

                // Replace with your API endpoint
                var apiUrl = $"https://api.olamaps.io/places/v1/autocomplete?input={query}&api_key=x1ND1TVXk8uoDlvIM59CfBpxvYUl1i7L4j3V2PfM";

                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetStringAsync(apiUrl);
                    var rec = response.GetAwaiter().GetResult();
                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(rec);
                    var results = jsonResponse["predictions"].Select(item => new Suggestion
                    { Description = item["description"].ToString() }).ToList();

                    Suggestions.Clear();
                    foreach (var item in results)
                    {
                        Suggestions.Add(item.Description);
                    }

                }
            }
            catch (Exception e)
            {


            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }

    public class TodoItem : UraniumBindableObject
    {
        public string Content { get; set; }

        private bool isInterval;
        public bool IsInterval { get => isInterval; set => SetProperty(ref isInterval, value); }

        public TodoItemType Type { get; set; }

        public static TodoItemType[] AvailableTypes => Enum.GetValues(typeof(TodoItemType)) as TodoItemType[];

        public enum TodoItemType
        {
            Personal,
            Work,
            Hobby,
            Family
        }
        public string Time { get; set; }
        public int IntervalTime { get; set; }

    }
}