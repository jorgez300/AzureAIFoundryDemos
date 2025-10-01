using CognitiveServices.TextToVoiceClient;
using Microsoft.Extensions.Configuration;

// Leer configuración de appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


var _client = new TTSClient(config);

await _client.GenerateSpeechAsync("Hola, este es un ejemplo de texto a voz usando Azure OpenAI y Cognitive Services", "alloy");