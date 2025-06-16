using ResultPattern;
using SmartCarModel;
using SmartCarProtocol;

namespace SmartCarViewModel.interfaces
{
    public interface ISaveConfiguration
    {
        void SaveConfiguration(SmartCarSettingsModel settings);
        Task<Result> SendConfiguration(ProtocolMessage message);
    }
}