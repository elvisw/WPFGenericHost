using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGenericHost.Services;

namespace WPFGenericHost.ViewModels
{
    [ObservableObject]
    public partial class MainViewModel
    {
        private readonly ITextService _textService;

        [ObservableProperty]
        private string? helloText;
        public MainViewModel(ITextService textService)
        {
            _textService = textService;
            HelloText = _textService.GetText();
        }
    }
}
