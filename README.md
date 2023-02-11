# WPFGenericHost

��WPFӦ��������Microsoft.Extensions.Hosting���Ա�߱�����ע�롢��־�����õȹ���

�ο���https://laurentkempe.com/2019/09/03/WPF-and-dotnet-Generic-Host-with-dotnet-Core-3-0/

����`CommunityToolkit.Mvvm`��`Microsoft.Extensions.Hosting`������nuget����

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

# ���⣺
1. ~~��ͼģ��ʹ��������ע��󣬹��캯������в������޷���xaml�а�`DataContext`��ֻ�������ش����ﴦ����͵������޷�ʹ��Visual Studio��xaml�����ͼ������󶨣�ֻ���ֹ���дxaml���룬����xaml�����ͼ�޷�ʵʱԤ�������ݣ�ֻ�����г���󿴵�Ч���� ~~

�����
1. Ϊ��ͼģ�����ʹ����������캯�����޲ι��캯���������ʱ���ݣ���һ����������ע�롣

```csharp
public partial class MainViewModel : ObservableObject
    {
        private readonly ITextService _textService;

        [ObservableProperty]
        private string? helloText;
        /// <summary>
        /// �޲ι��캯����ΪXAML������ṩ���ʱ֧��
        /// </summary>
        public MainViewModel()
        {
            _textService = new TextServiceForDesigner();
            HelloText = _textService.GetText();
        }
        /// <summary>
        /// ��������ע��Ĺ��캯��
        /// </summary>
        /// <param name="textService">��������ע��</param>
        public MainViewModel(ITextService textService)
        {
            _textService = textService;
            HelloText = _textService.GetText();
        }
    }
```

2. ʹ�� [���ʱ����](http://msdn.microsoft.com/en-us/library/ff602277(v=vs.95).aspx) ������ͼģ����������а󶨣���XAML�У�����������Ե�`<Window>`��`<UserControl>`��

```
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
mc:Ignorable="d"
d:DataContext="{d:DesignInstance Type=vm:MainViewModel, IsDesignTimeCreatable=True}"
```

����ʱ��`DataContext`����Ȼ��Ҫ�����ش����ﴦ������ע�룺

```csharp
public MainWindow(MainViewModel mvm)
        {
            InitializeComponent();
            DataContext = mvm;
        }
```


�ο���https://stackoverflow.com/questions/25366291/how-to-handle-dependency-injection-in-a-wpf-mvvm-application/25508012#25508012