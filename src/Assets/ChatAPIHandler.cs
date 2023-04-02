using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace TAC {
    class ChatAPIHandler {
        private OpenAIService openAIService;
        public ChatAPIHandler() {
            openAIService = new OpenAIService(new OpenAiOptions(){
                ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            });
        }

        public async Task<string> getResponse(string prompt) {
            ChatCompletionCreateResponse completionResult = await openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest {
                Messages = new List<ChatMessage> {ChatMessage.FromUser(prompt)},
                Model = Models.ChatGpt3_5Turbo
            });

            if (completionResult.Successful) {
                System.Console.WriteLine(completionResult.Choices[0].Message.Content);
                return completionResult.Choices[0].Message.Content;
            }
            return "Something went wrong.";
        }
    }
}