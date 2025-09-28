using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VerifyNewLibroValue;

public class FunctionVerifyNewLibroValue
{
    private readonly ILogger _logger;

    public FunctionVerifyNewLibroValue(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<FunctionVerifyNewLibroValue>();
    }

    [Function("FunctionVerifyNewLibroValue")]
    public void Run ([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {  
        _logger.LogInformation("C# Timer trigger function executed at: {ExecutionTime}", DateTime.Now);
                                                                                                                                                                                                                                                                                                                                                                      
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {NextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}