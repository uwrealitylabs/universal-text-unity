using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class ChatRequest
{
    public string model;
    public List<Message> messages;
    public int max_tokens;
    
    // Add these fields to match OpenAI's API expectations
    public float temperature = 0.7f;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class ChatResponse
{
    public string id;
    public string object_type;
    public long created;
    public string model;
    public List<Choice> choices;
    public Usage usage;
}

[System.Serializable]
public class Choice
{
    public Message message;
    public string finish_reason;
    public int index;
}

[System.Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}

public class ChatGPTHandler : MonoBehaviour
{
    [SerializeField] private string apiKey = "INSERT_API_KEY";
    [SerializeField] private string modelName = "gpt-3.5-turbo";
    [SerializeField] private int maxTokens = 150;
    [SerializeField] private float temperature = 0.7f;
    
    private List<Message> messagesLog = new List<Message>();

    public void SendMessage(string userMessage, System.Action<string> onResponse)
    {
        StartCoroutine(SendRequestCoroutine(userMessage, onResponse));
    }

    private IEnumerator SendRequestCoroutine(string sentMessage, System.Action<string> onResponse)
    {
        var userMessage = new Message { role = "user", content = sentMessage };
        messagesLog.Add(userMessage);

        ChatRequest requestData = new ChatRequest
        {
            model = modelName,
            messages = messagesLog,
            max_tokens = maxTokens
        };

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log($"Sending request to ChatGPT: {jsonData}");

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
                ChatResponse chatResponse = JsonUtility.FromJson<ChatResponse>(responseText);
                if (chatResponse.choices != null && chatResponse.choices.Count > 0)
                {
                    string responseMessage = chatResponse.choices[0].message.content;
                    
                    // Add assistant's response to message log
                    messagesLog.Add(new Message { role = "assistant", content = responseMessage });
                    
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
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response: " + request.downloadHandler.text);
                onResponse?.Invoke(null);
            }
        }
    }
    
    public List<Message> GetMessageLog()
    {
        return new List<Message>(messagesLog);
    }

    public void ClearMessageLog()
    {
        messagesLog.Clear();
    }
}
