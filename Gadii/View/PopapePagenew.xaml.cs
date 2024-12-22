using Mopups.Pages;
using Mopups.Services;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace HeathCare.View;

public partial class PopapePagenew : PopupPage
{
    public PopapePagenew()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }
    private void OnSendButtonClicked(object sender, EventArgs e)
    {
        // Get the message from the entry
        var userMessage = MessageEntry.Text;
        if(!string.IsNullOrEmpty(userMessage))
        {
            // Add user's message to the chat
            AddMessageToChat(userMessage, isUser: true);

            // Simulate an AI response
            var aiResponse = GenerateAIResponse(userMessage);
            AddMessageToChat(aiResponse, isUser: false);

            // Clear the message entry
            MessageEntry.Text = string.Empty;
        }
        
    }

    private void AddMessageToChat(string message, bool isUser)
    {
        var chatBubble = new Frame
        {
            BackgroundColor = isUser ? Color.FromArgb("#ADD8E6") : Color.FromArgb("#E0E0E0"), // Different colors for user and AI
            CornerRadius = 15,
            Margin = new Thickness(0, 5)
        };

        var messageLabel = new Label
        {
            Text = message,
            Margin = new Thickness(10),
            HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start
        };

        chatBubble.Content = messageLabel;

        // Add the chat bubble to the messages stack
        MessagesStackLayout.Children.Add(chatBubble);
    }

    private string GenerateAIResponse(string userMessage)
    {
        if (!string.IsNullOrEmpty(userMessage))
        {

            string last = " ? if my quation is related to health then show result other wise show please ask only health related quation. ";
            // Create the client and request
            var client = new RestClient("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=AIzaSyCUGpRMlgQ3qsY6xo03J4vJJ01t21kHBNw");
            var request = new RestRequest("", Method.Post);

            // Adding headers
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddHeader("Host", "generativelanguage.googleapis.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.15.0");
            request.AddHeader("Content-Type", "application/json");

            // Adding JSON body
            var jsonBody = new
            {
                contents = new[]
                {
        new
        {
            parts = new[]
            {
                new { text = userMessage+last }
            }
        }
                }
            };
            request.AddJsonBody(jsonBody);

            // Execute request
            var response = client.Execute(request);

            // Handle response
            if (response.IsSuccessful)
            {
                var jsonResponse = JObject.Parse(response.Content);
                userMessage = jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
            }
            else
            {
                Console.WriteLine("Error: " + response.ErrorMessage);
            }

            return userMessage;
        }
        else
        {
            return "";
        }
    }

}

