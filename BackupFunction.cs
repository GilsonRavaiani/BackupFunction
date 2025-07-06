public class BackupFunction
{
    [Function("BackupFunction")]
    public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo timer, FunctionContext context)
    {
        var log = context.GetLogger("BackupFunction");
        log.LogInformation("Função iniciada");

        // lógica de backup aqui

        log.LogInformation("Backup concluído!");
    }
}
