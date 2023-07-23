using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ProSupplier.Api.Controllers;
using ProSupplier.Business.Interfaces;

namespace ProSupplier.Api.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly ILogger _logger;

        public TesteController(INotifier notifier, IUser appUser, ILogger<TesteController> logger) : base(notifier, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {

            throw new Exception("error");

            //try
            //{
            //    var i = 0;
            //    var result = 42 / i;
            //}
            //catch (DivideByZeroException e)
            //{
            //    e.Ship(HttpContext);
            //}

            _logger.LogTrace("Log de Trace");
            _logger.LogDebug("Log de Debug");
            _logger.LogInformation("Log de Informação");
            _logger.LogWarning("Log de Aviso");
            _logger.LogError("Log de Erro");
            _logger.LogCritical("Log de problema crítico");

            return "Eu sou a V2";
        }
    }
}
