using Bliblioteca.Core.Aplication.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace VerifyNewLibroValue;

public class FunctionVerifyNewLibroValue
{
    private readonly ILogger _logger;
    private readonly ILibroServices _libroServices;


    public FunctionVerifyNewLibroValue(ILoggerFactory loggerFactory, ILibroServices libroServices)
    {
        _logger = loggerFactory.CreateLogger<FunctionVerifyNewLibroValue>();
        _libroServices = libroServices; 
    }


    [Function("FunctionVerifyNewLibroValue")]
    public async Task Run ([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {NextSchedule}", myTimer.ScheduleStatus.Next);
        }

        _logger.LogInformation("C# Timer trigger function executed at: {ExecutionTime}", DateTime.Now);

        if (myTimer.IsPastDue)
        {
            _logger.LogWarning("The timer is past due!");
        }

        else 
        {
            var libros = _libroServices.GetQuery();
            if (!await libros.AnyAsync()) 
            {
              _logger.LogWarning("No hay libros registrados.");
            }

            var libroAnioActual = libros.Where(l => l.AñoPublicacion == DateTime.UtcNow.Year).ToList();
            _logger.LogInformation("Timer is on time.");
        }
    }
}