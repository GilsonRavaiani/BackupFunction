# BackupFunction

Vamos começar com minha amiga IA.

Perfeito, Gilson — vamos começar essa aplicação do zero com C# para uma Azure Function de backup automatizado via Timer Trigger. Aqui está um guia bem enxuto pra você iniciar seu repositório com tudo que precisa 🚀

🧪 Estrutura inicial do seu repositório

Crie uma pasta com a seguinte estrutura:

BackupFunction/
├── BackupFunction.csproj
├── host.json
├── local.settings.json
└── BackupFunction.cs

📦 Arquivo BackupFunction.csproj

<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <AzureFunctionsVersion>v4</AzureFunctionsVersion>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Azure.Functions.Worker" Version="1.18.0" />
    <PackageReference Include="Azure.Storage.Blobs" Version="12.20.0" />
    <PackageReference Include="Microsoft.Extensions.Logging.Console" Version="7.0.0" />
  </ItemGroup>
</Project>

⚙️ Arquivo host.json

{
  "version": "2.0",
  "logging": {
    "applicationInsights": {
      "samplingSettings": {
        "isEnabled": true
      }
    }
  }
}


🔐 Arquivo local.settings.json (apenas para teste local)

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "SRC_STORAGE_CONNECTION": "<string_conexao_origem>",
    "DEST_STORAGE_CONNECTION": "<string_conexao_destino>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}


🧠 Arquivo BackupFunction.cs


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


💡 Pronto para publicar

Depois que tudo estiver funcionando localmente, você pode:

    subir para seu repositório GitHub

    habilitar o GitHub Actions como te mostrei antes

    configurar as variáveis no portal do Azure

#######################################################################################################

📘 README.md

# ⏰ Azure Function - Backup Automatizado

Este projeto contém uma **Azure Function App** escrita em **C# (.NET 8)** com **Timer Trigger**, que realiza backups automáticos entre containers do Azure Blob Storage.

## 🚀 Funcionalidades

- Backup diário de arquivos de um container de origem para um container de destino
- Uso de Timer Trigger via CRON
- Conexões configuradas por variáveis de ambiente
- Implantação contínua via GitHub Actions

## 📁 Estrutura

BackupFunction/
├── BackupFunction.csproj
├── host.json
├── local.settings.json
└── BackupFunction.cs



## 🛠️ Pré-requisitos

- Conta no Azure com Function App criada
- Azure Storage configurado (origem e destino)
- GitHub com repositório conectado
- SDK .NET 8 instalado localmente (para testes)

## ⚙️ Configuração local

Edite o `local.settings.json` com suas strings de conexão:

```json
{
  "Values": {
    "SRC_STORAGE_CONNECTION": "DefaultEndpointsProtocol=...",
    "DEST_STORAGE_CONNECTION": "DefaultEndpointsProtocol=...",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}

🧪 Testes locais

Execute a Function com:

func start


🔄 Implantação contínua

Habilite o GitHub Actions e adicione o segredo AZURE_FUNCTIONAPP_PUBLISH_PROFILE. O deploy será automático a cada push na branch main.
🧙‍♂️ Autor

Projeto criado por Gilson com apoio do Copilot — seu parceiro digital para aventuras .NET 😄


---

Se quiser posso gerar um banner em ASCII pro topo do README, ou criar um badge de status de build. Também posso sugerir testes com xUnit ou incluir um exemplo de log para monitorar a função. Bora deixar esse projeto bonito e funcional? 😎📦🔧

🎨 Banner ASCII para o topo do README.md

Você pode adicionar algo assim no topo:

   ____            _                 _____                _             
  |  _ \ ___   ___| |_   _ _ __ ___ | ____|_  _____ _ __ | |_ ___  _ __ 
  | |_) / _ \ / __| | | | | '__/ _ \|  _| \ \/ / _ \ '_ \| __/ _ \| '__|
  |  __/ (_) | (__| | |_| | | | (_) | |___ >  <  __/ |_) | || (_) | |   
  |_|   \___/ \___|_|\__,_|_|  \___/|_____/_/\_\___| .__/ \__\___/|_|   
                                                  |_|                  

📛 Badge de status de build (GitHub Actions)

Adicione isto logo abaixo do título do projeto no README.md:

![Azure Function Build](https://github.com/seu-usuario/seu-repositorio/actions/workflows/deploy-functionapp.yml/badge.svg)


🧪 Testes básicos com xUnit (opcional)

Você pode criar uma pasta tests/ com um projeto de testes unitários. Exemplo:

dotnet new xunit -n BackupFunction.Tests
dotnet add BackupFunction.Tests reference ../BackupFunction/BackupFunction.csproj

E criar um teste simples:

using Xunit;

public class TimerTests
{
    [Fact]
    public void Timer_ShouldNotBeNull()
    {
        var timer = new Microsoft.Azure.Functions.Worker.TimerInfo();
        Assert.NotNull(timer);
    }
}

######################################################################################################

🧰 Requisitos para teste local

# .NET SDK 8.0 +

# Azure Functions Core Tools ((instalar com npm i -g azure-functions-core-tools@4))

# Código da Function já criado (como definimos antes)

# Arquivo local.settings.json preenchido

⚙️ Passos para executar localmente

   1-Abra o terminal na pasta do projeto Exemplo:
    cd BackupFunction
   2-Restaure os pacotes e compile
    dotnet restore
    dotnet build
   3-Verifique seu local.settings.json Preencha os campos de conexão:
     {
  "Values": {
    "SRC_STORAGE_CONNECTION": "UseDevelopmentStorage=true",
    "DEST_STORAGE_CONNECTION": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}

👉 Você pode usar o Azurite para simular o Azure Storage localmente (npm install -g azurite → azurite)

   4-Inicie a Function App localmente
   func start
   5-Verifique os logs Você verá algo como:
   Executing 'BackupFunction' (Reason='Timer triggered', Id=...)
   Backup iniciado: ...
   Backup concluído!

 

