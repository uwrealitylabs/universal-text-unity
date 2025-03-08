using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;  // Add this at the top

[System.Serializable]
public class DeepSeekRequest
{
    public string model = "deepseek-r1:1.5b";  // Exact name from ollama list
    public string prompt;
}

[System.Serializable]
public class DeepSeekResponse
{
    public string response;
}

public class DeepSeekHandler : MonoBehaviour
{
    [SerializeField] private string modelName = "deepseek-r1:1.5b";  // Match exactly
    
    private string apiUrl = "http://127.0.0.1:11434/api/generate";  // Same Ollama endpoint
    private List<string> messageLog = new List<string>();

    public void SendMessage(string userMessage, System.Action<string> onResponse)
    {
        Debug.Log($"DeepSeekHandler: Sending message: {userMessage}");
        messageLog.Add(userMessage);
        StartCoroutine(SendRequestCoroutine(userMessage, onResponse));
    }

    private IEnumerator SendRequestCoroutine(string message, System.Action<string> onResponse)
    {
        Debug.Log($"DeepSeekHandler: Starting request for message: {message}");
        
        // Create the request data with exact model name
        var requestData = new Dictionary<string, string>
        {
            { "model", "deepseek-r1:1.5b" },  // Hardcode to ensure exact match
            { "prompt", message }
        };

        string jsonPayload = JsonConvert.SerializeObject(requestData);
        Debug.Log($"DeepSeekHandler: Sending JSON payload: {jsonPayload}");

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"DeepSeekHandler: Sending request to {apiUrl}");
            yield return request.SendWebRequest();

            Debug.Log($"DeepSeekHandler: Request completed with result: {request.result}");
            Debug.Log($"DeepSeekHandler: Response Code: {request.responseCode}");
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log($"DeepSeekHandler: Received raw response: {responseText}");
                string parsedResponse = ParseDeepSeekResponse(responseText);
                Debug.Log($"DeepSeekHandler: Parsed response: {parsedResponse}");
                messageLog.Add(parsedResponse);
                onResponse?.Invoke(parsedResponse);
            }
            else
            {
                Debug.LogError($"Error calling DeepSeek API: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
                Debug.LogError($"Full Response: {request.downloadHandler?.text}");
                Debug.LogError($"Request URL: {request.url}");
                onResponse?.Invoke(null);
            }
        }
    }

    private string ParseDeepSeekResponse(string responseText)
    {
        List<string> responses = new List<string>();

        // Split response by newline and parse each line as JSON
        foreach (string line in responseText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            try
            {
                var jsonResponse = JsonUtility.FromJson<DeepSeekResponse>(line);
                if (jsonResponse != null && !string.IsNullOrEmpty(jsonResponse.response))
                {
                    responses.Add(jsonResponse.response);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Could not parse line: {line}. Error: {ex.Message}");
            }
        }

        return string.Join("", responses);
    }

    // Helper method to get conversation history
    public List<string> GetMessageLog()
    {
        return new List<string>(messageLog);
    }

    // Helper method to clear conversation history
    public void ClearMessageLog()
    {
        messageLog.Clear();
    }
} 