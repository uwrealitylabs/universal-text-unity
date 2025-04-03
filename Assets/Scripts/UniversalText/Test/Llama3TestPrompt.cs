using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Llama3TestPrompt : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;
    [SerializeField] private Llama3Handler llama3Handler;  // Added reference to Llama3Handler

    private void Start()
    {
        // Add validation logging
        Debug.Log("Llama3TestPrompt: Starting...");
        
        if (inputField == null) Debug.LogError("Input Field not assigned!");
        if (button == null) Debug.LogError("Button not assigned!");
        if (scroll == null) Debug.LogError("Scroll View not assigned!");
        if (sent == null) Debug.LogError("Sent message prefab not assigned!");
        if (received == null) Debug.LogError("Received message prefab not assigned!");
        if (llama3Handler == null)
        {
            llama3Handler = FindObjectOfType<Llama3Handler>();
            if (llama3Handler == null) Debug.LogError("Llama3Handler not found in scene!");
        }

        button.onClick.AddListener(FireMessage);
    }

    private void AppendMessage(string messageContent, bool isUser)
    {
        Debug.Log($"Appending message: {messageContent}");
        
        // Create message instance
        var template = isUser ? sent : received;
        var item = Instantiate(template, scroll.content);
        
        // Add layout element if it doesn't exist
        if (!item.TryGetComponent<LayoutElement>(out var layoutElement))
        {
            layoutElement = item.gameObject.AddComponent<LayoutElement>();
        }
        layoutElement.minHeight = 30;
        layoutElement.preferredWidth = 300;
        layoutElement.flexibleWidth = 0;  // Don't stretch
        
        // Position message
        var rectTransform = item.GetComponent<RectTransform>();
        if (isUser)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.pivot = new Vector2(1, 0);
            rectTransform.anchoredPosition = new Vector2(-10, 0);
        }
        else
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0, 0);
            rectTransform.anchoredPosition = new Vector2(10, 0);
        }
        
        // Set text
        var textComponent = item.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = messageContent;
        }

        // Update layout
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(scroll.content);
        scroll.verticalNormalizedPosition = 0;
    }

    public void FireMessage()
    {
        string message = inputField.text;
        if (string.IsNullOrEmpty(message)) return;

        Debug.Log($"Sending message: {message}");
        AppendMessage(message, isUser: true);

        button.interactable = false;
        inputField.text = "";
        inputField.interactable = false;

        llama3Handler.SendMessage(message, response =>
        {
            Debug.Log($"Received response: {response}");
            if (response != null)
            {
                AppendMessage(response, isUser: false);
            }
            else
            {
                AppendMessage("Error: Failed to get response from Llama3", isUser: false);
            }
            button.interactable = true;
            inputField.interactable = true;
        });
    }
} 