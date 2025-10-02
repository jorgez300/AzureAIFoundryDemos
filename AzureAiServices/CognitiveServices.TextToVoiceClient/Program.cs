using CognitiveServices.TextToVoiceClient;
using Microsoft.Extensions.Configuration;

// Leer configuración de appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


var _client = new TTSClient(config);

Console.WriteLine("Seleccione la voz: ");
Console.WriteLine("1: alloy");
Console.WriteLine("2: echo");
Console.WriteLine("3: fable");
Console.WriteLine("4: onyx");
Console.WriteLine("5: nova");
Console.WriteLine("6: shimmer");

var selectedvoiceinput = Console.ReadLine();
VoiceType selectedvoice = VoiceType.alloy;

if (string.IsNullOrWhiteSpace(selectedvoiceinput))
{
    return;
}

switch (selectedvoiceinput)
{
    case "1":
        selectedvoice = VoiceType.alloy;
        break;
    case "2":
        selectedvoice = VoiceType.echo;
        break;
    case "3":
        selectedvoice = VoiceType.fable;
        break;
    case "4":
        selectedvoice = VoiceType.onyx;
        break;
    case "5":
        selectedvoice = VoiceType.nova;
        break;
    case "6":
        selectedvoice = VoiceType.shimmer;
        break;
    default:
        Console.WriteLine("Opción no válida, se usará la voz por defecto (alloy).");
        break;
}


while (true)
{

    Console.Write("User: ");

    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput))
    {
        break;
    }

    await _client.GenerateSpeechAsync(userInput, selectedvoice);


}
