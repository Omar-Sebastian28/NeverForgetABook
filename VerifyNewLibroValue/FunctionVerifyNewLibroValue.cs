using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VerifyNewLibroValue;

public class FunctionVerifyNewLibroValue
{
    public FunctionVerifyNewLibroValue([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer , ILoggerFactory loggerFactory)
    {
       var logger = loggerFactory.CreateLogger<FunctionVerifyNewLibroValue>();  
       logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}