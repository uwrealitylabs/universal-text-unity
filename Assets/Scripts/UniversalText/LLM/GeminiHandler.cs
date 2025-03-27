using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GeminiContent
{
    public string role;
    public List<GeminiPart> parts;

    public GeminiContent(string role, string text)
    {
        this.role = role;
        this.parts = new List<GeminiPart> { new GeminiPart { text = text } };
    }
}

[System.Serializable]
public class GeminiPart
{
    public string text;
}

[System.Serializable]
public class GeminiRequest
{
    public List<GeminiContent> contents;
    public GenerationConfig generationConfig;

    public GeminiRequest()
    {
        contents = new List<GeminiContent>();
        generationConfig = new GenerationConfig();
    }
}

[System.Serializable]
public class GenerationConfig
{
    public int maxOutputTokens = 150;
    public float temperature = 0.7f;
}

[System.Serializable]
public class GeminiResponse
{
    public List<GeminiCandidate> candidates;
}

[System.Serializable]
public class GeminiCandidate
{
    public GeminiContent content;
}

public class GeminiHandler : MonoBehaviour
{
    private string apiKey = "insert api key";
    private List<GeminiContent> messagesLog = new List<GeminiContent>();
    private string modelName = "gemini-2.0-flash";

    public void SendReply(string sentMessage, System.Action<string> onResponse)
    {
        StartCoroutine(SendRequestCoroutine(sentMessage, onResponse));
    }

    private IEnumerator SendRequestCoroutine(string sentMessage, System.Action<string> onResponse)
    {
        var userMessage = new GeminiContent("user", sentMessage);
        messagesLog.Add(userMessage);

        GeminiRequest requestData = new GeminiRequest();
        requestData.contents = messagesLog;
        requestData.generationConfig.maxOutputTokens = 150;

        string jsonData = JsonUtility.ToJson(requestData);

        string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={apiKey}";
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var responseText = request.downloadHandler.text;
                GeminiResponse geminiResponse = JsonUtility.FromJson<GeminiResponse>(responseText);

                if (geminiResponse.candidates != null && geminiResponse.candidates.Count > 0)
                {
                    string responseMessage = geminiResponse.candidates[0].content.parts[0].text;

                    messagesLog.Add(new GeminiContent("model", responseMessage));

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
                Debug.LogError($"Error: {request.error}\nResponse: {request.downloadHandler.text}");
                onResponse?.Invoke(null);
            }
        }
    }
}
