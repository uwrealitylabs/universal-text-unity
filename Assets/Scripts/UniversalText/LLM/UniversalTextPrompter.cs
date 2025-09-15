using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniversalText.Core;
using System.Text.RegularExpressions;

public class UniversalTextPrompter : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private InputField inputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI responseText;
    
    [Header("LLM Settings")]
    [SerializeField] private DeepSeekHandler deepSeekHandler;
    [SerializeField] private Llama3Handler llama3Handler;
    [SerializeField] private ChatGPTHandler chatGPTHandler;
    [SerializeField] private GeminiHandler geminiHandler;
    
    // Make the enum public so it can be used in public methods
    public enum LLMType { DeepSeek, Llama3, ChatGPT, Gemini }
    [SerializeField] private LLMType activeLLM = LLMType.Llama3;
    
    [Header("Context Settings")]
    [SerializeField] private float contextUpdateInterval = 1f;
    [SerializeField] private bool includeContext = true;
    
    private float nextContextUpdateTime;
    private string currentRTR = "";

    private void Start()
    {
        Debug.Log("UniversalTextPrompter: Starting...");
        
        // Validate UI components
        if (inputField == null) Debug.LogError("Input Field not assigned!");
        if (submitButton == null) Debug.LogError("Submit Button not assigned!");
        if (responseText == null) Debug.LogError("Response Text not assigned!");
        
        // Find LLM handlers if not assigned
        if (deepSeekHandler == null && activeLLM == LLMType.DeepSeek)
        {
            deepSeekHandler = FindObjectOfType<DeepSeekHandler>();
            if (deepSeekHandler == null) Debug.LogError("DeepSeekHandler not found!");
        }
        
        if (llama3Handler == null && activeLLM == LLMType.Llama3)
        {
            llama3Handler = FindObjectOfType<Llama3Handler>();
            if (llama3Handler == null) Debug.LogError("Llama3Handler not found!");
        }
        
        if (chatGPTHandler == null && activeLLM == LLMType.ChatGPT)
        {
            chatGPTHandler = FindObjectOfType<ChatGPTHandler>();
            if (chatGPTHandler == null) Debug.LogError("ChatGPTHandler not found!");
        }
        
        if (geminiHandler == null && activeLLM == LLMType.Gemini)
        {
            geminiHandler = FindObjectOfType<GeminiHandler>();
            if (geminiHandler == null) Debug.LogError("GeminiHandler not found!");
        }
        
        // Set up button listener
        if (submitButton != null)
            submitButton.onClick.AddListener(FireMessage);
    }

    private void Update()
    {
        if (Time.time >= nextContextUpdateTime)
        {
            UpdateRTR();
            nextContextUpdateTime = Time.time + contextUpdateInterval;
        }
    }

    private void UpdateRTR()
    {
        currentRTR = UniversalTextScanner.Instance.Generate();
        Debug.Log($"Current RTR: {currentRTR}");
    }

    public string GetCurrentContext()
    {
        return currentRTR;
    }
    
    public void FireMessage()
    {
        string userMessage = inputField.text;
        if (string.IsNullOrEmpty(userMessage)) return;

        string fullPrompt = userMessage;
        
        // Add context if enabled
        if (includeContext)
        {
            fullPrompt = $"You are a direct and helpful assistant. Based on this context: {currentRTR}\n" +
                         $"Answer this question concisely: {userMessage}";
        }
        
        Debug.Log($"Sending message: {fullPrompt}");

        // Disable UI while processing
        submitButton.interactable = false;
        inputField.text = "";
        inputField.interactable = false;

        // Choose which handler to use
        switch (activeLLM)
        {
            case LLMType.DeepSeek:
                if (deepSeekHandler != null)
                    deepSeekHandler.SendMessage(fullPrompt, OnResponseReceived);
                else
                    HandleMissingLLM("DeepSeek");
                break;
                
            case LLMType.Llama3:
                if (llama3Handler != null)
                    llama3Handler.SendMessage(fullPrompt, OnResponseReceived);
                else
                    HandleMissingLLM("Llama3");
                break;
                
            case LLMType.ChatGPT:
                if (chatGPTHandler != null)
                    chatGPTHandler.SendMessage(fullPrompt, OnResponseReceived);
                else
                    HandleMissingLLM("ChatGPT");
                break;
                
            case LLMType.Gemini:
                if (geminiHandler != null)
                    geminiHandler.SendMessage(fullPrompt, OnResponseReceived);
                else
                    HandleMissingLLM("Gemini");
                break;
        }
    }
    
    private void HandleMissingLLM(string llmName)
    {
        Debug.LogError($"{llmName} handler not available!");
        responseText.text = $"Error: {llmName} handler not available";
        submitButton.interactable = true;
        inputField.interactable = true;
    }
    
    private void OnResponseReceived(string response)
    {
        Debug.Log($"Received raw response: {response}");
        if (response != null)
        {
            // Clean up the response
            string cleanResponse = CleanResponse(response);
            
            // Update UI
            responseText.text = cleanResponse;
        }
        else
        {
            responseText.text = "Error: Failed to get response";
        }
        
        // Re-enable UI
        submitButton.interactable = true;
        inputField.interactable = true;
    }
    
    private string CleanResponse(string response)
    {
        if (string.IsNullOrEmpty(response))
            return "";
        
        // Remove thinking tags and content
        string cleaned = Regex.Replace(
            response, 
            "<think>[\\s\\S]*?</think>", 
            "", 
            RegexOptions.Singleline
        );
        
        // Remove other tags
        cleaned = cleaned
            .Replace("<assistant>", "")
            .Replace("</assistant>", "")
            .Replace("Let me think about this.", "")
            .Replace("Let me help you with that.", "")
            .Trim();
        
        // Force spaces between words by adding spaces after punctuation
        cleaned = Regex.Replace(cleaned, @"([.,!?;:])([a-zA-Z])", "$1 $2");
        
        // Add spaces between lowercase followed by uppercase (camelCase detection)
        cleaned = Regex.Replace(cleaned, @"([a-z])([A-Z])", "$1 $2");
        
        // Add spaces between letters and numbers
        cleaned = Regex.Replace(cleaned, @"([a-zA-Z])(\d)", "$1 $2");
        cleaned = Regex.Replace(cleaned, @"(\d)([a-zA-Z])", "$1 $2");
        
        // Normalize multiple spaces to single spaces
        cleaned = Regex.Replace(cleaned, @"\s+", " ");
        
        return cleaned;
    }
    
    // Public method to switch between models
    public void SetActiveLLM(LLMType llmType)
    {
        activeLLM = llmType;
        Debug.Log($"Switched to {activeLLM} model");
    }
}
