using System;
using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// This script is to test submitting prompts and receiving responses
public class TestPrompt : MonoBehaviour
{
    // Variables for the Canvas (testing)
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;

    private float height;
    private void Start()
    {
        button.onClick.AddListener(FireMessage);
    }

     private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

   public void FireMessage()
    {
        String message = "What is 8 + 8"; // Test message

        button.enabled = false;
        inputField.text = "";
        inputField.enabled = false;

        FindObjectOfType<ChatGPTHandler>().SendReply(message, response => // Accept the callback
            {
                Debug.Log("Response received: " + response);
                AppendMessage(response); // Append message to the canvas
            });

        button.enabled = true;
        inputField.enabled = true;
    }
}
