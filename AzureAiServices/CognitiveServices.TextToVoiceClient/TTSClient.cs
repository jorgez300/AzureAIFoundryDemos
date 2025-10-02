using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CognitiveServices.TextToVoiceClient
{
    public enum VoiceType
    {
        alloy,
        echo,
        fable,
        onyx,
        nova,
        shimmer
    }

    public class TTSClient
    {
        IConfigurationRoot _config;
        public TTSClient(IConfigurationRoot config)
        {
            this._config = config;
        }

        public async Task GenerateSpeechAsync(string inputText, VoiceType voice = VoiceType.alloy)
        {
            var url = this._config["AzureOpenAI:Endpoint"];
            using var client = new HttpClient();

            var rawinput = "<speak>Hola, esto es una prueba.<break time=\"1500ms\"/>Ahora hay una pausa de 1.5 segundos.<break time=\"2000ms\"/>Y aquí continúa el audio.</speak>";

            //var rawinput = $"<speak><break time=\"2000ms\"/>        {inputText}.</speak>";


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._config["AzureOpenAI:ApiKey"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var payload = new
            {
                model = this._config["AzureOpenAI:DeploymentName"],
                input = rawinput,
                voice = voice.ToString()
            };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)

            {
                var audioBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = $"Audio{DateTime.Now:HHmmss}.wav";
                await File.WriteAllBytesAsync(fileName, audioBytes);
                Console.WriteLine($"Audio guardado en {fileName}");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}\n{error}");
            }
        }
    }
}
