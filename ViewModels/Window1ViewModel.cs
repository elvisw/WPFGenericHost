using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
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

        [ObservableProperty]
        private string? _helloText;

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

        public void Receive(StringMessage message)
        {
            Message = message.Value;
        }
    }
}
