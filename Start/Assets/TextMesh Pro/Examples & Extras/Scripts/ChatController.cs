using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ChatController : MonoBehaviour {


    [FormerlySerializedAs("ChatInputField")] public TMP_InputField m_chatInputField;

    [FormerlySerializedAs("ChatDisplayOutput")] public TMP_Text m_chatDisplayOutput;

    [FormerlySerializedAs("ChatScrollbar")] public Scrollbar m_chatScrollbar;

    void OnEnable()
    {
        m_chatInputField.onSubmit.AddListener(AddToChatOutput);
    }

    void OnDisable()
    {
        m_chatInputField.onSubmit.RemoveListener(AddToChatOutput);
    }


    void AddToChatOutput(string newText)
    {
        // Clear Input Field
        m_chatInputField.text = string.Empty;

        var timeNow = System.DateTime.Now;

        string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText;

        if (m_chatDisplayOutput != null)
        {
            // No special formatting for first entry
            // Add line feed before each subsequent entries
            if (m_chatDisplayOutput.text == string.Empty)
                m_chatDisplayOutput.text = formattedInput;
            else
                m_chatDisplayOutput.text += "\n" + formattedInput;
        }

        // Keep Chat input field active
        m_chatInputField.ActivateInputField();

        // Set the scrollbar to the bottom when next text is submitted.
        m_chatScrollbar.value = 0;
    }

}
