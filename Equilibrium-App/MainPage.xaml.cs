using Microsoft.Extensions.Options;

namespace Equilibrium_App
{
    public partial class MainPage : ContentPage
    {
        private HttpClient httpClient = new HttpClient();
        private readonly Configuration.AppSettings _settings;

        public MainPage(IOptions<Configuration.AppSettings> options)
        {
            _settings = options.Value;
            InitializeComponent();
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
    }
}