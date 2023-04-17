using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Serialization;


namespace TMPro
{

    public class TMPTextEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Serializable]
        public class CharacterSelectionEvent : UnityEvent<char, int> { }

        [Serializable]
        public class SpriteSelectionEvent : UnityEvent<char, int> { }

        [Serializable]
        public class WordSelectionEvent : UnityEvent<string, int, int> { }

        [Serializable]
        public class LineSelectionEvent : UnityEvent<string, int, int> { }

        [Serializable]
        public class LinkSelectionEvent : UnityEvent<string, string, int> { }


        /// <summary>
        /// Event delegate triggered when pointer is over a character.
        /// </summary>
        public CharacterSelectionEvent OnCharacterSelection
        {
            get { return m_onCharacterSelection; }
            set { m_onCharacterSelection = value; }
        }
        [FormerlySerializedAs("m_OnCharacterSelection")] [SerializeField]
        private CharacterSelectionEvent m_onCharacterSelection = new CharacterSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a sprite.
        /// </summary>
        public SpriteSelectionEvent OnSpriteSelection
        {
            get { return m_onSpriteSelection; }
            set { m_onSpriteSelection = value; }
        }
        [FormerlySerializedAs("m_OnSpriteSelection")] [SerializeField]
        private SpriteSelectionEvent m_onSpriteSelection = new SpriteSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a word.
        /// </summary>
        public WordSelectionEvent OnWordSelection
        {
            get { return m_onWordSelection; }
            set { m_onWordSelection = value; }
        }
        [FormerlySerializedAs("m_OnWordSelection")] [SerializeField]
        private WordSelectionEvent m_onWordSelection = new WordSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a line.
        /// </summary>
        public LineSelectionEvent OnLineSelection
        {
            get { return m_onLineSelection; }
            set { m_onLineSelection = value; }
        }
        [FormerlySerializedAs("m_OnLineSelection")] [SerializeField]
        private LineSelectionEvent m_onLineSelection = new LineSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a link.
        /// </summary>
        public LinkSelectionEvent OnLinkSelection
        {
            get { return m_onLinkSelection; }
            set { m_onLinkSelection = value; }
        }
        [FormerlySerializedAs("m_OnLinkSelection")] [SerializeField]
        private LinkSelectionEvent m_onLinkSelection = new LinkSelectionEvent();



        private TMP_Text m_textComponent;

        private Camera m_camera;
        private Canvas m_canvas;

        private int m_selectedLink = -1;
        private int m_lastCharIndex = -1;
        private int m_lastWordIndex = -1;
        private int m_lastLineIndex = -1;

        void Awake()
        {
            // Get a reference to the text component.
            m_textComponent = gameObject.GetComponent<TMP_Text>();

            // Get a reference to the camera rendering the text taking into consideration the text component type.
            if (m_textComponent.GetType() == typeof(TextMeshProUGUI))
            {
                m_canvas = gameObject.GetComponentInParent<Canvas>();
                if (m_canvas != null)
                {
                    if (m_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        m_camera = null;
                    else
                        m_camera = m_canvas.worldCamera;
                }
            }
            else
            {
                m_camera = Camera.main;
            }
        }


        void LateUpdate()
        {
            if (TMP_TextUtilities.IsIntersectingRectTransform(m_textComponent.rectTransform, Input.mousePosition, m_camera))
            {
                #region Example of Character or Sprite Selection
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_textComponent, Input.mousePosition, m_camera, true);
                if (charIndex != -1 && charIndex != m_lastCharIndex)
                {
                    m_lastCharIndex = charIndex;

                    TMP_TextElementType elementType = m_textComponent.textInfo.characterInfo[charIndex].elementType;

                    // Send event to any event listeners depending on whether it is a character or sprite.
                    if (elementType == TMP_TextElementType.Character)
                        SendOnCharacterSelection(m_textComponent.textInfo.characterInfo[charIndex].character, charIndex);
                    else if (elementType == TMP_TextElementType.Sprite)
                        SendOnSpriteSelection(m_textComponent.textInfo.characterInfo[charIndex].character, charIndex);
                }
                #endregion


                #region Example of Word Selection
                // Check if Mouse intersects any words and if so assign a random color to that word.
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_textComponent, Input.mousePosition, m_camera);
                if (wordIndex != -1 && wordIndex != m_lastWordIndex)
                {
                    m_lastWordIndex = wordIndex;

                    // Get the information about the selected word.
                    TMP_WordInfo wInfo = m_textComponent.textInfo.wordInfo[wordIndex];

                    // Send the event to any listeners.
                    SendOnWordSelection(wInfo.GetWord(), wInfo.firstCharacterIndex, wInfo.characterCount);
                }
                #endregion


                #region Example of Line Selection
                // Check if Mouse intersects any words and if so assign a random color to that word.
                int lineIndex = TMP_TextUtilities.FindIntersectingLine(m_textComponent, Input.mousePosition, m_camera);
                if (lineIndex != -1 && lineIndex != m_lastLineIndex)
                {
                    m_lastLineIndex = lineIndex;

                    // Get the information about the selected word.
                    TMP_LineInfo lineInfo = m_textComponent.textInfo.lineInfo[lineIndex];

                    // Send the event to any listeners.
                    char[] buffer = new char[lineInfo.characterCount];
                    for (int i = 0; i < lineInfo.characterCount && i < m_textComponent.textInfo.characterInfo.Length; i++)
                    {
                        buffer[i] = m_textComponent.textInfo.characterInfo[i + lineInfo.firstCharacterIndex].character;
                    }

                    string lineText = new string(buffer);
                    SendOnLineSelection(lineText, lineInfo.firstCharacterIndex, lineInfo.characterCount);
                }
                #endregion


                #region Example of Link Handling
                // Check if mouse intersects with any links.
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_textComponent, Input.mousePosition, m_camera);

                // Handle new Link selection.
                if (linkIndex != -1 && linkIndex != m_selectedLink)
                {
                    m_selectedLink = linkIndex;

                    // Get information about the link.
                    TMP_LinkInfo linkInfo = m_textComponent.textInfo.linkInfo[linkIndex];

                    // Send the event to any listeners.
                    SendOnLinkSelection(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkIndex);
                }
                #endregion
            }
            else
            {
                // Reset all selections given we are hovering outside the text container bounds.
                m_selectedLink = -1;
                m_lastCharIndex = -1;
                m_lastWordIndex = -1;
                m_lastLineIndex = -1;
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter()");
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit()");
        }


        private void SendOnCharacterSelection(char character, int characterIndex)
        {
            if (OnCharacterSelection != null)
                OnCharacterSelection.Invoke(character, characterIndex);
        }

        private void SendOnSpriteSelection(char character, int characterIndex)
        {
            if (OnSpriteSelection != null)
                OnSpriteSelection.Invoke(character, characterIndex);
        }

        private void SendOnWordSelection(string word, int charIndex, int length)
        {
            if (OnWordSelection != null)
                OnWordSelection.Invoke(word, charIndex, length);
        }

        private void SendOnLineSelection(string line, int charIndex, int length)
        {
            if (OnLineSelection != null)
                OnLineSelection.Invoke(line, charIndex, length);
        }

        private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
        {
            if (OnLinkSelection != null)
                OnLinkSelection.Invoke(linkID, linkText, linkIndex);
        }

    }
}
