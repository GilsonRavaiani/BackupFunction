using System;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BackupFunctionApp
{
    public class BackupFunction
    {
        [Function("BackupFunction")]
        public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer, FunctionContext context)
        {
            var log = context.GetLogger("BackupFunction");
            log.LogInformation($"Backup iniciado: {DateTime.UtcNow}");

            var srcConnection = Environment.GetEnvironmentVariable("SRC_STORAGE_CONNECTION");
            var destConnection = Environment.GetEnvironmentVariable("DEST_STORAGE_CONNECTION");

            var srcClient = new BlobServiceClient(srcConnection);
            var destClient = new BlobServiceClient(destConnection);

            var srcContainer = srcClient.GetBlobContainerClient("origem");
            var destContainer = destClient.GetBlobContainerClient("backup");

            foreach (var blob in srcContainer.GetBlobs())
            {
                var srcBlob = srcContainer.GetBlobClient(blob.Name);
                var destBlob = destContainer.GetBlobClient(blob.Name);

                var content = srcBlob.DownloadContent().Value.Content;
                destBlob.Upload(content.ToStream(), overwrite: true);
            }

            log.LogInformation("Backup concluído!");
        }
    }
}
