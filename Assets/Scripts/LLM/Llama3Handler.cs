using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Llama3Request
{
    public string model = "llama3.2";
    public string prompt;
}

[System.Serializable]
public class Llama3Response
{
    public string response;
}

public class Llama3Handler : MonoBehaviour
{
    [SerializeField] private string modelName = "llama3.2";
    
    private string apiUrl = "http://127.0.0.1:11434/api/generate";
    private List<string> messageLog = new List<string>();

    public void SendMessage(string userMessage, System.Action<string> onResponse)
    {
        messageLog.Add(userMessage);
        StartCoroutine(SendRequestCoroutine(userMessage, onResponse));
    }

    private IEnumerator SendRequestCoroutine(string message, System.Action<string> onResponse)
    {
        var requestData = new Llama3Request
        {
            model = modelName,
            prompt = message
        };

        string jsonPayload = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                string parsedResponse = ParseLlamaResponse(responseText);
                messageLog.Add(parsedResponse);
                onResponse?.Invoke(parsedResponse);
            }
            else
            {
                Debug.LogError("Error calling Llama API: " + request.error);
                onResponse?.Invoke(null);
            }
        }
    }

    private string ParseLlamaResponse(string responseText)
    {
        List<string> responses = new List<string>();

        // Split response by newline and parse each line as JSON
        foreach (string line in responseText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            try
            {
                var jsonResponse = JsonUtility.FromJson<Llama3Response>(line);
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
