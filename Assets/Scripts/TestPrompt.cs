using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TestPrompt : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;

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

        var textComponent = item.GetComponentInChildren<TMPro.TextMeshProUGUI>();
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

        FindObjectOfType<ChatGPTHandler>().SendReply(message, response =>
        {
            Debug.Log("Response received: " + response);
            AppendMessage(response, isUser: false);
            button.enabled = true;
            inputField.enabled = true;
        });
    }
}
