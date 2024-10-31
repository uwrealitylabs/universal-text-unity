using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

// Define the request structure
[System.Serializable]
public class ChatRequest
{
    public string model;
    public List<Message> messages;
    public int max_tokens;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

// Define the response structure
[System.Serializable]
public class ChatResponse
{
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}


public class ChatGPTHandler : MonoBehaviour
{
    private string apiKey = "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";
    private List<Message> messagesLog = new List<Message>();

    public void SendReply(string sentMessage, System.Action<string> onResponse)
    {
        StartCoroutine(SendRequestCoroutine(sentMessage, onResponse));
    }

    private IEnumerator SendRequestCoroutine(string sentMessage, System.Action<string> onResponse)
    {
        // Create the user message
        var userMessage = new Message { role = "user", content = sentMessage };
        messagesLog.Add(userMessage);

        // Create the chat request
        ChatRequest requestData = new ChatRequest
        {
            model = "gpt-3.5-turbo",
            messages = messagesLog,
            max_tokens = 150
        };

        string jsonData = JsonUtility.ToJson(requestData);

        // Set up UnityWebRequest
        using (UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var responseText = request.downloadHandler.text;

                // Deserialize response
                ChatResponse chatResponse = JsonUtility.FromJson<ChatResponse>(responseText);
                if (chatResponse.choices != null && chatResponse.choices.Count > 0)
                {
                    string responseMessage = chatResponse.choices[0].message.content;
                    onResponse?.Invoke(responseMessage);
                }
                else
                {
                    Debug.LogWarning("No response received from API.");
                    onResponse?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                onResponse?.Invoke(null);
            }
        }
    }
}
