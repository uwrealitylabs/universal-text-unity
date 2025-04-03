using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

[System.Serializable]
public class DeepSeekRequest
{
    public string model = "deepseek-r1:1.5b";
    public string prompt;
}

[System.Serializable]
public class DeepSeekResponse
{
    public string response;
}

public class DeepSeekHandler : MonoBehaviour
{
    [SerializeField] private string modelName = "deepseek-r1:1.5b";
    
    private string apiUrl = "http://127.0.0.1:11434/api/generate";
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
        
        var requestData = new Dictionary<string, string>
        {
            { "model", "deepseek-r1:1.5b" }, 
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
                
                // Try a completely different parsing approach
                string parsedResponse = ParseResponseManually(responseText);
                Debug.Log($"DeepSeekHandler: Manually parsed response: {parsedResponse}");
                
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

    private string ParseResponseManually(string responseText)
    {
        // Split the response by lines
        string[] lines = responseText.Split('\n');
        StringBuilder finalResponse = new StringBuilder();
        
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            try
            {
                // Parse the JSON
                Dictionary<string, string> responsePart = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(line);
                
                if (responsePart != null && responsePart.ContainsKey("response"))
                {
                    string text = responsePart["response"];
                    
                    // Skip thinking sections
                    if (text.Contains("<think>") && text.Contains("</think>"))
                    {
                        int startIdx = text.IndexOf("<think>");
                        int endIdx = text.IndexOf("</think>") + "</think>".Length;
                        
                        if (startIdx > 0)
                        {
                            finalResponse.Append(text.Substring(0, startIdx));
                        }
                        
                        if (endIdx < text.Length)
                        {
                            finalResponse.Append(text.Substring(endIdx));
                        }
                    }
                    else
                    {
                        // No thinking tags, just append the text
                        finalResponse.Append(text);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to parse line: {line}. Error: {ex.Message}");
            }
        }
        
        // Clean up any remaining tags
        string result = finalResponse.ToString()
            .Replace("<assistant>", "")
            .Replace("</assistant>", "")
            .Trim();
            
        // Add spaces after punctuation if needed
        result = Regex.Replace(result, @"([.,!?;:])([a-zA-Z])", "$1 $2");
        
        return result;
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