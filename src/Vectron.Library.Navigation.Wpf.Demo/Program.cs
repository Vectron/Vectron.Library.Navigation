using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vectron.Extensions.Hosting;
using Vectron.Extensions.Hosting.Wpf;
using Vectron.Extensions.Logging.TextBlock;
using Vectron.Library.Navigation.Wpf;
using Vectron.Library.Navigation.Wpf.Demo;

var hostBuilder = Host.CreateApplicationBuilder(args);

_ = hostBuilder.Services
    .AddResourceDictionary("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")
    .AddResourceDictionary("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")
    .AddResourceDictionary("pack://application:,,,/Vectron.Library.Navigation.Wpf.Demo;component/TouchStyle.xaml")
    .AddResourceDictionary("pack://application:,,,/Vectron.Library.Navigation.Wpf;component/Navigation.xaml")
    .AddResourceDictionary("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.Steel.xaml")
    .AddSingleton<MahApps.Metro.Controls.Dialogs.IDialogCoordinator, MahApps.Metro.Controls.Dialogs.DialogCoordinator>()
    .AddWpf<App, MainWindow, MainWindowViewModel>()
    .AddScopedHost()
    .AddSingleton<IMessenger>(s => WeakReferenceMessenger.Default);

var rootBuilder = hostBuilder.Services
    .AddWpfNavigation()
    .AddChild(x =>
    {
        _ = x.Caption("Page 1")
        .Content<PageViewModel>();
    });

rootBuilder
    .Caption("Page 2")
    .Content<PageViewModel>()
    .AddChild()
        .Caption("Sub page 1")
        .Content<PageViewModel>();

hostBuilder.Logging
    .AddThemedTextBlock(x => x.Theme = "MEL-Dark");

using var host = hostBuilder.Build();
await host.RunAsync().ConfigureAwait(false);
