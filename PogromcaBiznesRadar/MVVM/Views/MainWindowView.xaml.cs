using Library.Events;

namespace PogromcaBiznesRadar.MVVM.Views;

public partial class MainWindowView
{
    private readonly IEventAggregator eventAggregator;

    public MainWindowView(IEventAggregator eventAggregator)
    {
        InitializeComponent();
        this.Left = 1670;
        this.Top = 535;
        this.eventAggregator = eventAggregator;
        this.eventAggregator.GetEvent<BiznesRadarExecuteEvent>().Subscribe(ClearCanvas);
    }

    private void ClearCanvas()
    {
        StockChartCanvas.Children.Clear();
    }
}