# WPFGenericHost

在WPF应用中引入Microsoft.Extensions.Hosting，以便具备依赖注入、日志、配置等功能

参考：https://laurentkempe.com/2019/09/03/WPF-and-dotnet-Generic-Host-with-dotnet-Core-3-0/

依赖`CommunityToolkit.Mvvm`和`Microsoft.Extensions.Hosting`这两个nuget包。

## App.xaml

```xml
<Application x:Class="wpfGenericHost.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:wpfGenericHost"
             Startup="Application_Startup"
             Exit="Application_Exit">
    <Application.Resources />         
</Application>
```

## App.xaml.cs

```csharp
public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<Settings>(context.Configuration);
                    services.AddSingleton<ITextService, TextService>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddDebug();
                })
                .Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
```

# 问题：
1. ~~视图模型使用了依赖注入后，构造函数里带有参数，无法在xaml中绑定`DataContext`，只能在隐藏代码里处理，这就导致了无法使用Visual Studio的xaml设计视图来处理绑定，只能手工编写xaml代码，并且xaml设计视图无法实时预览绑定数据，只能运行程序后看到效果。 ~~

解决：
1. 为视图模型类型创建两个构造函数，无参构造函数用于设计时数据，另一个用于依赖注入。

```csharp
public partial class MainViewModel : ObservableObject
    {
        private readonly ITextService _textService;

        [ObservableProperty]
        private string? helloText;
        /// <summary>
        /// 无参构造函数，为XAML设计器提供设计时支持
        /// </summary>
        public MainViewModel()
        {
            _textService = new TextServiceForDesigner();
            HelloText = _textService.GetText();
        }
        /// <summary>
        /// 用于依赖注入的构造函数
        /// </summary>
        /// <param name="textService">用于依赖注入</param>
        public MainViewModel(ITextService textService)
        {
            _textService = textService;
            HelloText = _textService.GetText();
        }
    }
```

2. 使用 [设计时属性](http://msdn.microsoft.com/en-us/library/ff602277(v=vs.95).aspx) ，将视图模型在设计器中绑定，在XAML中，添加以下属性到`<Window>`或`<UserControl>`：

```
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
mc:Ignorable="d"
d:DataContext="{d:DesignInstance Type=vm:MainViewModel, IsDesignTimeCreatable=True}"
```

运行时的`DataContext`，依然需要在隐藏代码里处理依赖注入：

```csharp
public MainWindow(MainViewModel mvm)
        {
            InitializeComponent();
            DataContext = mvm;
        }
```


参考：https://stackoverflow.com/questions/25366291/how-to-handle-dependency-injection-in-a-wpf-mvvm-application/25508012#25508012