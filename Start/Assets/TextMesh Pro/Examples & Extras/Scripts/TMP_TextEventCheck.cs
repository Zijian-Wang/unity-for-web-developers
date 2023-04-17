using UnityEngine;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    public class TMPTextEventCheck : MonoBehaviour
    {

        [FormerlySerializedAs("TextEventHandler")] public TMPTextEventHandler m_textEventHandler;

        private TMP_Text m_textComponent;

        void OnEnable()
        {
            if (m_textEventHandler != null)
            {
                // Get a reference to the text component
                m_textComponent = m_textEventHandler.GetComponent<TMP_Text>();
                
                m_textEventHandler.OnCharacterSelection.AddListener(OnCharacterSelection);
                m_textEventHandler.OnSpriteSelection.AddListener(OnSpriteSelection);
                m_textEventHandler.OnWordSelection.AddListener(OnWordSelection);
                m_textEventHandler.OnLineSelection.AddListener(OnLineSelection);
                m_textEventHandler.OnLinkSelection.AddListener(OnLinkSelection);
            }
        }


        void OnDisable()
        {
            if (m_textEventHandler != null)
            {
                m_textEventHandler.OnCharacterSelection.RemoveListener(OnCharacterSelection);
                m_textEventHandler.OnSpriteSelection.RemoveListener(OnSpriteSelection);
                m_textEventHandler.OnWordSelection.RemoveListener(OnWordSelection);
                m_textEventHandler.OnLineSelection.RemoveListener(OnLineSelection);
                m_textEventHandler.OnLinkSelection.RemoveListener(OnLinkSelection);
            }
        }


        void OnCharacterSelection(char c, int index)
        {
            Debug.Log("Character [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnSpriteSelection(char c, int index)
        {
            Debug.Log("Sprite [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnWordSelection(string word, int firstCharacterIndex, int length)
        {
            Debug.Log("Word [" + word + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLineSelection(string lineText, int firstCharacterIndex, int length)
        {
            Debug.Log("Line [" + lineText + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLinkSelection(string linkID, string linkText, int linkIndex)
        {
            if (m_textComponent != null)
            {
                TMP_LinkInfo linkInfo = m_textComponent.textInfo.linkInfo[linkIndex];
            }
            
            Debug.Log("Link Index: " + linkIndex + " with ID [" + linkID + "] and Text \"" + linkText + "\" has been selected.");
        }

    }
}
