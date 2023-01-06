using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGenericHost.Services
{
    public class TextService : ITextService
    {
        private readonly string _text;
        private readonly ILogger _logger;
        public TextService(IOptions<Settings> options, ILogger<TextService> logger)
        {
            _logger = logger;
            _text = options.Value.Text;
            _logger.LogInformation($"Text read from settings: '{options.Value.Text}'");
        }

        public string GetText() => _text;
    }
}
