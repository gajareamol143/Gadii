using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gadii.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gadii.ViewModel
{
    public class TransportViewModel : INotifyPropertyChanged
    {
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

        public TransportViewModel()
        {
            Suggestions = new ObservableCollection<string>();
        }
      
        private void FetchSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Suggestions.Clear();
                return;
            }

            // Replace with your API endpoint
            var apiUrl = $"https://api.olamaps.io/places/v1/autocomplete?input={query}&api_key=x1ND1TVXk8uoDlvIM59CfBpxvYUl1i7L4j3V2PfM";

            using (HttpClient client = new HttpClient())
            {
                var response =  client.GetStringAsync(apiUrl);
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


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}




