namespace Equilibrium_App
{
    public partial class MainPage : ContentPage
    {
        private HttpClient httpClient = new HttpClient();

        public MainPage()
        {
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
                string jsonContent = $"{{\"content\": \"ping\", \"user_id\": {Settings.dummyUserID}, \"user_name\": \"{Settings.dummyUsername}\", \"chat_id\": {Settings.dummyChatID}}}";
                HttpResponseMessage response = await httpClient.PostAsync($"http://{Settings.dummyIP}:{Settings.dummyPort}/command", new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"));

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