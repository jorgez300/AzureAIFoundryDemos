using Microsoft.Extensions.Configuration;
using SemanticKernelAI.ChatClient;


// Leer configuración de appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


var aiClient = new AIChatClient(config);

while (true)
{
    Console.Write("User: ");

    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput))
    {
        break;
    }

    Console.Write("IA: ");
    Console.WriteLine(await aiClient.SendMessageAsync(userInput));

}