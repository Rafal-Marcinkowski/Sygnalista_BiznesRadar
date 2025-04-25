using Library;
using Library.BiznesRadar;
using Library.Events;
using Library.Interfaces;
using PogromcaBiznesRadar.Services;

namespace PogromcaBiznesRadar.MVVM.ViewModels;

public class MainWindowViewModel : BindableBase
{
    public MainWindowViewModel(IEventAggregator eventAggregator, ICommunicationService communicationService)
    {
        CommunicationManager = new(communicationService, this);
        HtmlProcessingManager = new();

        this.eventAggregator = eventAggregator;
        this.eventAggregator.GetEvent<CommunicationEvent>().Subscribe((communicationPayload) => CommunicationManager.OnMessageReceived(communicationPayload));
        _ = CommunicationManager.StartListeningAsync();
    }

    private readonly IEventAggregator eventAggregator;

    public CommunicationManager CommunicationManager { get; set; }
    public HtmlProcessingManager HtmlProcessingManager { get; set; }

    private int backgroundColor = 1;
    public int BackgroundColor
    {
        get => backgroundColor;
        set => SetProperty(ref backgroundColor, value);
    }

    public async Task BiznesRadarExecute(string companyCode)
    {
        BackgroundColor = 0;

        try
        {
            _ = HtmlProcessingManager.Clear();
            eventAggregator.GetEvent<BiznesRadarExecuteEvent>().Publish();

            await HtmlProcessingManager.DownloadHtmlAsync(companyCode);

            if (!String.IsNullOrEmpty(HtmlProcessingManager.SymbolOId))
            {
                string jsonData = await DownloadHtml.GetChartDataAsync(HtmlProcessingManager.SymbolOId);
                _ = SaveTextToFile.SaveAsync("JsonData", jsonData ?? "brak danych jason");

                await ChartManager.DrawChart(jsonData);
            }

            await HtmlProcessingManager.ProcessHtmlAsync(companyCode);
        }
        catch (Exception ex)
        {
            _ = SaveTextToFile.SaveAsync("ErrorInBiznesRadarExecuteTask", ex.Message);
            await HtmlProcessingManager.Clear();
        }
    }
}
