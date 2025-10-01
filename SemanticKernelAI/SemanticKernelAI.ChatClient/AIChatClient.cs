using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;


namespace SemanticKernelAI.ChatClient
{
    public class AIChatClient
    {
        private readonly IChatCompletionService _chatService;
        private readonly ChatHistory _history;
        private readonly Kernel _kernel;
        private readonly AzureOpenAIPromptExecutionSettings _settings = new()
        {
            /*MaxTokens = 800,
            Temperature = 0.7,
            TopP = 0.95,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            StopSequences = new[] { "User:", "Assistant:" },*/
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        public AIChatClient(IConfigurationRoot config)
        {


            var deploymentName = config["AzureOpenAI:DeploymentName"];
            var endpoint = config["AzureOpenAI:Endpoint"];
            var apiKey = config["AzureOpenAI:ApiKey"];

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: deploymentName ?? "",
                endpoint: endpoint ?? "",
                apiKey: apiKey ?? ""
            );

            _kernel = builder.Build();
            _chatService = _kernel.GetRequiredService<IChatCompletionService>();
            _history = new ChatHistory();
            _history.AddSystemMessage("Eres un asistente que responde siempre de forma corta");

        }

        public async Task<string> SendMessageAsync(string userMessage)
        {
            _history.AddUserMessage(userMessage);
            var chatResponse = await _chatService.GetChatMessageContentsAsync(_history, _settings, _kernel);
            var responseText = chatResponse[^1].Content?? "";
            _history.AddAssistantMessage(responseText);
            return responseText;
        }
    }
}
