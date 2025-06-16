using ResultPattern;
using SmartCarModel;
using SmartCarProtocol;
using SmartCarProtocolClient;
using SmartCarViewModel.interfaces;
using SmartCarViewModel.utils;
using System.Text.Json;

namespace SmartCarViewModel
{
    public class CarViewModel : ViewModelBase, ISaveConfiguration, IReadConfiguration, IDisposable
    {
        private readonly string SETTINGS_FOLDER;
        private readonly string SETTINGS_FILE;
        private readonly SmartCarSettingsViewModel _settings;
        private readonly SmartCarControllerViewModel _smartCarController;
        private readonly ClientProtocol _client;

        public CarViewModel(INotifyCommand? notifyCmd = null): base(notifyCmd)
        {
            SETTINGS_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SmartCar");
            SETTINGS_FILE = Path.Combine(SETTINGS_FOLDER, "settings.json");
            _settings = new SmartCarSettingsViewModel(ReadConfiguration(), this, notifyCmd);
            _smartCarController = new SmartCarControllerViewModel(_settings, this, notifyCmd);
            _client = new(_settings.SmartCarCamera, _settings.SmartCarPort);
        }

        public SmartCarSettingsViewModel Settings => _settings;
        public SmartCarControllerViewModel SmartCarController => _smartCarController;

        public async Task<Result> SendConfiguration(ProtocolMessage message)
        {
            if(_client.IsConnected != true)
            {
                if (await _client.ConnectAsync() is Result result && result.IsFailure)
                    return result;
            }
            
            return await _client.SendMessageAsync(message);
        }

        public void Dispose() => _client.Disconnect();
        
        public SmartCarSettingsModel ReadConfiguration()
        {
            if (!Directory.Exists(SETTINGS_FOLDER)||! File.Exists(SETTINGS_FILE))
            {
                Directory.CreateDirectory(SETTINGS_FOLDER);
                SmartCarSettingsModel config = new();
                SaveConfiguration(config);
                return config;
            }

            SmartCarSettingsModel? model = JsonSerializer.Deserialize<SmartCarSettingsModel>(File.ReadAllText(SETTINGS_FILE));
            return model ?? new();
        }

        public void SaveConfiguration(SmartCarSettingsModel settings)
        {
            if (!Directory.Exists(SETTINGS_FOLDER)) 
                Directory.CreateDirectory(SETTINGS_FOLDER);

            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(SETTINGS_FILE, json);
        }
    }
}
