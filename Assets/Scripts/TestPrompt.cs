using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TestPrompt : MonoBehaviour
{
    // Variables for the Canvas (testing)
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

    // Append a message to the UI
    /*
    private void AppendMessage(string messageContent, bool isUser)
    {
        scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        // Instantiate the correct UI element (sent or received) based on the message type
        var item = Instantiate(isUser ? sent : received, scroll.content);
        item.GetChild(0).GetChild(0).GetComponent<Text>().text = messageContent;
        item.anchoredPosition = new Vector2(0, -height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        height += item.sizeDelta.y;
        scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        scroll.verticalNormalizedPosition = 0;
    }
    */

    private void AppendMessage(string messageContent, bool isUser)
    {
        // Instantiate the correct template (sent or received)
        var template = isUser ? sent : received;
        var item = Instantiate(template, scroll.content);
        item.gameObject.SetActive(true); // Ensure the item is active

        // Access the Text component safely
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

        // Adjust layout and scroll position
        LayoutRebuilder.ForceRebuildLayoutImmediate(scroll.content);
        scroll.verticalNormalizedPosition = 0;
    }

    public void FireMessage()
    {
        string message = inputField.text; // Use input field content as the message
        if (string.IsNullOrEmpty(message)) return;

        // Display user message in the UI
        AppendMessage(message, isUser: true);

        // Disable button and input field while waiting for response
        button.enabled = false;
        inputField.text = "";
        inputField.enabled = false;

        // Send message to ChatGPTHandler and handle the response
        FindObjectOfType<ChatGPTHandler>().SendReply(message, response =>
        {
            Debug.Log("Response received: " + response);
            AppendMessage(response, isUser: false); // Append the response as an assistant message
            button.enabled = true;
            inputField.enabled = true;
        });
    }
}
