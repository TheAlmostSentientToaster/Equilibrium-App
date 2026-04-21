using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace Equilibrium_App
{
    public partial class MainPage : ContentPage
    {
        private HttpClient httpClient = new HttpClient();
        private readonly Configuration.AppSettings _settings;
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>
        {
            new Item
            {
                Title = "Eintrag 1",
                Image = "dotnet_bot.png",
                Description = "Das ist eine Beschreibung"
            },
            new Item
            {
                Title = "Eintrag 2",
                Image = "dotnet_bot.png",
                Description = "Noch eine Beschreibung"
            }
        };

        public MainPage(IOptions<Configuration.AppSettings> options)
        {
            _settings = options.Value;
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"Items count: {Items.Count}");
            BindingContext = this;
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            SendRequest();
        }

        private async void SendRequest()
        {
            try
            {
                string jsonContent = $"{{\"content\": \"ping\", \"user_id\": {_settings.DummyUserID}, \"user_name\": \"{_settings.DummyUserName}\", \"chat_id\": {_settings.DummyChatID}}}";
                HttpResponseMessage response = await httpClient.PostAsync($"http://{_settings.DummyIP}:{_settings.DummyPort}/command", new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlertAsync("Success", "Request sent successfully", "OK");
                }
                else
                {
                    await DisplayAlertAsync("Error", "Request failed with status code: " + response.StatusCode, "OK");
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlertAsync("Error", "Request failed: " + ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", "An error occurred: " + ex.Message, "OK");
            }
        }

        private void OnExpanderAdderClicked(object sender, EventArgs e)
        {
            Item item = new Item
            {
                Title = "Dynamischer Eintrag",
                Image = "dotnet_bot.png",
                Description = "Noch eine Beschreibung"
            };

            Items.Add(item);
        }
    }
}