using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPFGenericHost.Services
{
    /// <summary>
    /// 用于给视图模型类的无参构造函数提供一个非依赖注入的实现
    /// </summary>
    public class TextServiceForDesigner : ITextService
    {
        public string GetText() => "Hello WPF .NET 6.0\nFor XAML Desinger Test!";
    }
}
