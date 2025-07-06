using System;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions;
using Microsoft.Extensions.Logging;

public class BackupFunction
{
    [Function("BackupFunction")]
    public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo timer, FunctionContext context)
    {
        var log = context.GetLogger("BackupFunction");
        log.LogInformation($"Backup iniciado: {DateTime.UtcNow}");

        // Lógica do backup aqui...

        log.LogInformation("Backup concluído!");
    }
}
