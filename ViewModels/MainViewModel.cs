using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGenericHost.Services;
using WPFGenericHost.Views;

namespace WPFGenericHost.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
        }
        public MainViewModel(ILogger<MainViewModel> logger)
        {
            logger.LogInformation($"{typeof(MainWindow)} has been loaded.");
        }

        [RelayCommand]
        private void OpenWindow1()
        {
            var window1 = App.Current.Services.GetService<Window1>();
            window1?.Show();
        }
    }
}
