using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using Microsoft.Maui.Media;

namespace Equilibrium_App
{
    public partial class MainPage : ContentPage
    {
        private HttpClient httpClient = new HttpClient();
        private readonly Configuration.AppSettings _settings;
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        public MainPage(IOptions<Configuration.AppSettings> options)
        {
            _settings = options.Value;
            InitializeComponent();
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
                Description = "Noch eine Beschreibung"
            };

            Items.Add(item);
        }

        private async void OnCameraButtonClicked(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();

                    byte[] imageBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                    string base64Image = Convert.ToBase64String(imageBytes);

                    string jsonContent = $"{{\"photo_data\": \"{base64Image}\", \"user_id\": {_settings.DummyUserID}, \"user_name\": \"{_settings.DummyUserName}\", \"chat_id\": {_settings.DummyChatID}}}";
                    HttpResponseMessage response = await httpClient.PostAsync($"http://{_settings.DummyIP}:{_settings.DummyPort}/photo", new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"));

                    Item item = new Item
                    {
                        Title = "Foto Eintrag",
                        Image = ImageSource.FromStream(() => new MemoryStream(imageBytes)),
                        Description = "Ein Eintrag mit einem Foto"
                    };

                    if (response.IsSuccessStatusCode)
                    {
                        Items.Add(item);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        await DisplayAlertAsync("Error", "Photo upload failed with status code: " + response.StatusCode, "OK");
                    }
                }
            }
        }
    }
}