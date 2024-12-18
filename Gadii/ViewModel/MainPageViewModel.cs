using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UraniumUI;
using Gadii.Model;
using Gadii.View;

namespace Gadii.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        
        public ObservableCollection<TodoItem> Items { get; protected set; } = new ObservableCollection<TodoItem>();

        public ObservableCollection<TodoItem> SelectedItems { get; set; } = new ObservableCollection<TodoItem>();

        private TodoItem newItem = new();
        public TodoItem NewItem 
        { 
            get => newItem; 
            set 
            { newItem = value;
                OnPropertyChanged(nameof(newItem)) ;
             } 
        }
        
        public ICommand AddNewItemCommand { get; protected set; }

        public ICommand RemoveSelectedItemsCommand { get; protected set; }
        public ICommand LogoutCommand { get;  set; }

        public MainPageViewModel()
        {
            if (Items.Count == 0)
            {
                Items.Add(new TodoItem { Content = "Throw away the rubbish", Type = TodoItem.TodoItemType.Personal });
                Items.Add(new TodoItem { Content = "Attend the meeting today\n11:00AM", Type = TodoItem.TodoItemType.Work });
                Items.Add(new TodoItem { Content = "Prepare presentation for new project", Type = TodoItem.TodoItemType.Work });
                Items.Add(new TodoItem { Content = "Spend time with family", Type = TodoItem.TodoItemType.Family });
                Items.Add(new TodoItem { Content = "Complete the puzzle", Type = TodoItem.TodoItemType.Hobby });
                Items.Add(new TodoItem { Content = "Don't forget to call dad", Type = TodoItem.TodoItemType.Family });
            }

            AddNewItemCommand = new Command(() =>
            {
                Items.Insert(0, NewItem);
                NewItem = new();
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
            catch(Exception e)
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

        private bool isDone;
        public bool IsDone { get => isDone; set => SetProperty(ref isDone, value); }

        public TodoItemType Type { get; set; }

        public static TodoItemType[] AvailableTypes => Enum.GetValues(typeof(TodoItemType)) as TodoItemType[];

        public enum TodoItemType
        {
            Personal,
            Work,
            Hobby,
            Family
        }
    }
}