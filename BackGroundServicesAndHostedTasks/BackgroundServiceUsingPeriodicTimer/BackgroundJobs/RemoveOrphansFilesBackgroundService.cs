using System.Runtime.CompilerServices;
using System.Security.AccessControl;
//  A Hosted Service :
//  is simply a class that starts automatically when the application starts and stops when the application shuts down.
public class RemoveOrphansFilesBackgroundService(ILogger<RemoveOrphansFilesBackgroundService> logger) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("CleanUp service started at time {startingTime}",DateTimeOffset.UtcNow);
        
        var periodicTimer =new PeriodicTimer(_interval);
        while(await periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                logger.LogInformation("scanning for orphans items");
                await Task.Delay(1000,stoppingToken);

                int orphanedItemsCount = Random.Shared.Next(1,10);

                logger.LogInformation("Deleted {items} orphaned items at {time}",orphanedItemsCount,DateTimeOffset.UtcNow);
            }
            catch(Exception ex)
            {
                logger.LogError(ex,"Error in Remove orphaned files background service");
            }
        }
        // or the old way
        // while(!stoppingToken.IsCancellationRequested)
        // {
        //     try
        //     {
        //         logger.LogInformation("scanning for orphans items");
        //         await Task.Delay(1000,stoppingToken);

        //         int orphanedItemsCount = Random.Shared.Next(1,10);

        //         logger.LogInformation("Deleted {items} orphaned items at {time}",orphanedItemsCount,DateTimeOffset.UtcNow);
        //     }
        //     catch(Exception ex)
        //     {
        //         logger.LogError(ex,"Error in Remove orphaned files background service");
        //     }
        //     await Task.Delay(_interval,stoppingToken);
        // }
    }
}