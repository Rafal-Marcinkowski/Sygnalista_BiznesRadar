using Library;
using Library.Communication;
using Library.Interfaces;
using PogromcaBiznesRadar.MVVM.ViewModels;

namespace PogromcaBiznesRadar.Services;

public class CommunicationManager(ICommunicationService communicationService, MainWindowViewModel viewModel)
{
    public async Task OnMessageReceived(CommunicationPayload payload)
    {
        _ = viewModel.BiznesRadarExecute(payload.Message);
        _ = SaveTextToFile.SaveAsync($"MessageFrom_{payload.Port}", $"{payload.Message}");
    }

    public async Task StartListeningAsync()
    {
        _ = communicationService.ListenAsync();
    }
}
