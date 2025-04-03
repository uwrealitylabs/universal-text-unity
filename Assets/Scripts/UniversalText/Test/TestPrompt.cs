using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestPrompt : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;

    [SerializeField] private bool useGemini = true;

    private float height;

    private void Start()
    {
        button.onClick.AddListener(FireMessage);
    }

    private void AppendMessage(string messageContent, bool isUser)
    {
        var template = isUser ? sent : received;
        var item = Instantiate(template, scroll.content);
        item.gameObject.SetActive(true); 

        var textComponent = item.GetComponentInChildren<Text>();
        if (textComponent != null)
        {
            textComponent.text = messageContent;
        }
        else
        {
            Debug.LogError("Text component not found in the message template.");
            return;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(scroll.content);
        scroll.verticalNormalizedPosition = 0;
    }

    public void FireMessage()
    {
        string message = inputField.text; 
        if (string.IsNullOrEmpty(message)) return;

        AppendMessage(message, isUser: true);

        button.enabled = false;
        inputField.text = "";
        inputField.enabled = false;

        if (useGemini)
        {
            var geminiHandler = FindObjectOfType<GeminiHandler>();
            if (geminiHandler != null)
            {
                geminiHandler.SendMessage(message, response =>
                {
                    Debug.Log("Gemini response received: " + response);
                    AppendMessage(response ?? "Error: No response from Gemini", isUser: false);
                    button.enabled = true;
                    inputField.enabled = true;
                });
            }
            else
            {
                Debug.LogError("GeminiHandler not found in the scene!");
                button.enabled = true;
                inputField.enabled = true;
            }
        }
        else
        {
            var chatGPTHandler = FindObjectOfType<ChatGPTHandler>();
            if (chatGPTHandler != null)
            {
                chatGPTHandler.SendMessage(message, response =>
                {
                    Debug.Log("ChatGPT response received: " + response);
                    AppendMessage(response ?? "Error: No response from ChatGPT", isUser: false);
                    button.enabled = true;
                    inputField.enabled = true;
                });
            }
            else
            {
                Debug.LogError("ChatGPTHandler not found in the scene!");
                button.enabled = true;
                inputField.enabled = true;
            }
        }
    }
}
