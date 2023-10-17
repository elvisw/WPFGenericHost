using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGenericHost.Models;
using WPFGenericHost.Services;
using WPFGenericHost.Views;

namespace WPFGenericHost.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _message;

        public MainViewModel()
        {
        }
        public MainViewModel(ILogger<MainViewModel> logger)
        {
            logger.LogInformation($"{typeof(MainWindow)} has been loaded.");
            //WeakReferenceMessenger.Default.RegisterAll(this);
        }


        [RelayCommand]
        private void OpenWindow1()
        {
            var window1 = App.Current.Services.GetService<Window1>();
            window1?.Show();
            WeakReferenceMessenger.Default.Send(new StringMessage("Window1 has been opened"));
        }

        [RelayCommand]
        private void SendMessage()
        {
            WeakReferenceMessenger.Default.Send(new StringMessage(Message!));
        }

    }
}
