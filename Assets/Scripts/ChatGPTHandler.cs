using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace OpenAI
{
    public class ChatGPTHandler : MonoBehaviour
    {

        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messagesLog = new List<ChatMessage>();
        private string prompt = "Answer the questions like a navigator. Don't break character. Don't ever mention that you are an AI model.";

        public async void SendReply(String sentMessage, Action<ChatMessage> onResponse)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = sentMessage
            };
            
            // If its the first message, add in the prompt so chatgpt acts properly
            if (messagesLog.Count == 0) newMessage.Content = prompt + "\n" + sentMessage; 
            
            // Add users message to the history
            messagesLog.Add(newMessage);
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o-mini",
                Messages = messagesLog
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messagesLog.Add(message); // Add the message to the chat history
                Debug.Log(message); // Print it to the console
                
                // Return the generated message
                onResponse?.Invoke(message);

            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

        
        }
    }
}
