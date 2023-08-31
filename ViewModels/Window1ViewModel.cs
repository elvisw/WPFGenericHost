using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGenericHost.Services;

namespace WPFGenericHost.ViewModels
{
    public partial class Window1ViewModel : ObservableObject
    {
        private readonly ITextService _textService;

        [ObservableProperty]
        private string? helloText;
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
        }
    }
}
