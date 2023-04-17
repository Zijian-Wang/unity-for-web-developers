using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;


namespace TMPro.Examples
{

    public class TMPExampleScript01 : MonoBehaviour
    {
        public enum ObjectType { TextMeshPro = 0, TextMeshProUGUI = 1 };

        [FormerlySerializedAs("ObjectType")] public ObjectType m_objectType;
        [FormerlySerializedAs("isStatic")] public bool m_isStatic;

        private TMP_Text m_text;

        //private TMP_InputField m_inputfield;


        private const string KLabel = "The count is <#0080ff>{0}</color>";
        private int m_count;

        void Awake()
        {
            // Get a reference to the TMP text component if one already exists otherwise add one.
            // This example show the convenience of having both TMP components derive from TMP_Text. 
            if (m_objectType == 0)
                m_text = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
            else
                m_text = GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();

            // Load a new font asset and assign it to the text object.
            m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");

            // Load a new material preset which was created with the context menu duplicate.
            m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");

            // Set the size of the font.
            m_text.fontSize = 120;

            // Set the text
            m_text.text = "A <#0080ff>simple</color> line of text.";

            // Get the preferred width and height based on the supplied width and height as opposed to the actual size of the current text container.
            Vector2 size = m_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);

            // Set the size of the RectTransform based on the new calculated values.
            m_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);
        }


        void Update()
        {
            if (!m_isStatic)
            {
                m_text.SetText(KLabel, m_count % 1000);
                m_count += 1;
            }
        }

    }
}
