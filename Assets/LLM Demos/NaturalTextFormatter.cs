using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class NaturalTextFormatter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI outputText;

    private string apiUrl = "http://127.0.0.1:11434/api/generate";
    private string command = "Make the given text after the colon sound like a natural description, referring to user in second person. Don't break character. Don't ever mention that you are an AI model: ";
    private string rawInput = "The user is grabbing in their right hand a wooden bowl with small volume, which is made of maple wood, is 25cm wide, and contains (a Fuji apple that can be eaten by raising it to your mouth, which is 120% ripe, contains (a common earthworm that makes any food that it infests unsafe to eat, which is 9mm long), and contains (an apple seed that contains a harmless amount of cyanide, which is of the Fuji variety)). The user is pointing with their left hand at a garbage can which is empty, and is next to (a composter which is empty)";

    void Start()
    {
        StartCoroutine(FormatTextWithLlama(command + rawInput));
    }

    private IEnumerator FormatTextWithLlama(string inputText)
    {
        string jsonPayload = "{\"model\":\"llama3\",\"prompt\":\"" + EscapeJson(inputText) + "\"}";

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
                outputText.text = ParseLlamaResponse(responseText);
            }
            else
            {
                Debug.LogError("Error calling Llama API: " + request.error);
                outputText.text = "Error formatting text";
            }
        }
    }

    private string EscapeJson(string input)
    {
        return input.Replace("\"", "\\\"").Replace("\\", "\\\\");
    }

    private string ParseLlamaResponse(string responseText)
    {
        List<string> responses = new List<string>();

        // Split response by newline and try parsing each line as JSON
        foreach (string line in responseText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            try
            {
                // Try to parse the line as JSON to extract the "response" field
                var jsonResponse = JsonUtility.FromJson<ResponseFormat>(line);
                
                // Check if the "response" field exists and add it to our responses list
                if (jsonResponse != null && !string.IsNullOrEmpty(jsonResponse.response))
                {
                    responses.Add(jsonResponse.response);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Could not parse line as JSON: " + line + " Error: " + ex.Message);
            }
        }

        // Combine all the responses into one single output string
        return string.Join(" ", responses);
    }

    [System.Serializable]
    private class ResponseFormat
    {
        public string response; // Assuming "response" is the correct field name
    }
}
