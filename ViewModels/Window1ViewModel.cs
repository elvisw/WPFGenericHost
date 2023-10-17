using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGenericHost.Models;
using WPFGenericHost.Services;

namespace WPFGenericHost.ViewModels
{
    public partial class Window1ViewModel : ObservableObject, IRecipient<StringMessage>
    {
        private readonly ITextService _textService;
        string[] _titles = { "Excellent", "Good", "Super", "REALLY GOOD DOCTOR!", "THANK YOU!", "THE BEST", "EXCELLENT PHYSICIAN", "EXCELLENT DOCTOR" };


        [ObservableProperty]
        private string? _helloText;

        public ObservableCollection<string> List1 { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string? _message;

        /// <summary>
        /// 无参构造函数，为XAML设计器提供设计时支持
        /// </summary>
        public Window1ViewModel()
        {
            _textService = new TextServiceForDesigner();
            HelloText = _textService.GetText();
        }
        /// <summary>
        /// 用于依赖注入的构造函数
        /// </summary>
        /// <param name="textService">用于依赖注入</param>
        public Window1ViewModel(ITextService textService)
        {
            _textService = textService;
            HelloText = _textService.GetText();
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        [RelayCommand]
        private void AddItem()
        {
            List1.Add(_titles[new Random().Next(0, _titles.Length)]);
        }

        public void Receive(StringMessage message)
        {
            Message = message.Value;
        }
    }
}
