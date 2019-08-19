using Microsoft.Extensions.Logging;

namespace WireMockServer
{
    public class App
    {
        private readonly IWireMockService _service;
        private readonly ILogger _logger;
        
        public App(IWireMockService service, ILoggerFactory factory)
        {
            _service = service;
            _logger = factory.CreateLogger("WireMock .NET");
        }

        public void Run()
        {
            _logger.LogInformation("WireMock.Net App running");
            _service.Run();
        }
    }
}