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
        // Validate all required components
        bool hasErrors = false;
        
        if (inputField == null)
        {
            Debug.LogError("Input Field reference not set on Llama3TestPrompt!");
            hasErrors = true;
        }
        
        if (button == null)
        {
            Debug.LogError("Button reference not set on Llama3TestPrompt!");
            hasErrors = true;
        }
        
        if (scroll == null)
        {
            Debug.LogError("Scroll Rect reference not set on Llama3TestPrompt!");
            hasErrors = true;
        }
        
        if (sent == null)
        {
            Debug.LogError("Sent message prefab reference not set on Llama3TestPrompt!");
            hasErrors = true;
        }
        
        if (received == null)
        {
            Debug.LogError("Received message prefab reference not set on Llama3TestPrompt!");
            hasErrors = true;
        }

        // Try to find Llama3Handler if not assigned
        if (llama3Handler == null)
        {
            llama3Handler = FindObjectOfType<Llama3Handler>();
            if (llama3Handler == null)
            {
                Debug.LogError("Llama3Handler not found in scene!");
                hasErrors = true;
            }
        }

        // Disable button if any errors
        if (hasErrors)
        {
            if (button != null)
            {
                button.interactable = false;
            }
            return;
        }

        // Add button listener if everything is valid
        button.onClick.AddListener(FireMessage);
    }

    private void AppendMessage(string messageContent, bool isUser)
    {
        Debug.Log($"Appending {(isUser ? "sent" : "received")} message: {messageContent}");
        
        // Validate prefab references
        if (sent == null || received == null)
        {
            Debug.LogError("Message prefabs not assigned!");
            return;
        }

        // Create message instance
        var template = isUser ? sent : received;
        var item = Instantiate(template, scroll.content);
        
        // Find TextMeshProUGUI in the root or children
        var textComponent = item.GetComponent<TMPro.TextMeshProUGUI>();
        if (textComponent == null)
        {
            textComponent = item.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }
        
        if (textComponent != null)
        {
            textComponent.SetText(messageContent);
            Debug.Log($"Successfully set message text to: {messageContent}");
        }
        else
        {
            Debug.LogError($"TextMeshProUGUI component not found in {(isUser ? "sent" : "received")} message prefab! " +
                          "Make sure to create the prefab using UI → Text - TextMeshPro");
            return;
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