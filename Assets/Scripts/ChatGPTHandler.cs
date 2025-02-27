/*using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

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
    private string apiKey = "insert api key";
    private List<Message> messagesLog = new List<Message>();

    public void SendReply(string sentMessage, System.Action<string> onResponse)
    {
        StartCoroutine(SendRequestCoroutine(sentMessage, onResponse));
    }

    private IEnumerator SendRequestCoroutine(string sentMessage, System.Action<string> onResponse)
    {
        var userMessage = new Message { role = "user", content = sentMessage };
        messagesLog.Add(userMessage);
sk-
        ChatRequest requestData = new ChatRequest
        {
            model = "gpt-3.5-turbo",
            messages = messagesLog,
            max_tokens = 150
        };

        string jsonData = JsonUtility.ToJson(requestData);

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
}*/
