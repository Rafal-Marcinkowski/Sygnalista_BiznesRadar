using Library.Communication;
using Library.Interfaces;
using PogromcaBiznesRadar.MVVM.ViewModels;
using PogromcaBiznesRadar.MVVM.Views;
using PogromcaBiznesRadar.Services;
using System.Windows;

namespace PogromcaBiznesRadar;

public partial class App : PrismApplication
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindowView>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<MainWindowViewModel>();
        containerRegistry.RegisterSingleton<MainWindowView>();
        containerRegistry.RegisterSingleton<CommunicationManager>();
        containerRegistry.RegisterSingleton<ICommunicationService, CommunicationService>();
    }
}
