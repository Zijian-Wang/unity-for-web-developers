using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class TMPUiFrameRateCounter : MonoBehaviour
    {
        [FormerlySerializedAs("UpdateInterval")] public float m_updateInterval = 5.0f;
        private float m_lastInterval = 0;
        private int m_frames = 0;

        public enum FPSCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        [FormerlySerializedAs("AnchorPosition")] public FPSCounterAnchorPositions m_anchorPosition = FPSCounterAnchorPositions.TopRight;

        private string m_htmlColorTag;
        private const string FPSLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

        private TextMeshProUGUI m_textMeshPro;
        private RectTransform m_frameCounterTransform;

        private FPSCounterAnchorPositions m_lastAnchorPosition;

        void Awake()
        {
            if (!enabled)
                return;

            Application.targetFrameRate = 1000;

            GameObject frameCounter = new GameObject("Frame Counter");
            m_frameCounterTransform = frameCounter.AddComponent<RectTransform>();

            m_frameCounterTransform.SetParent(this.transform, false);

            m_textMeshPro = frameCounter.AddComponent<TextMeshProUGUI>();
            m_textMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            m_textMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");

            m_textMeshPro.enableWordWrapping = false;
            m_textMeshPro.fontSize = 36;

            m_textMeshPro.isOverlay = true;

            Set_FrameCounter_Position(m_anchorPosition);
            m_lastAnchorPosition = m_anchorPosition;
        }


        void Start()
        {
            m_lastInterval = Time.realtimeSinceStartup;
            m_frames = 0;
        }


        void Update()
        {
            if (m_anchorPosition != m_lastAnchorPosition)
                Set_FrameCounter_Position(m_anchorPosition);

            m_lastAnchorPosition = m_anchorPosition;

            m_frames += 1;
            float timeNow = Time.realtimeSinceStartup;

            if (timeNow > m_lastInterval + m_updateInterval)
            {
                // display two fractional digits (f2 format)
                float fps = m_frames / (timeNow - m_lastInterval);
                float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

                if (fps < 30)
                    m_htmlColorTag = "<color=yellow>";
                else if (fps < 10)
                    m_htmlColorTag = "<color=red>";
                else
                    m_htmlColorTag = "<color=green>";

                m_textMeshPro.SetText(m_htmlColorTag + FPSLabel, fps, ms);

                m_frames = 0;
                m_lastInterval = timeNow;
            }
        }


        void Set_FrameCounter_Position(FPSCounterAnchorPositions anchorPosition)
        {
            switch (anchorPosition)
            {
                case FPSCounterAnchorPositions.TopLeft:
                    m_textMeshPro.alignment = TextAlignmentOptions.TopLeft;
                    m_frameCounterTransform.pivot = new Vector2(0, 1);
                    m_frameCounterTransform.anchorMin = new Vector2(0.01f, 0.99f);
                    m_frameCounterTransform.anchorMax = new Vector2(0.01f, 0.99f);
                    m_frameCounterTransform.anchoredPosition = new Vector2(0, 1);
                    break;
                case FPSCounterAnchorPositions.BottomLeft:
                    m_textMeshPro.alignment = TextAlignmentOptions.BottomLeft;
                    m_frameCounterTransform.pivot = new Vector2(0, 0);
                    m_frameCounterTransform.anchorMin = new Vector2(0.01f, 0.01f);
                    m_frameCounterTransform.anchorMax = new Vector2(0.01f, 0.01f);
                    m_frameCounterTransform.anchoredPosition = new Vector2(0, 0);
                    break;
                case FPSCounterAnchorPositions.TopRight:
                    m_textMeshPro.alignment = TextAlignmentOptions.TopRight;
                    m_frameCounterTransform.pivot = new Vector2(1, 1);
                    m_frameCounterTransform.anchorMin = new Vector2(0.99f, 0.99f);
                    m_frameCounterTransform.anchorMax = new Vector2(0.99f, 0.99f);
                    m_frameCounterTransform.anchoredPosition = new Vector2(1, 1);
                    break;
                case FPSCounterAnchorPositions.BottomRight:
                    m_textMeshPro.alignment = TextAlignmentOptions.BottomRight;
                    m_frameCounterTransform.pivot = new Vector2(1, 0);
                    m_frameCounterTransform.anchorMin = new Vector2(0.99f, 0.01f);
                    m_frameCounterTransform.anchorMax = new Vector2(0.99f, 0.01f);
                    m_frameCounterTransform.anchoredPosition = new Vector2(1, 0);
                    break;
            }
        }
    }
}